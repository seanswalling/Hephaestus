using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Legacy
{
    public class LegacyProjectFrameworkParser : LegacyFormat, IProjectFrameworkParser
    {
        private readonly ITfmTranslator _translator;

        public LegacyProjectFrameworkParser(ITfmTranslator translator)
        {
            _translator = translator;
        }

        public Framework Parse(XDocument project)
        {
            var value = project.Descendants(Namespace + "TargetFrameworkVersion").SingleOrDefault()?.Value;
            return _translator.Translate(value);
        }
    }
}
