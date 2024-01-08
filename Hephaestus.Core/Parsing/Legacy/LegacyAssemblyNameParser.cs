using System.Linq;
using System.Xml.Linq;

namespace Hephaestus.Core.Parsing.Legacy
{
    public class LegacyAssemblyNameParser : LegacyFormat, IAssemblyNameParser
    {
        public string? Parse(XDocument document)
        {
            return document.Descendants(Namespace + "AssemblyName").SingleOrDefault()?.Value;
        }
    }
}
