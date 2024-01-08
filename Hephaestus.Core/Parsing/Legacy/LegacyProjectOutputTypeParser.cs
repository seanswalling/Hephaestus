using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Legacy
{
    public class LegacyProjectOutputTypeParser : LegacyFormat, IProjectOutputTypeParser
    {
        private readonly IOutputTypeTranslator _outputTypeTranslator;

        public LegacyProjectOutputTypeParser(IOutputTypeTranslator outputTypeTranslator)
        {
            _outputTypeTranslator = outputTypeTranslator;
        }

        public OutputType Parse(XDocument project)
        {
            return _outputTypeTranslator.Translate(project.Descendants(Namespace + "OutputType").SingleOrDefault()?.Value);
        }
    }
}
