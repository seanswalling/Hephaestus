using System.Linq;
using System.Xml.Linq;

namespace Hephaestus.Core.Parsing.Sdk
{
    public class SdkRootNamespaceParser : IRootNamespaceParser
    {
        public string? Parse(XDocument document)
        {
            return document.Descendants("RootNamespace").SingleOrDefault()?.Value;
        }
    }
}
