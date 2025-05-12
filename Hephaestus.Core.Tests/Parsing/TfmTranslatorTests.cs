using System;
using System.Collections.Generic;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing
{
    public class TfmTranslatorTests
    {
        [Fact]
        public void ThrowsIfUnsupportedTfm()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TfmTranslator().Translate("NotATfm"));
        }

        [Fact]
        public void ThrowsOnEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new TfmTranslator().Translate(string.Empty));
        }

        [Fact]
        public void ThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => new TfmTranslator().Translate(null));
        }

        [Fact]
        public void ThrowsOnWhitespace()
        {
            Assert.Throws<ArgumentNullException>(() => new TfmTranslator().Translate(" "));
        }

        [Theory]
        [MemberData(nameof(TranslatorSupportedTfms))]
        public void TranslatesSupportedTfms(string moniker, Framework framework)
        {
            var result = new TfmTranslator().Translate(moniker);
            Assert.Equal(framework, result);
        }

        public static IEnumerable<object[]> TranslatorSupportedTfms()
        {
            yield return new object[] { "netcoreapp1.0", Framework.netcoreapp10 };
            yield return new object[] { "netcoreapp1.1", Framework.netcoreapp11 };
            yield return new object[] { "netcoreapp2.0", Framework.netcoreapp20 };
            yield return new object[] { "netcoreapp2.1", Framework.netcoreapp21 };
            yield return new object[] { "netcoreapp2.2", Framework.netcoreapp22 };
            yield return new object[] { "netcoreapp3.0", Framework.netcoreapp30 };
            yield return new object[] { "netcoreapp3.1", Framework.netcoreapp31 };
            yield return new object[] { "net5.0", Framework.net50 };
            yield return new object[] { "net6.0", Framework.net60 };
            yield return new object[] { "net7.0", Framework.net70 };
            yield return new object[] { "net8.0", Framework.net80 };
            yield return new object[] { "net9.0", Framework.net90 };
            yield return new object[] { "netstandard1.0", Framework.netstandard10 };
            yield return new object[] { "netstandard1.1", Framework.netstandard11 };
            yield return new object[] { "netstandard1.2", Framework.netstandard12 };
            yield return new object[] { "netstandard1.3", Framework.netstandard13 };
            yield return new object[] { "netstandard1.4", Framework.netstandard14 };
            yield return new object[] { "netstandard1.5", Framework.netstandard15 };
            yield return new object[] { "netstandard1.6", Framework.netstandard16 };
            yield return new object[] { "netstandard2.0", Framework.netstandard20 };
            yield return new object[] { "netstandard2.1", Framework.netstandard21 };
            yield return new object[] { "net11", Framework.net11 };
            yield return new object[] { "net20", Framework.net20 };
            yield return new object[] { "net35", Framework.net35 };
            yield return new object[] { "net40", Framework.net40 };
            yield return new object[] { "net403", Framework.net403 };
            yield return new object[] { "net45", Framework.net45 };
            yield return new object[] { "net451", Framework.net451 };
            yield return new object[] { "net452", Framework.net452 };
            yield return new object[] { "net46", Framework.net46 };
            yield return new object[] { "net461", Framework.net461 };
            yield return new object[] { "net462", Framework.net462 };
            yield return new object[] { "net47", Framework.net47 };
            yield return new object[] { "net471", Framework.net471 };
            yield return new object[] { "net472", Framework.net472 };
            yield return new object[] { "net48", Framework.net48 };
            yield return new object[] { "v4.8", Framework.net48 };
        }
    }
}
