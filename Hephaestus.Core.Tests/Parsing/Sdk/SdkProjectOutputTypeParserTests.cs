using System;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Sdk;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Sdk
{
    public class SdkProjectOutputTypeParserTests : SdkFormatTestBase
    {
        [Fact]
        public void CanParseOutputType()
        {
            SdkElement.Add(
                new XElement("PropertyGroup",
                    new XElement("OuputType", "library")
                ));

            var result = new SdkProjectOutputTypeParser(new OutputTypeTranslator()).Parse(Project);

            Assert.Equal(OutputType.Library, result);
        }

        [Fact]
        public void ThrowsOnMultipleOutputTypeElements()
        {
            SdkElement.Add(
                new XElement("PropertyGroup",
                    new XElement("OutputType", "library"),
                    new XElement("OutputType", "library")
                ));

            Assert.Throws<InvalidOperationException>(() => new SdkProjectOutputTypeParser(new OutputTypeTranslator()).Parse(Project));
        }

        [Fact]
        public void ReturnsLibraryOnMissingElement()
        {
            SdkElement.Add(new XElement("PropertyGroup"));

            var result = new SdkProjectOutputTypeParser(new OutputTypeTranslator()).Parse(Project);

            Assert.Equal(OutputType.Library, result);
        }
    }
}
