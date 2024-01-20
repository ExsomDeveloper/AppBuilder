namespace AppBuilder
{
    public interface IVersionControl
    {
        string GetAppVersion();
        void SetStagesOfDevelopment(StagesOfDevelopment stagesOfDevelopment);
    }
}