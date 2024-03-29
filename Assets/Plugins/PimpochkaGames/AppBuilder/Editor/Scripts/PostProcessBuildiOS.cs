#if UNITY_IOS || UNITY_IPHONE

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections.Generic;
using System.IO;

namespace PimpochkaGames.AppBuilder.Editor
{
public class PostProcessBuildiOS
{
    // Set the IDFA request description:
    const string k_TrackingDescription = "This app includes ads. To improve your experience and see ads that match your interests, allow tracking.";

    private const string FileNameI2Localization = "I2Localization";
#if !UNITY_2019_3_OR_NEWER
        private const string UnityMainTargetName = "Unity-iPhone";
#endif

    [PostProcessBuildAttribute(int.MaxValue)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget == BuildTarget.iOS) 
        {
            AddPListValues(buildPath);

        var projectPath = PBXProject.GetPBXProjectPath(buildPath);
        var project = new PBXProject();
        project.ReadFromFile(projectPath);

#if UNITY_2019_3_OR_NEWER
        var unityMainTargetGuid = project.GetUnityMainTargetGuid();
        var unityFrameworkTargetGuid = project.GetUnityFrameworkTargetGuid();
#else
            var unityMainTargetGuid = project.TargetGuidByName(UnityMainTargetName);
            var unityFrameworkTargetGuid = project.TargetGuidByName(UnityMainTargetName);
#endif

        DisableBitcode(project);
        SetLocalizationText(buildPath, project, "en", "This app includes ads. To improve your experience and see ads that match your interests, allow tracking.");
        SetLocalizationText(buildPath, project, "ru", "Это приложение включает в себя рекламу. Чтобы улучшить ваш опыт и видеть рекламу, которая соответствует вашим интересам, разрешите отслеживание.");
        SetLocalizationText(buildPath, project, "uk", "Ця програма включає рекламу. Щоб покращити ваш досвід та бачити рекламу, яка відповідає вашим інтересам, дозвольте відстеження.");
        SetLocalizationText(buildPath, project, "kk", "Бұл қолданбада жарнамалар бар. Тәжірибеңізді жақсарту және қызығушылықтарыңызға сәйкес келетін жарнамаларды көру үшін бақылауға рұқсат етіңіз.");
        project.WriteToFile(projectPath);
        }
    }

    private static void AddPListValues(string pathToXcode) 
    {
        // Retrieve the plist file from the Xcode project directory:
        string plistPath = pathToXcode + "/Info.plist";
        PlistDocument plistObj = new PlistDocument();
 
 
        // Read the values from the plist file:
        plistObj.ReadFromString(File.ReadAllText(plistPath));
 
        // Set values from the root object:
        PlistElementDict plistRoot = plistObj.root;
 
        // Set the description key-value in the plist:
        plistRoot.SetString("NSUserTrackingUsageDescription", k_TrackingDescription);
 
        // Save changes to the plist:
        File.WriteAllText(plistPath, plistObj.WriteToString());
    }

    private static void SetLocalizationText(string buildPath, PBXProject project, string localeCode, string localizedUserTrackingDescription)
    {
        string i2LocalizationFilePath = Path.Combine(buildPath, FileNameI2Localization);
        var i2LocalizationFileDetected = Directory.Exists(i2LocalizationFilePath);
        if (!i2LocalizationFileDetected)
        {
            Debug.LogError("I2Localization failed! Path: " + i2LocalizationFilePath);
            return;
        }

        var localizationFilePath = Path.Combine(buildPath, FileNameI2Localization);
        var localeSpecificDirectoryName = localeCode + ".lproj";
        var localeSpecificDirectoryPath = Path.Combine(localizationFilePath, localeSpecificDirectoryName);
        var infoPlistStringsFilePath = Path.Combine(localeSpecificDirectoryPath, "InfoPlist.strings");

        var localizedDescriptionLine = "NSUserTrackingUsageDescription = \"" + localizedUserTrackingDescription + "\";\n";

        // File already exists, update it in case the value changed between builds.
        if (File.Exists(infoPlistStringsFilePath))
        {
            var output = new List<string>();
            var lines = File.ReadAllLines(infoPlistStringsFilePath);
            var keyUpdated = false;
            foreach (var line in lines)
            {
                if (line.Contains("NSUserTrackingUsageDescription"))
                {
                    output.Add(localizedDescriptionLine);
                    keyUpdated = true;
                }
                else
                {
                    output.Add(line);
                }
            }

            if (!keyUpdated)
                output.Add(localizedDescriptionLine);

            File.WriteAllText(infoPlistStringsFilePath, string.Join("\n", output.ToArray()) + "\n");
        }
        // File doesn't exist, create one.
        else
        {
            UnityEngine.Debug.Log("This language not found! " + localeCode);
            //File.WriteAllText(infoPlistStringsFilePath, "/* Localized versions of Info.plist keys - Generated by AppBuilder */\n" + localizedDescriptionLine);
        }
    }

    private static void DisableBitcode(PBXProject pbxProject)
    {
        //Main
        string target = pbxProject.GetUnityMainTargetGuid();
        pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
        //Unity Tests
        target = pbxProject.TargetGuidByName(PBXProject.GetUnityTestTargetName());
        pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
        //Unity Framework
        target = pbxProject.GetUnityFrameworkTargetGuid();
        pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
    }
}
}
#endif