using PimpochkaGames.NeutralAgeScreen;

namespace AppBuilder
{
    public class NeutralAgeScreenModule : Module<NeutralAgeScreenModule, NeutralAgeScreenConfig>
    {
        public static NeutralAgeScreenManager Agent { get; private set; }

        protected override void OnInitialize(NeutralAgeScreenConfig config)
        {
            Agent = Instantiate(config.NeutralAgeScreenManagerPrefab, gameObject.transform);
        }
    }
}

namespace AppBuilder
{
    [System.Serializable]
    public class NeutralAgeScreenConfig : ModuleConfig
    {
        public bool AutoStart = true;
        public NeutralAgeScreenManager NeutralAgeScreenManagerPrefab;
    }
}
