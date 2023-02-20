using Application;
using System.IO;
using System.Collections.Concurrent;
using Entities;

namespace Files
{
    public class SourceWatcher
    {
        private CancellationToken _cancellationToken;
        private FileReader _strategy;
        private string _directory_path;
        private string _filter;

        private FileSystemWatcher _watcher;

        public event Action<SourceWatcher, ParsingResult> OnFileParsed;

        public SourceWatcher(FileReader strategy, string directory_path, string filter, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _directory_path = directory_path;
            _strategy = strategy;
            _filter = filter;

            Start();
        }

        private void Start()
        {
            _watcher = new (_directory_path);
            _watcher.Filter = _filter;
            _watcher.NotifyFilter = NotifyFilters.Attributes
                              | NotifyFilters.CreationTime
                              | NotifyFilters.DirectoryName
                              | NotifyFilters.FileName
                              | NotifyFilters.LastAccess
                              | NotifyFilters.LastWrite
                              | NotifyFilters.Security
                              | NotifyFilters.Size;
            _watcher.Created += OnFileCreated;
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
        }

        private async void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            if(!_cancellationToken.IsCancellationRequested)
                OnFileParsed?.Invoke(this, await _strategy.ReadData(e.FullPath, _cancellationToken));
        }

        public void Continue()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
        }
    }
}