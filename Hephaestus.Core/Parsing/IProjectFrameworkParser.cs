using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IProjectFrameworkParser
    {
        Framework Parse(XDocument project);
    }
}
