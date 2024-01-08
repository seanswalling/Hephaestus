using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Legacy;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Legacy
{
    public class LegacyProjectFrameworkParserTests : LegacyFormatTestBase
    {
        [Fact]
        public void CanParseTargetFramework()
        {
            ProjectRoot.Add(
                new XElement(Namespace + "PropertyGroup",
                    new XElement(Namespace + "TargetFrameworkVersion", "v4.8")
                ));
            var result = new LegacyProjectFrameworkParser(new TfmTranslator()).Parse(Project);
            Assert.Equal(Framework.net48, result);
        }

        [Fact]
        public void CanParseTargetFrameworks()
        {
            ProjectRoot.Add(
                new XElement(Namespace + "PropertyGroup",
                    new XElement(Namespace + "TargetFrameworkVersion", "v4.8")
                ));
            var result = new LegacyProjectFrameworkParser(new TfmTranslator()).Parse(Project);
            Assert.Equal(Framework.net48, result);
        }
    }
}
