using System;
using System.Xml.Linq;
using Hephaestus.Core.Application;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Factories;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Factories
{
    public class CSharpListerFactoryTests
    {
        [Theory]
        [InlineData(ProjectFormat.Sdk, typeof(SdkCSharpFileLister))]
        [InlineData(ProjectFormat.Framework, typeof(LegacyCSharpFileLister))]
        public void CanCreateParser(ProjectFormat format, Type parserType)
        {
            var meta = new ProjectMetadata("Foo\\Bar", Framework.net80, OutputType.Library, format, "Foo", "Foo", "Foo", new Warnings(null, null, []));
            var parser = new CSharpFileListerFactory(new BasicFileCollection(CacheManager.Empty())).Create(meta, new XDocument());
            Assert.IsType(parserType, parser);
        }

        [Fact]
        public void UnknownFormatThrows()
        {
            var meta = new ProjectMetadata("Foo\\Bar", Framework.net80, OutputType.Library, ProjectFormat.Unknown, "Foo", "Foo", "Foo", new Warnings(null, null, []));
            Assert.Throws<ArgumentException>(() => new CSharpFileListerFactory(new BasicFileCollection(CacheManager.Empty())).Create(meta, new XDocument()));
        }
    }
}
