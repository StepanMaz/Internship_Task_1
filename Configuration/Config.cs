namespace Configuration
{
    public class Config : IAppConfig
    {
        public string input {get; set;}
        public string output {get; set;}
        public string log {get; set;}

        public string GetInputDirectory() => input;

        public string GetOutputDirectory() => output;

        public string GetLogDirectory() => log;
    }
}