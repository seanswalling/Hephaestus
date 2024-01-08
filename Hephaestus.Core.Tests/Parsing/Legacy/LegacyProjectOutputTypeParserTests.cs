using System;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Legacy;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Legacy
{
    public class LegacyProjectOutputTypeParserTests : LegacyFormatTestBase
    {
        [Fact]
        public void CanParseOutputType()
        {
            ProjectRoot.Add(
                new XElement(Namespace + "PropertyGroup",
                    new XElement(Namespace + "OuputType", "library")
                ));

            var result = new LegacyProjectOutputTypeParser(new OutputTypeTranslator()).Parse(Project);

            Assert.Equal(OutputType.Library, result);
        }

        [Fact]
        public void ThrowsOnMultipleOutputTypeElements()
        {
            ProjectRoot.Add(
                new XElement(Namespace + "PropertyGroup",
                    new XElement(Namespace + "OutputType", "library"),
                    new XElement(Namespace + "OutputType", "library")
                ));

            Assert.Throws<InvalidOperationException>(() => new LegacyProjectOutputTypeParser(new OutputTypeTranslator()).Parse(Project));
        }

        [Fact]
        public void ReturnsLibraryOnMissingElement()
        {
            ProjectRoot.Add(new XElement(Namespace + "PropertyGroup"));

            var result = new LegacyProjectOutputTypeParser(new OutputTypeTranslator()).Parse(Project);

            Assert.Equal(OutputType.Library, result);
        }
    }
}
