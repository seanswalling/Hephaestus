using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing
{
    public class ProjectFormatParserTests
    {
        [Fact]
        public void CanParseSdkFormat()
        {
            var content = new XDocument();
            content.Add(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk")));
            var format = new ProjectFormatParser().Parse(content);

            Assert.Equal(ProjectFormat.Sdk, format);
        }

        [Fact]
        public void AllOtherXDocumentsAreAssumedLegacy()
        {
            var format = new ProjectFormatParser().Parse(new XDocument());
            Assert.Equal(ProjectFormat.Framework, format);
        }
    }
}
