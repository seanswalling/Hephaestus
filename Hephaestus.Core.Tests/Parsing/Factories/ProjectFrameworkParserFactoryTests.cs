using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Factories;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Factories
{
    public class ProjectFrameworkParserFactoryTests
    {
        [Theory]
        [InlineData(ProjectFormat.Sdk, typeof(SdkProjectFrameworkParser))]
        [InlineData(ProjectFormat.Framework, typeof(LegacyProjectFrameworkParser))]
        public void CanGetFrameworkParser(ProjectFormat format, Type parserType)
        {
            var parser = new ProjectFrameworkParserFactory(new TfmTranslator()).Create(format);
            Assert.IsType(parserType, parser);
        }

        [Fact]
        public void UnknownFormatThrows()
        {
            Assert.Throws<ArgumentException>(() => new ProjectFrameworkParserFactory(new TfmTranslator()).Create(ProjectFormat.Unknown));
        }
    }
}
