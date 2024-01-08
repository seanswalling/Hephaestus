using Hephaestus.Core.Version1.Domain;
using Hephaestus.Core.Version1.FileSystem.Loading;

namespace Hephaestus.Core.Version1
{
    internal class SolutionCacheV1 : FileCache<SolutionV1>
    {
        private static SolutionCacheV1? _instance;

        private SolutionCacheV1() { }

        internal static SolutionCacheV1 Instance
        {
            get
            {
                _instance ??= new SolutionCacheV1();
                return _instance;
            }
        }
    }
}
