using Hephaestus.Core.Version1.Domain;
using Hephaestus.Core.Version1.FileSystem.Loading;

namespace Hephaestus.Core.Version1
{
    internal class ProjectCacheV1 : FileCache<ProjectV1>
    {
        private static ProjectCacheV1? _instance;

        private ProjectCacheV1() { }

        internal static ProjectCacheV1 Instance
        {
            get
            {
                _instance ??= new ProjectCacheV1();
                return _instance;
            }
        }
    }
}
