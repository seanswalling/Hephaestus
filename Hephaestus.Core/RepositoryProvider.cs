using Hephaestus.Core.Domain;

namespace Hephaestus.Core
{
    public class RepositoryProvider
    {
        private CodeRepository? _currentModel;
        public bool HasUpdates { get; private set; }

        public RepositoryProvider()
        {
            _currentModel = null;
            HasUpdates = false;
        }

        public void Update(CodeRepository repo)
        {
            _currentModel = repo;
            HasUpdates = true;
        }

        public CodeRepository FetchUpdates()
        {
            HasUpdates = false;
            return _currentModel!;
        }
    }
}
