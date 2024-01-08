using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Sdk;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Sdk
{
    public class SdkProjectFrameworkParserTests : SdkFormatTestBase
    {
        [Fact]
        public void CanParseTargetFramework()
        {
            SdkElement.Add(
                new XElement("PropertyGroup",
                    new XElement("TargetFramework", "net48")
                ));
            var result = new SdkProjectFrameworkParser(new TfmTranslator()).Parse(Project);
            Assert.Equal(Framework.net48, result);
        }

        [Fact]
        public void CanParseTargetFrameworks()
        {
            SdkElement.Add(
                new XElement("PropertyGroup",
                    new XElement("TargetFrameworks", "net48")
                ));
            var result = new SdkProjectFrameworkParser(new TfmTranslator()).Parse(Project);
            Assert.Equal(Framework.net48, result);
        }

        [Fact]
        public void TakesFirstOfTargetFrameworks()
        {
            SdkElement.Add(
                new XElement("PropertyGroup",
                    new XElement("TargetFrameworks", "net8.0;net48")
                ));
            var result = new SdkProjectFrameworkParser(new TfmTranslator()).Parse(Project);
            Assert.Equal(Framework.net80, result);
        }
    }
}
