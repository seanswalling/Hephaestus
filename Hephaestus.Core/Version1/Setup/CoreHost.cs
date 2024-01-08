using System.IO;
using Hephaestus.Core.Version1.Building;
using Hephaestus.Core.Version1.FileSystem.Loading;
using Hephaestus.Core.Version1.FileSystem.Watcher;

namespace Hephaestus.Core.Version1.Setup
{
    public class CoreHost
    {
        private readonly string _repoPath;
        private readonly string _repoName;
        private readonly CodeFileSystemWatcher _watcher;
        private readonly FileContentCache _contentCache;
        private readonly CacheFileProviderAdapter _fileProviderAdapter;
        private readonly CacheFileStoreAdapter _fileStoreAdapter;
        private readonly FileSystemLoaderV1 _fileLoader;
        private readonly DomainV1Builder _domainBuilder;
        private readonly IRepositoryV1Store _repositoryStore;

        public CoreHost(string repoPath, string repoName, IRepositoryV1Store repositoryStore)
        {
            _repoPath = repoPath;
            _repoName = repoName;
            _repositoryStore = repositoryStore;
            _watcher = new CodeFileSystemWatcher(_repoPath);
            _contentCache = new FileContentCache();
            _fileProviderAdapter = new CacheFileProviderAdapter(_contentCache);
            _fileStoreAdapter = new CacheFileStoreAdapter(_contentCache);
            _fileLoader = new FileSystemLoaderV1(_fileStoreAdapter);
            _domainBuilder = new DomainV1Builder(_fileProviderAdapter);
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
