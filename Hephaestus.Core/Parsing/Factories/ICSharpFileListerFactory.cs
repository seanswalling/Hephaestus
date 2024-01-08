using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Factories
{
    public interface ICSharpFileListerFactory
    {
        ICSharpFileLister Create(ProjectMetadata metadata, XDocument projectContent);
    }
}
