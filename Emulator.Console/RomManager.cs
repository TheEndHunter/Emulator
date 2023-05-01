using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Emulator.App
{

    public delegate void DirectoryFileChanged();
    internal class DirectoryWatcher : IDisposable
    {
        public DirectoryWatcher(string path)
        {
            var full = Path.GetFullPath(path);
            _binWatcher = new FileSystemWatcher(full, "*.bin")
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size
            };

            _binWatcher.Changed += OnChange;
            _binWatcher.Deleted += OnChange;
            _binWatcher.Renamed += OnChange;
            _binWatcher.Created += OnChange;
            _binWatcher.Error += OnError;

            _romWatcher = new FileSystemWatcher(full)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size
            };
            _romWatcher.Changed += OnChange;
            _romWatcher.Deleted += OnChange;
            _romWatcher.Renamed += OnChange;
            _romWatcher.Created += OnChange;
            _romWatcher.Error += OnError;
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Console.Error.WriteLine(e.ToString());
            throw e.GetException();
        }

        private void OnChange(object sender, FileSystemEventArgs e)
        {
            FileChanged?.Invoke();
        }

        private readonly FileSystemWatcher _binWatcher;
        private readonly FileSystemWatcher _romWatcher;
        private bool disposedValue;

        public event DirectoryFileChanged? FileChanged;


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _romWatcher.Changed -= OnChange;
                _romWatcher.Deleted -= OnChange;
                _romWatcher.Renamed -= OnChange;
                _romWatcher.Created -= OnChange;
                _romWatcher.Error -= OnError;
                _binWatcher.Changed -= OnChange;
                _binWatcher.Deleted -= OnChange;
                _binWatcher.Renamed -= OnChange;
                _binWatcher.Created -= OnChange;
                _binWatcher.Error -= OnError;

                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                _binWatcher.Dispose();
                _romWatcher.Dispose();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~DirectoryWatcher()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    internal class RomManager
    {
        private readonly string _romDir;
        private readonly DirectoryWatcher _watcher;
        private object _lock;
        private List<(string Name, string FullPath)> _fileList;
        private int _exitIndex;
        private string _fileMenu;
        public int FileCount
        {
            get
            {
                try
                {
                    Monitor.Enter(_lock);
                    Monitor.Wait(_lock, 10);
                    return _fileList.Count;
                }
                finally
                {
                    Monitor.PulseAll(_lock);
                    Monitor.Exit(_lock);
                }
            }
        }

        public int ExitIndex
        {
            get
            {
                try
                {
                    Monitor.Enter(_lock);
                    Monitor.Wait(_lock, 10);
                    return _exitIndex;
                }
                finally
                {
                    Monitor.PulseAll(_lock);
                    Monitor.Exit(_lock);
                }
            }
        }

        public readonly string DisassemblyDir;

        public bool RequiresUpdate { get; set; }

        public RomManager(string romDir = "Roms")
        {
            _lock = new object();
            _romDir = Path.GetFullPath(romDir);
            _watcher = new(_romDir);
            _watcher.FileChanged += OnChange;

            DisassemblyDir = Path.GetFullPath(Path.Combine(_romDir, "Disassembly"));

            RequiresUpdate = true;
            OnChange();
            RequiresUpdate = false;

        }

        public string GetMenu()
        {
            try
            {
                Monitor.Enter(_lock);
                Monitor.Wait(_lock, 10);
                return _fileMenu;
            }
            finally
            {
                Monitor.PulseAll(_lock);
                Monitor.Exit(_lock);
            }
        }

        public (string Name, string FullPath)? GetFileByIndex(int index)
        {
            try
            {
                Monitor.Enter(_lock);
                Monitor.Wait(_lock, 10);
                if (index < 0 || index >= _fileList.Count || _fileList.Count < 1) return null;

                return _fileList[index];
            }
            finally
            {
                Monitor.PulseAll(_lock);
                Monitor.Exit(_lock);
            }
        }

        private void OnChange()
        {
            try
            {
                Monitor.Enter(_lock);
                Monitor.Wait(_lock, 10);
                List<(string Name, string FullPath)> list = new();
                StringBuilder sb = new();
                int fileIndex = 0;
                foreach (var file in Directory.EnumerateFiles(_romDir))
                {
                    var t = new FileInfo(file);
                    var s = t.Extension.ToLower();
                    if (s.Contains("bin") || s.Contains("rom"))
                    {
                        sb.AppendFormat("{0}: {1}{2}", fileIndex, t.Name, Environment.NewLine);
                        list.Add(new(t.Name.Remove(t.Name.IndexOf('.')), file));
                        fileIndex++;
                    }
                }
                _exitIndex = list.Count;
                _fileList = list;
                sb.AppendFormat("{0}: Exit\n", ExitIndex);
                _fileMenu = sb.ToString();
                sb.Clear();
                RequiresUpdate = true;
            }
            finally
            {
                Monitor.PulseAll(_lock);
                Monitor.Exit(_lock);
            }
        }

        ~RomManager()
        {
            _watcher.Dispose();
            _fileList.Clear();
            _lock = null;
            _fileMenu = string.Empty;
        }
    }
}