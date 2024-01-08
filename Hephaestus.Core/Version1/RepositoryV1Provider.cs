using Hephaestus.Core.Version1.Domain;

namespace Hephaestus.Core.Version1
{
    public class RepositoryV1Provider
    {
        private CodeRepositoryV1? _currentModel;
        public bool HasUpdates { get; private set; }

        public RepositoryV1Provider()
        {
            _currentModel = null;
            HasUpdates = false;
        }

        public void Update(CodeRepositoryV1 repo)
        {
            _currentModel = repo;
            HasUpdates = true;
        }

        public CodeRepositoryV1 FetchUpdates()
        {
            HasUpdates = false;
            return _currentModel!;
        }
    }
}
