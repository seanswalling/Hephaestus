using System;
using System.IO;
using System.Threading.Tasks;

namespace Hephaestus.Core.FileSystem.Watcher
{
    public class CodeFileSystemWatcher
    {
        private readonly FileSystemWatcher _watcher;


        public CodeFileSystemWatcher(string baseRepoPath)
        {
            _watcher = new FileSystemWatcher(baseRepoPath);
            _watcher.NotifyFilter = NotifyFilters.CreationTime
                                   | NotifyFilters.DirectoryName
                                   | NotifyFilters.FileName
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.Size;

            _watcher.Error += OnError;

            _watcher.Filters.Add("packages.config");
            _watcher.Filters.Add("*.csproj");
            _watcher.Filters.Add("*.sln");
            _watcher.IncludeSubdirectories = true;
        }

        public void StartWatcher()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void SubscribeChanged(Action<FileSystemEventArgs> onChanged)
        {
            _watcher.Changed += Delay(onChanged);
        }

        public void SubscribeCreated(Action<FileSystemEventArgs> onCreated)
        {
            _watcher.Created += Delay(onCreated);
        }

        public void SubscribeDeleted(Action<FileSystemEventArgs> onDeleted)
        {
            _watcher.Deleted += Delay(onDeleted);
        }

        public void SubscribeRenamed(Action<RenamedEventArgs> onRenamed)
        {
            _watcher.Renamed += Delay(onRenamed);
        }

        private static FileSystemEventHandler Delay(Action<FileSystemEventArgs> action)
        {
            return ((o, e) =>
            {
                Task.Delay(10).Wait();
                action(e);
            });
        }

        private static RenamedEventHandler Delay(Action<RenamedEventArgs> action)
        {
            return ((o, e) =>
            {
                Task.Delay(10).Wait();
                action(e);
            });
        }

        private static void OnError(object sender, ErrorEventArgs e) => Console.Write(e.GetException());
    }
}
