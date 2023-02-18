using Configuration;
using Files;

namespace Application 
{
    public class App
    {
        IAppConfig _conf;

        public SourceWatcher txtWatcher, csvWatcher;
        public CancellationTokenSource CTsource = new CancellationTokenSource();

        public App()
        {
            _conf = new ConfigurationManager("config.json").GetConfig();
        }

        public void Run()
        {
            txtWatcher = new SourceWatcher(new TXTFileReader(), _conf.GetInputDirectory(), ".txt", CTsource.Token);
            txtWatcher.OnFileParsed += OnFileParsed;
            csvWatcher = new SourceWatcher(new CSVFileReader(), _conf.GetInputDirectory(), ".csv", CTsource.Token);
            txtWatcher.OnFileParsed += OnFileParsed;
        }

        public async void OnFileParsed(object sender, ParsingResult result)
        {
            
        }

        public void Stop() 
        {
            
        }
    }
}