using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Legacy
{
    public class LegacyEmbeddedResourceParserTests : LegacyFormatTestBase
    {
        [Fact]
        public void CanParseEmbeddedResource()
        {
            var ig = new XElement(Namespace + "ItemGroup");
            var er = new XElement(Namespace + "EmbeddedResource", new XAttribute("Include", "Foo/Bah"));
            ig.Add(er);
            ProjectRoot.Add(ig);
            var result = new LegacyEmbeddedResourceParser().Parse(Project);
            Assert.Single(result);
            Assert.Equal(new EmbeddedResource("Foo/Bah"), result.Single());
        }

        [Fact]
        public void LinkedPathsAreAddedsAsALink()
        {
            var ig = new XElement(Namespace + "ItemGroup");
            var er = new XElement(Namespace + "EmbeddedResource",
                new XAttribute("Include", "Foo/Bah"),
                new XElement(Namespace + "Link", "LinkedFoo"));
            ig.Add(er);
            ProjectRoot.Add(ig);
            var result = new LegacyEmbeddedResourceParser().Parse(Project);
            Assert.Single(result);
            var resource = new EmbeddedResource("Foo/Bah");
            resource.Link("LinkedFoo");
            Assert.Equal(resource, result.Single());
        }

        [Fact]
        public void IfNoResourcesThenReturnEmptyCollection()
        {
            var result = new LegacyEmbeddedResourceParser().Parse(Project);
            Assert.Empty(result);
        }
    }
}
