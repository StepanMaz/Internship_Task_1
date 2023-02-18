using System.Text.Json;

namespace Configuration
{
    public class ConfigurationManager
    {
        public readonly string Path;
        private Config _config;

        public ConfigurationManager(string path) 
        {
            Path = path;
        }

        private void ParseConfig() {
            string conf = File.ReadAllText(Path);
            _config = JsonSerializer.Deserialize<Config>(conf);
        }

        public IAppConfig GetConfig() {
            if(!IsFileExists())
            {
                throw new FileNotFoundException("Config file was not found");
            }

            ParseConfig();

            return _config;
        }

        private bool IsFileExists() {
            return File.Exists(Path);
        }
    }
}