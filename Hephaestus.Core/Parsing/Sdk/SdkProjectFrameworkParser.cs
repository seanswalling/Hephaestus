using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Sdk
{
    public class SdkProjectFrameworkParser : IProjectFrameworkParser
    {
        private readonly ITfmTranslator _translator;

        public SdkProjectFrameworkParser(ITfmTranslator translator)
        {
            _translator = translator;
        }

        public Framework Parse(XDocument project)
        {
            //TODO Not Ideal only takes the first in a collection.  Need a better way.
            if (project.Descendants("TargetFrameworks").Any())
            {
                return _translator.Translate(project.Descendants("TargetFrameworks").SingleOrDefault()?.Value.Split(",")
                    .First());
            }

            return _translator.Translate(project.Descendants("TargetFramework").SingleOrDefault()?.Value);
        }
    }
}
