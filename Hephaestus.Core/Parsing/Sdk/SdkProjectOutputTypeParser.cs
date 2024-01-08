using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Sdk
{
    public class SdkProjectOutputTypeParser : IProjectOutputTypeParser
    {
        private readonly IOutputTypeTranslator _outputTypeTranslator;

        public SdkProjectOutputTypeParser(IOutputTypeTranslator outputTypeTranslator)
        {
            _outputTypeTranslator = outputTypeTranslator;
        }

        public OutputType Parse(XDocument project)
        {
            return _outputTypeTranslator.Translate(project.Descendants("OutputType").SingleOrDefault()?.Value);
        }
    }
}
