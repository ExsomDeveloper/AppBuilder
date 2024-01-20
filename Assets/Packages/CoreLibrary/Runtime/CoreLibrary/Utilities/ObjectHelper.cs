namespace PimpochkaGames.CoreLibrary
{
    public static class ObjectHelper
    {
        public static T CreateInstanceIfNull<T>(ref T reference, System.Func<T> createFunc) where T : class
        {
            if (reference == null)
            {
                reference = createFunc();
            }

            return reference;
        }
    }
}