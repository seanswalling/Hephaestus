using System.Linq;
using System.Xml.Linq;

namespace Hephaestus.Core.Parsing.Legacy
{
    public class LegacyTitleParser : LegacyFormat, ITitleParser
    {
        public string? Parse(XDocument document)
        {
            return document.Descendants(Namespace + "Title").SingleOrDefault()?.Value;
        }
    }
}
