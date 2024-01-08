using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Version1.FileSystem.Loading;

namespace Hephaestus.Core.Version1
{
    public class CacheFileProviderAdapter : IFileProvider
    {
        private readonly FileContentCache _cache;
        public CacheFileProviderAdapter(FileContentCache cache)
        {
            _cache = cache;
        }

        public bool HasFile(string path)
        {
            return _cache.HasFile(path);
        }

        public string GetFile(string path)
        {
            return _cache.GetFile(path);
        }

        public IEnumerable<KeyValuePair<string, string>> QueryByExtension(string extension)
        {
            return _cache.Entries().Where(x => Path.GetExtension(x.Key) == extension);
        }
    }
}
