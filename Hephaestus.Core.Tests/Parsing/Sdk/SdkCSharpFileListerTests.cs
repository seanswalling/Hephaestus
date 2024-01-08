using System.Collections.Generic;
using Hephaestus.Core.Application;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Sdk;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Sdk
{
    public class SdkCSharpFileListerTests
    {
        private BasicFileCollection _collection;

        public SdkCSharpFileListerTests()
        {
            _collection = new BasicFileCollection(CacheManager.Build("Test", Files()));
        }

        [Fact]
        public void CanListFiles()
        {
            var meta = new ProjectMetadata(
                "C:\\Foo\\TheBaring.csproj",
                Framework.net80,
                OutputType.Library,
                ProjectFormat.Sdk
                );

            var files = new SdkCSharpFileLister(_collection, meta).ListFiles();

            Assert.Equal(2, files.Count);
            Assert.True(files.ContainsKey("C:\\Foo\\Bar.cs"));
            Assert.Equal("The Foo Bar Test File!", files["C:\\Foo\\Bar.cs"]);
            Assert.True(files.ContainsKey("C:\\Foo\\Bang.cs"));
            Assert.Equal("The Wiz Bang Test File!", files["C:\\Foo\\Bang.cs"]);
        }

        [Fact]
        public void DoNotListUnGlobbedFiles()
        {
            var meta = new ProjectMetadata(
                "C:\\Foo\\TheBaring.csproj",
                Framework.net80,
                OutputType.Library,
                ProjectFormat.Sdk
                );

            var files = new SdkCSharpFileLister(_collection, meta).ListFiles();

            Assert.Equal(2, files.Count);
            Assert.False(files.ContainsKey("C:\\Wiz\\Bang.cs"));
        }

        public Dictionary<string, string> Files()
        {
            return new Dictionary<string, string>
            {
                { "C:\\Foo\\Bar.cs", "The Foo Bar Test File!" },
                { "C:\\Foo\\Bang.cs", "The Wiz Bang Test File!" },
                { "C:\\Wiz\\Bang.cs", "Ignored Content" },
                { "C:\\Foo\\NotAsCs.json", "Rnadom json Content" },
                { "C:\\Foo\\TheBaring.csproj", "Some Content"},
            };
        }

    }
}
