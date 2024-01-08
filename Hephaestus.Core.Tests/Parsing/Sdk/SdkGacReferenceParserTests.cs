using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Sdk;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Sdk
{
    public class SdkGacReferenceParserTests : SdkFormatTestBase
    {
        [Fact]
        public void CanParseGacReference()
        {
            SdkElement.Add(
                new XElement("ItemGroup",
                    new XElement("Reference",
                        new XAttribute("Include", "System.ServiceModel"))));

            var result = new SdkGacReferenceParser(Project).Parse();
            Assert.Equal(new GacReference("System.ServiceModel"), result.Single());
        }

        [Fact]
        public void CanParseMultipleGacReferences()
        {
            SdkElement.Add(
                new XElement("ItemGroup",
                    new XElement("Reference", new XAttribute("Include", "System.ServiceModel")),
                    new XElement("Reference", new XAttribute("Include", "System"))
                    ));

            var result = new SdkGacReferenceParser(Project).Parse();

            Assert.Equal(new[]
            {
                new GacReference("System.ServiceModel"),
                new GacReference("System")
            },
            result);
        }

        [Fact]
        public void ReturnsEmptyWhenNoGacReferences()
        {
            var result = new SdkGacReferenceParser(Project).Parse();
            Assert.Equal(Array.Empty<GacReference>(), result);
        }

        [Fact]
        public void IncludeIsRequired()
        {
            SdkElement.Add(
               new XElement("ItemGroup",
                   new XElement("Reference")));

            Assert.Throws<InvalidDataException>(() => new SdkGacReferenceParser(Project).Parse().ToArray());
        }
    }
}
