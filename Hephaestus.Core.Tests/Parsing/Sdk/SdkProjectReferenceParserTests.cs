using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Sdk;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Sdk
{
    public class SdkProjectReferenceParserTests : SdkFormatTestBase
    {
        [Fact]
        public void CanParseProjectReference()
        {
            SdkElement.Add(
                new XElement("ItemGroup",
                    new XElement("ProjectReference",
                        new XAttribute("Include", "Foo\\Bar"))));

            var result = new SdkProjectReferenceParser(Project).Parse();
            Assert.Equal(new ProjectReference("Foo\\Bar"), result.Single());
        }

        [Fact]
        public void CanParseMultipleProjectReferences()
        {
            SdkElement.Add(
                new XElement("ItemGroup",
                    new XElement("ProjectReference", new XAttribute("Include", "Foo\\Bar")),
                    new XElement("ProjectReference", new XAttribute("Include", "Foo2\\Bar3"))
                    ));

            var result = new SdkProjectReferenceParser(Project).Parse();

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
            var result = new SdkProjectReferenceParser(Project).Parse();
            Assert.Equal(Array.Empty<ProjectReference>(), result);
        }

        [Fact]
        public void IncludeIsRequired()
        {
            SdkElement.Add(
               new XElement("ItemGroup",
                   new XElement("ProjectReference")));

            Assert.Throws<InvalidDataException>(() => new SdkProjectReferenceParser(Project).Parse().ToArray());
        }
    }
}
