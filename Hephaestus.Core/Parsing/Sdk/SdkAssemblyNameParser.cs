using System.Linq;
using System.Xml.Linq;

namespace Hephaestus.Core.Parsing.Sdk
{
    public class SdkAssemblyNameParser : IAssemblyNameParser
    {
        public string? Parse(XDocument document)
        {
            return document.Descendants("AssemblyName").SingleOrDefault()?.Value;
        }
    }
}
