using System.Collections.Generic;
using System.Linq;
using Hephaestus.Core.Application;
using Hephaestus.Core.Domain;

namespace Hephaestus.Console
{
    public class MainModel
    {
        private readonly Application _app;
        public string Name => _app.Name;
        public string CacheState => _app.CacheManager.IsEmpty() ? "Empty" : "Set";
        public bool CanParse => CacheState != "Empty";
        public IEnumerable<KnownRepository> KnownRepositories => _app.KnownRepositories;
        public CodeRepository CodeRepository { get; private set; }
        public IEnumerable<string> Solutions => CodeRepository.Solutions.Select(x => x.Name);

        public MainModel(Application app)
        {
            _app = app;
        }

        internal void Clear()
        {
            _app.Clear();
        }

        internal void LoadRepository(KnownRepository kr)
        {
            _app.LoadRepository(kr);
        }

        internal void AddRepository(string name, string path)
        {
            _app.AddRepository(name, path);
        }

        internal void Parse()
        {
            CodeRepository = _app.Parse();
        }

        internal void RebuildCache()
        {
            _app.RebuildCache();
        }
    }
}
