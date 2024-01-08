using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Legacy
{
    public class LegacyWarningParser : IWarningsParser
    {
        public Warnings Parse(XDocument document)
        {
            return new Warnings(null, null, []);
        }
    }
}
