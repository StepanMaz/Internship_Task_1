namespace Configuration
{
    public interface IAppConfig
    {
        public string GetInputDirectory();
        public string GetOutputDirectory();
        public string GetLogDirectory();
    }
}