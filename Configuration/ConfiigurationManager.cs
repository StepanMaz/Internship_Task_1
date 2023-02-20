using System.Text.Json;

namespace Configuration
{
    public class ConfigurationManager
    {
        public readonly string Path;
        private IAppConfig _config;

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

            try
            {
                ParseConfig();
            }
            catch (JsonException e) {
                throw new Exception("Config is not in correct format", e);
            }

            if(!ValidateConfig(_config)) 
                throw new Exception("Some important field/s is/are empty");
            
            if(!DirectoryExists(_config.GetInputDirectory()))
                throw new Exception("Input directory does not exist");

            if(!DirectoryExists(_config.GetOutputDirectory()))
                throw new Exception("Output directory does not exist");

            return _config;
        }

        private bool ValidateConfig(IAppConfig config) {
            if(string.IsNullOrEmpty(config.GetInputDirectory())) return false;
            if(string.IsNullOrEmpty(config.GetOutputDirectory())) return false;
            return true;
        }

        private bool DirectoryExists(string path) {
            return Directory.Exists(path);
        }

        private bool IsFileExists() {
            return File.Exists(Path);
        }
    }
}