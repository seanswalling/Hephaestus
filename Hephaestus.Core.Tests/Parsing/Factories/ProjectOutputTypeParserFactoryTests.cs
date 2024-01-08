using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Factories;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Factories
{
    public class ProjectOutputTypeParserFactoryTests
    {
        [Theory]
        [InlineData(ProjectFormat.Sdk, typeof(SdkProjectOutputTypeParser))]
        [InlineData(ProjectFormat.Framework, typeof(LegacyProjectOutputTypeParser))]
        public void CanCreateOutputTypeParser(ProjectFormat format, Type parserType)
        {
            var parser = new ProjectOutputTypeParserFactory(new OutputTypeTranslator()).Create(format);
            Assert.IsType(parserType, parser);
        }

        [Fact]
        public void UnknownFormatThrows()
        {
            Assert.Throws<ArgumentException>(() => new ProjectOutputTypeParserFactory(new OutputTypeTranslator()).Create(ProjectFormat.Unknown));
        }
    }
}
