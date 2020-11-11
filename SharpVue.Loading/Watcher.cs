using System;
using System.IO;
using System.Threading;

namespace SharpVue.Loading
{
    public interface IWatcher
    {
        event Action<FileChangedEventArgs> FileChanged;
    }

    internal class Watcher : IWatcher
    {
        private readonly FileSystemWatcher FileWatcher;

        public event Action<FileChangedEventArgs> FileChanged = delegate { };

        public Watcher(Workspace workspace)
        {
            this.FileWatcher = new FileSystemWatcher(workspace.BaseFolder);
            this.FileWatcher.Changed += OnChanged;
            this.FileWatcher.Created += OnChanged;
            this.FileWatcher.Deleted += OnChanged;
            this.FileWatcher.Renamed += OnRenamed;
            this.FileWatcher.IncludeSubdirectories = true;
            this.FileWatcher.EnableRaisingEvents = true;
        }

        private void OnFileChanged(string path, bool deleted)
        {
            Thread.Sleep(100);
            FileChanged(new FileChangedEventArgs(path, deleted));
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            OnFileChanged(e.FullPath, false);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            OnFileChanged(e.FullPath, e.ChangeType == WatcherChangeTypes.Deleted);
        }
    }

    public sealed class FileChangedEventArgs : EventArgs
    {
        public string FilePath { get; }
        public string Extension { get; }
        public bool Deleted { get; }

        internal FileChangedEventArgs(string filePath, bool deleted)
        {
            this.FilePath = filePath;
            this.Extension = Path.GetExtension(filePath);
            this.Deleted = deleted;
        }
    }
}
