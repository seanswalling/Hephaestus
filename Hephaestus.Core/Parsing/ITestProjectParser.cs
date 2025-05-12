using System.Xml.Linq;

namespace Hephaestus.Core.Parsing
{
    public interface ITestProjectParser
    {
        bool Parse(XDocument project);
    }
}
