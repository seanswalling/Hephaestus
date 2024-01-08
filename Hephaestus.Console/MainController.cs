using Hephaestus.Core.Application;

namespace Hephaestus.Console
{
    public class MainController
    {
        private MainModel _model;

        public MainController(MainModel model)
        {
            _model = model;
        }

        public void SetRepository(KnownRepository kr)
        {
            _model.Clear();
            _model.LoadRepository(kr);
        }

        public void AddRepository(string name, string path)
        {
            _model.AddRepository(name, path);
        }

        public void Parse()
        {
            if (_model.CanParse)
                _model.Parse();
        }

        public void RebuildCache()
        {
            _model.RebuildCache();
        }
    }
}
