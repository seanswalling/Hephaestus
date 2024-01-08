using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Sdk
{
    public class SdkEmbeddedResourceParser : IEmbeddedResourceParser
    {
        public IEnumerable<EmbeddedResource> Parse(XDocument project)
        {
            return project.Descendants("EmbeddedResource")
                .Select(x =>
                {
                    var include = x.Attribute("Include");
                    if (include == null)
                        return new EmbeddedResource("Unknown Embedded Resource");
                    var relativePath = include.Value;
                    var linkedPath = x.Attribute("Link")?.Value;
                    var resource = new EmbeddedResource(relativePath);
                    if (linkedPath != null)
                        resource.Link(linkedPath);
                    return resource;
                });
        }
    }
}
