using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Legacy
{
    public class LegacyEmbeddedResourceParser : LegacyFormat, IEmbeddedResourceParser
    {
        public IEnumerable<EmbeddedResource> Parse(XDocument project)
        {
            return project.Descendants(Namespace + "EmbeddedResource")
                .Select(x =>
                {
                    var relativePath = x.Attribute("Include")!.Value;
                    var linkedPath = x.Descendants(Namespace + "Link").SingleOrDefault()?.Value;

                    var resource = new EmbeddedResource(relativePath);

                    if (linkedPath != null)
                    {
                        resource.Link(linkedPath);
                    }

                    return resource;
                });
        }
    }
}
