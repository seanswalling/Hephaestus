using System.Linq;
using System.Xml.Linq;

namespace Hephaestus.Core.Parsing.Legacy
{
    public class LegacyRootNamespaceParser : LegacyFormat, IRootNamespaceParser
    {
        public string? Parse(XDocument document)
        {
            return document.Descendants(Namespace + "RootNamespace").SingleOrDefault()?.Value;
        }
    }
}
