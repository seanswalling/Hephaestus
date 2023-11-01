using Hephaestus.Core.FileSystem.Loading;
using Hephaestus.Core.Parsing;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hephaestus.Core
{
    internal class CacheFileProviderAdapter : IFileProvider
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
