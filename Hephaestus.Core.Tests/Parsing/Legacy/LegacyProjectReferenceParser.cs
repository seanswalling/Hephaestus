using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Legacy
{
    public class LegacyProjectReferenceParserTests : LegacyFormatTestBase
    {
        [Fact]
        public void CanParseProjectReference()
        {
            ProjectRoot.Add(
                new XElement(Namespace + "ItemGroup",
                    new XElement(Namespace + "ProjectReference",
                        new XAttribute("Include", "Foo\\Bar"))));

            var result = new LegacyProjectReferenceParser(Project).Parse();
            Assert.Equal(new ProjectReference("Foo\\Bar"), result.Single());
        }

        [Fact]
        public void CanParseMultipleProjectReferences()
        {
            ProjectRoot.Add(
                new XElement(Namespace + "ItemGroup",
                    new XElement(Namespace + "ProjectReference", new XAttribute("Include", "Foo\\Bar")),
                    new XElement(Namespace + "ProjectReference", new XAttribute("Include", "Foo2\\Bar3"))
                    ));

            var result = new LegacyProjectReferenceParser(Project).Parse();

            Assert.Equal(new[]
            {
                new ProjectReference("Foo\\Bar"),
                new ProjectReference("Foo2\\Bar3")
            },
            result);
        }

        [Fact]
        public void ReturnsEmptyWhenNoProjectReferences()
        {
            var result = new LegacyProjectReferenceParser(Project).Parse();
            Assert.Equal(Array.Empty<ProjectReference>(), result);
        }

        [Fact]
        public void IncludeIsRequired()
        {
            ProjectRoot.Add(
               new XElement(Namespace + "ItemGroup",
                   new XElement(Namespace + "ProjectReference")));

            Assert.Throws<InvalidDataException>(() => new LegacyProjectReferenceParser(Project).Parse().ToArray());
        }
    }
}
