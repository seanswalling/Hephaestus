using System;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Factories;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Factories
{
    public class ReferenceParserFactoryTests
    {
        [Theory]
        [InlineData(ProjectFormat.Framework)]
        [InlineData(ProjectFormat.Sdk)]
        public void CanGetReferenceParser(ProjectFormat format)
        {
            var parser = new ReferenceParserFactory().Create(format, new XDocument(), new XDocument());
            Assert.IsType<ReferenceParser>(parser);
        }

        [Fact]
        public void UnknownFormatThrows()
        {
            Assert.Throws<ArgumentException>(() => new ReferenceParserFactory().Create(ProjectFormat.Unknown, new XDocument(), new XDocument()));
        }
    }
}
