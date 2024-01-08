using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing
{
    public class OutputTypeTranslatorTests
    {
        [Fact]
        public void IfNullReturnLibrary()
        {
            var result = new OutputTypeTranslator().Translate(null);
            Assert.Equal(OutputType.Library, result);
        }

        [Fact]
        public void IfEmptyReturnLibrary()
        {
            var result = new OutputTypeTranslator().Translate(string.Empty);
            Assert.Equal(OutputType.Library, result);
        }

        [Fact]
        public void IfWhitespaceReturnLibrary()
        {
            var result = new OutputTypeTranslator().Translate(" ");
            Assert.Equal(OutputType.Library, result);
        }

        [Theory]
        [InlineData("library", OutputType.Library)]
        [InlineData("exe", OutputType.Exe)]
        [InlineData("module", OutputType.Module)]
        [InlineData("winexe", OutputType.Winexe)]
        public void CanTranslateOutput(string input, OutputType expected)
        {
            var result = new OutputTypeTranslator().Translate(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void UnknownOutputThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new OutputTypeTranslator().Translate(Guid.NewGuid().ToString()));
        }
    }
}
