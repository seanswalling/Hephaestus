using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IProjectParser
    {
        Project Parse(string filePath, XDocument document);
    }
}