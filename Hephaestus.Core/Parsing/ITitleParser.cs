using System.Xml.Linq;

namespace Hephaestus.Core.Parsing
{
    public interface ITitleParser
    {
        string? Parse(XDocument document);
    }
}
