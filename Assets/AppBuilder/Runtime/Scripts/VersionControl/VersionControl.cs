using UnityEditor;
using UnityEngine;

namespace AppBuilder
{
    public class VersionControl //: IVersionControl
    {
        //private const string PRE_ALPHA = "pre-alpha";
        //private const string ALPHA = "alpha";
        //private const string BETA = "beta";
        //private const string RELEASE_CANDIDATE = "release-candidate";
        //private const string RELEASE = "release";

        //private StagesOfDevelopment _stagesOfDevelopment = StagesOfDevelopment.PreAlpha;
        //private string _buildTag;

        //public string GetAppVersion()
        //{
        //    string fullBuildName = $"{GetProjectName()}-v.{Application.version}-b.{PlayerSettings.Android.bundleVersionCode}-{GetStagesOfDevelopmentString()}";

        //    if (!string.IsNullOrEmpty(_buildTag))
        //        fullBuildName += "-" + _buildTag;

        //    return fullBuildName;
        //}

        //public void SetStagesOfDevelopment(StagesOfDevelopment stagesOfDevelopment)
        //{
        //    _stagesOfDevelopment = stagesOfDevelopment;
        //}

        //public void SetBuildTag(string tag)
        //{
        //    _buildTag = tag;
        //}

        //private string GetProjectName()
        //{
        //    string[] s = Application.dataPath.Split('/');
        //    string projectName = s[s.Length - 2];
        //    return projectName;
        //}

        //private string GetStagesOfDevelopmentString()
        //{
        //    return _stagesOfDevelopment switch
        //    {
        //        StagesOfDevelopment.PreAlpha => PRE_ALPHA,
        //        StagesOfDevelopment.Alpha => ALPHA,
        //        StagesOfDevelopment.Beta => BETA,
        //        StagesOfDevelopment.ReleaseCandidate => RELEASE_CANDIDATE,
        //        StagesOfDevelopment.Release => RELEASE,
        //        _ => PRE_ALPHA
        //    };
        //}
    }
}
