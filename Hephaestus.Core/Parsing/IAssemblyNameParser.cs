using System.Xml.Linq;

namespace Hephaestus.Core.Parsing
{
    public interface IAssemblyNameParser
    {
        string? Parse(XDocument document);
    }
}
