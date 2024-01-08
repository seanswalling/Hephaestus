using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Factories;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Factories
{
    public class EmbeddedResourceParserFactoryTests
    {
        [Theory]
        [InlineData(ProjectFormat.Sdk, typeof(SdkEmbeddedResourceParser))]
        [InlineData(ProjectFormat.Framework, typeof(LegacyEmbeddedResourceParser))]
        public void CanGetResourceParser(ProjectFormat format, Type parserType)
        {
            var parser = new EmbeddedResourceParserFactory().Create(format);
            Assert.IsType(parserType, parser);
        }

        [Fact]
        public void UnknownFormatThrows()
        {
            Assert.Throws<ArgumentException>(() => new EmbeddedResourceParserFactory().Create(ProjectFormat.Unknown));
        }
    }
}
