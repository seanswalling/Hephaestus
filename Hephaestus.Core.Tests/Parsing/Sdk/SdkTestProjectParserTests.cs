using System.Xml.Linq;
using Hephaestus.Core.Parsing.Sdk;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Sdk
{
    public class SdkTestProjectParserTests : SdkFormatTestBase
    {
        [Theory]
        [InlineData("xunit.v3", "1.0.0")]
        [InlineData("xunit.runner.visualstudio", "2.5.1")]
        [InlineData("Microsoft.NET.Test.Sdk", "17.4.1")]
        public void CanParseTestProjectReferenceX(string testProject, string version)
        {
            SdkElement.Add(
                new XElement("ItemGroup",
                    new XElement("PackageReference",
                        new XAttribute("Include", testProject),
                        new XAttribute("Version", version))));

            var result = new SdkTestProjectParser().Parse(Project);
            Assert.True(result);
        }

        [Fact]
        public void CanParseLackOfTestProjectReference()
        {
            var result = new SdkTestProjectParser().Parse(Project);
            Assert.False(result);
        }
    }
}
