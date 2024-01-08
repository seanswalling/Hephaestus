using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IProjectMetadataParser
    {
        ProjectMetadata Parse(string projectPath, XDocument project);
    }
}
