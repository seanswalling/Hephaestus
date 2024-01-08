using System.Xml.Linq;

namespace Hephaestus.Core.Parsing
{
    public interface IRootNamespaceParser
    {
        string? Parse(XDocument document);
    }
}
