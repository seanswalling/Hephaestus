using System.IO;
using Hephaestus.Core.Building;
using Hephaestus.Core.FileSystem.Loading;
using Hephaestus.Core.FileSystem.Watcher;

namespace Hephaestus.Core.Setup
{
    public class CoreHost
    {
        private readonly string _repoPath;
        private readonly string _repoName;
        private readonly CodeFileSystemWatcher _watcher;
        private readonly FileContentCache _contentCache;
        private readonly CacheFileProviderAdapter _fileProviderAdapter;
        private readonly CacheFileStoreAdapter _fileStoreAdapter;
        private readonly FileSystemLoader _fileLoader;
        private readonly DomainBuilder _domainBuilder;
        private readonly IRepositoryStore _repositoryStore;

        public CoreHost(string repoPath, string repoName, IRepositoryStore repositoryStore)
        {
            _repoPath = repoPath;
            _repoName = repoName;
            _repositoryStore = repositoryStore;
            _watcher = new CodeFileSystemWatcher(_repoPath);
            _contentCache = new FileContentCache();
            _fileProviderAdapter = new CacheFileProviderAdapter(_contentCache);
            _fileStoreAdapter = new CacheFileStoreAdapter(_contentCache);
            _fileLoader = new FileSystemLoader(_fileStoreAdapter);
            _domainBuilder = new DomainBuilder(_fileProviderAdapter);
        }

        public void Init()
        {
            _fileLoader.LoadAllFiles(_repoPath);
            var model = _domainBuilder.BuildCodeRepository(_repoPath, _repoName);
            _repositoryStore.Store(model);
        }

        private void Update(FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Deleted)
            {
                _fileLoader.LoadFile(e.FullPath);
            }
            var model = _domainBuilder.BuildCodeRepository(_repoPath, _repoName);
            _repositoryStore.Store(model);
        }

        private void Rename(RenamedEventArgs e)
        {
            _fileLoader.LoadFile(e.FullPath);
            var model = _domainBuilder.BuildCodeRepository(_repoPath, _repoName);
            _repositoryStore.Store(model);
        }
    }
}
