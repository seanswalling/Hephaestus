using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IProjectFormatParser
    {
        ProjectFormat Parse(XDocument project);
    }
}
