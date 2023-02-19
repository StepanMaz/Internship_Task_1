using Configuration;
using Files;
using System.IO;
using System.Text.Json;
using System.Timers;

namespace Application 
{
    public class App
    {
        IAppConfig _conf;
        IDataShaper _shaper;

        int file_count = 1;
        DateOnly today;
        System.Timers.Timer _timer;

        Meta _meta = new Meta();

        public SourceWatcher txtWatcher, csvWatcher;
        public CancellationTokenSource CTsource = new CancellationTokenSource();

        public App(IDataShaper shaper)
        {
            _conf = new ConfigurationManager("config.json").GetConfig();
            _shaper = shaper;

            SetTimer();
        }

        private async Task SetTimer() {
            await Task.Delay((int)(DateTime.Today.AddDays(1) - DateTime.Now).TotalMilliseconds);
            MidnightSaveFiles();
            _timer = new (1000 * 60 * 60 *24);
            _timer.Elapsed += (s, e) => MidnightSaveFiles();
        }

        private void MidnightSaveFiles() {
            file_count = 1;
            File.CreateText(GetOutputTimedFolder() + "meta.log").WriteLine(
                JsonSerializer.Serialize(_meta, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                })
            );
            _meta = new Meta();
            today = DateOnly.FromDateTime(DateTime.Today);
        }

        private string GetOutputTimedFolder() => string.Format("{0}\\{1}", _conf.GetOutputDirectory(), DateTime.Today.ToString("dd_MM-yyyy"));
        private string GetFileName() => string.Format("{0}\\output{1}.json", GetOutputTimedFolder(), file_count);

        public void Run()
        {
            txtWatcher = new SourceWatcher(new TXTFileReader(), Path.GetFullPath(_conf.GetInputDirectory()), "*.txt", CTsource.Token);
            txtWatcher.OnFileParsed += OnFileParsed;
            csvWatcher = new SourceWatcher(new CSVFileReader(), Path.GetFullPath(_conf.GetInputDirectory()), "*.csv", CTsource.Token);
            csvWatcher.OnFileParsed += OnFileParsed;
        }

        public async void OnFileParsed(object sender, ParsingResult result)
        {
            if(result.isInvalid)
                _meta.invalid_files.Add(result.file_path);
            _meta.found_errors += result.failed_lines;
            _meta.parsed_lines += result.lines;
            _meta.parsed_files++;

            string file_name = GetFileName();
            string res = _shaper.TransformData(result.details);
            await File.WriteAllTextAsync(file_name, res);
        }

        public void Stop() 
        {
            CTsource.Cancel();
        }
    }
}