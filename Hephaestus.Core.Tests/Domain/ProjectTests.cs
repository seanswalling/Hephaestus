using System;
using Hephaestus.Core.Domain;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class ProjectTests
    {
        private readonly ProjectMetadata _metadata;

        public ProjectTests()
        {
            _metadata = new ProjectMetadata("Foo\\Bah", Framework.Unknown, OutputType.Unknown, ProjectFormat.Unknown, "Foo", "Foo", "Foo", new Warnings(null, null, []), false);
        }

        [Fact]
        public void NameCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Project(null, _metadata, [], [], new ReferenceManager()));
        }

        [Fact]
        public void MetadataCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Project("Foo.csproj", null, [], [], new ReferenceManager()));
        }

        [Fact]
        public void FilesCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Project("Foo.csproj", _metadata, null, [], new ReferenceManager()));
        }

        [Fact]
        public void ResourcesCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Project("Foo.csproj", _metadata, [], null, new ReferenceManager()));
        }

        [Fact]
        public void ReferencesCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Project("Foo.csproj", _metadata, [], [], null));
        }

        [Fact]
        public void CanCreateProject()
        {
            _ = new Project("Foo.csproj", _metadata, [], [], new ReferenceManager());
        }
    }
}
