using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Sdk;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Sdk
{
    public class SdkEmbeddedResourceParserTests : SdkFormatTestBase
    {
        [Fact]
        public void CanParseEmbeddedResource()
        {
            var ig = new XElement("ItemGroup");
            var er = new XElement("EmbeddedResource", new XAttribute("Include", "Foo/Bah"));
            ig.Add(er);
            SdkElement.Add(ig);
            var result = new SdkEmbeddedResourceParser().Parse(Project);
            Assert.Single(result);
            Assert.Equal(new EmbeddedResource("Foo/Bah"), result.Single());
        }

        [Fact]
        public void MissingIncludeAppearsAsUnknown()
        {
            var ig = new XElement("ItemGroup");
            var er = new XElement("EmbeddedResource");
            ig.Add(er);
            SdkElement.Add(ig);
            var result = new SdkEmbeddedResourceParser().Parse(Project);
            Assert.Single(result);
            Assert.Equal(new EmbeddedResource("Unknown Embedded Resource"), result.Single());
        }

        [Fact]
        public void LinkedPathsAreAddedsAsALink()
        {
            var ig = new XElement("ItemGroup");
            var er = new XElement("EmbeddedResource",
                new XAttribute("Include", "Foo/Bah"),
                new XAttribute("Link", "LinkedFoo"));
            ig.Add(er);
            SdkElement.Add(ig);
            var result = new SdkEmbeddedResourceParser().Parse(Project);
            Assert.Single(result);
            var resource = new EmbeddedResource("Foo/Bah");
            resource.Link("LinkedFoo");
            Assert.Equal(resource, result.Single());
        }
    }
}
