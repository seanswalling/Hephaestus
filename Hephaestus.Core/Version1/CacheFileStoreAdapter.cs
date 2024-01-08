using Hephaestus.Core.Version1.FileSystem.Loading;

namespace Hephaestus.Core.Version1
{
    internal class CacheFileStoreAdapter : IFileStore
    {
        private readonly FileContentCache _cache;

        public CacheFileStoreAdapter(FileContentCache cache)
        {
            _cache = cache;
        }

        public void Save(string path, string content)
        {
            _cache.Set(path, content);
        }

        public void Rename(string oldPath, string newPath)
        {
            if (!_cache.HasFile(oldPath)) return;
            var oldContent = _cache.GetFile(oldPath);
            _cache.Remove(oldPath);
            _cache.Set(newPath, oldContent);
        }

        public void Remove(string path)
        {
            if (!_cache.HasFile(path)) return;
            _cache.Remove(path);
        }
    }
}