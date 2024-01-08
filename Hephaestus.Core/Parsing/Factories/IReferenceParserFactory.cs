using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Factories
{
    public interface IReferenceParserFactory
    {
        IReferenceParser Create(ProjectFormat format, XDocument project, XDocument? packages);
    }
}
