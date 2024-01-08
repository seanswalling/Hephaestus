using System.Linq;
using System.Xml.Linq;

namespace Hephaestus.Core.Parsing.Sdk
{
    public class SdkTitleParser : ITitleParser
    {
        public string? Parse(XDocument document)
        {
            return document.Descendants("Title").SingleOrDefault()?.Value;
        }
    }
}
