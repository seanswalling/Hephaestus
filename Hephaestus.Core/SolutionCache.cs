using Hephaestus.Core.Domain;
using Hephaestus.Core.FileSystem.Loading;

namespace Hephaestus.Core
{
    internal class SolutionCache : FileCache<Solution>
    {
        private static SolutionCache? _instance;

        private SolutionCache() { }

        internal static SolutionCache Instance
        {
            get
            {
                _instance ??= new SolutionCache();
                return _instance;
            }
        }
    }
}
