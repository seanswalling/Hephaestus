using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IProjectOutputTypeParser
    {
        OutputType Parse(XDocument project);
    }
}
