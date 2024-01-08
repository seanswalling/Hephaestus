using System.Collections.Generic;

namespace Hephaestus.Core.Application
{
    public class CacheManager
    {
        public ContentCache Content { get; private set; }
        public OwnershipCache Ownership { get; private set; }

        private CacheManager(ContentCache content, OwnershipCache ownership)
        {
            Content = content;
            Ownership = ownership;
        }

        public void Save()
        {
            Content.Save();
            Ownership.Save();
        }

        public bool IsEmpty()
        {
            return Content.IsEmpty() || Ownership.IsEmpty();
        }

        public static bool Exists(string name)
        {
            return OwnershipCache.Exists(name) && ContentCache.Exists(name);
        }

        public static CacheManager Load(string name)
        {
            return new CacheManager(ContentCache.Load(name), OwnershipCache.Load(name));
        }

        public static CacheManager Empty()
        {
            return new CacheManager(ContentCache.Empty(), OwnershipCache.Empty());
        }

        public static CacheManager Build(string name, IDictionary<string, string> fileCollection)
        {
            var ownership = OwnershipCache.Build(name, fileCollection.Keys);
            var content = ContentCache.Build(name, fileCollection);

            return new CacheManager(content, ownership);
        }
    }
}
