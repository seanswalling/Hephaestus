using System;
using Hephaestus.Core.Domain;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class ProjectTests
    {
        [Fact]
        public void NameCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Project(null, new ProjectMetadata("Foo\\Bah", Framework.Unknown, OutputType.Unknown, ProjectFormat.Unknown), [], [], new ReferenceManager()));
        }

        [Fact]
        public void MetadataCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Project("Foo.csproj", null, [], [], new ReferenceManager()));
        }

        [Fact]
        public void FilesCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Project("Foo.csproj", new ProjectMetadata("Foo\\Bah", Framework.Unknown, OutputType.Unknown, ProjectFormat.Unknown), null, [], new ReferenceManager()));
        }

        [Fact]
        public void ResourcesCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Project("Foo.csproj", new ProjectMetadata("Foo\\Bah", Framework.Unknown, OutputType.Unknown, ProjectFormat.Unknown), [], null, new ReferenceManager()));
        }

        [Fact]
        public void ReferencesCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Project("Foo.csproj", new ProjectMetadata("Foo\\Bah", Framework.Unknown, OutputType.Unknown, ProjectFormat.Unknown), [], [], null));
        }

        [Fact]
        public void CanCreateProject()
        {
            _ = new Project("Foo.csproj", new ProjectMetadata("Foo\\Bah", Framework.Unknown, OutputType.Unknown, ProjectFormat.Unknown), [], [], new ReferenceManager());
        }
    }
}
