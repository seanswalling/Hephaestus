using Hephaestus.Core.Domain;
using Hephaestus.Core.FileSystem.Loading;

namespace Hephaestus.Core
{
    internal class ProjectCache : FileCache<Project>
    {
        private static ProjectCache? _instance;

        private ProjectCache() { }

        internal static ProjectCache Instance
        {
            get
            {
                _instance ??= new ProjectCache();
                return _instance;
            }
        }
    }
}
