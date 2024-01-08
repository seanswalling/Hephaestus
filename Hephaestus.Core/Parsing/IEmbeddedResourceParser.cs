using System.Collections.Generic;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IEmbeddedResourceParser
    {
        IEnumerable<EmbeddedResource> Parse(XDocument project);
    }
}
