using System.Collections.Generic;
using System.Linq;
using Hephaestus.Core.Application;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public class BasicFileCollection : IFileCollection
    {
        private CacheManager _cacheManager;

        public BasicFileCollection(CacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void Update(CacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public string GetContent(string filePath)
        {
            return _cacheManager.Content.FileContent[filePath];
        }

        public IDictionary<string, string> GetFiles(Glob glob)
        {
            return _cacheManager.Content.FileContent
                .Where(x => glob.IncludesFile(x.Key))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public IDictionary<string, string> GetFiles(string projectPath)
        {
            var filePaths = _cacheManager.Ownership.FileOwnership[projectPath];
            return GetFiles(filePaths);
        }

        public IDictionary<string, string> GetFiles(IEnumerable<string> filePaths)
        {
            return filePaths
                .Where(fp => _cacheManager.Content.FileContent.ContainsKey(fp))
                .ToDictionary(fp => fp, fp => _cacheManager.Content.FileContent[fp]);
        }

        public bool Exists(string filePath)
        {
            return _cacheManager.Content.FileContent.ContainsKey(filePath);
        }
    }
}
