using System;
using System.Linq;
using Hephaestus.Core.Domain;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class CSharpFileTests
    {
        [Fact]
        public void FilePathRequired()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CSharpFile(
                    null,
                    new CSharpNamespace("Foo.Bah"),
                    Array.Empty<CSharpUsing>()
                ));
        }

        [Fact]
        public void NamespaceRequired()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CSharpFile(
                    "Path/To/file",
                    null,
                Array.Empty<CSharpUsing>()
                ));
        }

        [Fact]
        public void UsingsNotRequiredRequired()
        {
            var file = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            Assert.NotNull(file);
        }

        [Fact]
        public void CSharpFileEquals()
        {
            var file1 = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            var file2 = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            Assert.True(file1.Equals(file2));
        }

        [Fact]
        public void CSharpFileNotEquals()
        {
            var file1 = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            var file2 = new CSharpFile(
                "Path/To/diff/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            var file3 = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah.Diff"),
                null
                );

            Assert.False(file1.Equals(file2));
            Assert.False(file1.Equals(file3));
            Assert.False(file2.Equals(file1));
            Assert.False(file2.Equals(file3));
            Assert.False(file3.Equals(file1));
            Assert.False(file3.Equals(file2));
        }

        [Fact]
        public void CSharpFileObjectEquals()
        {
            var file1 = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            var file2 = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            Assert.True(file1.Equals((object)file2));
        }

        [Fact]
        public void CSharpFileObjectNotEquals()
        {
            var file1 = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            var file2 = new CSharpFile(
                "Path/To/diff/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            var file3 = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah.Diff"),
                null
                );

            Assert.False(file1.Equals((object)file2));
            Assert.False(file1.Equals((object)file3));
            Assert.False(file2.Equals((object)file1));
            Assert.False(file2.Equals((object)file3));
            Assert.False(file3.Equals((object)file1));
            Assert.False(file3.Equals((object)file2));
        }

        [Fact]
        public void CSharpFileObjectNotEqualsWrongObject()
        {
            var file1 = new CSharpFile(
               "Path/To/file",
               new CSharpNamespace("Foo.Bah"),
               null
               );

            var foo = new object();

            Assert.False(file1.Equals(foo));
        }

        [Fact]
        public void CSharpFilesHaveSameHashCodes()
        {
            var file1 = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            var file2 = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            Assert.Equal(file1.GetHashCode(), file2.GetHashCode());
        }

        [Fact]
        public void CSharpFilesHaveDifferentHashCodes()
        {
            var file1 = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            var file2 = new CSharpFile(
                "Path/To/diff/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            var file3 = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah.Diff"),
                null
                );

            Assert.NotEqual(file1.GetHashCode(), file2.GetHashCode());
            Assert.NotEqual(file1.GetHashCode(), file3.GetHashCode());
            Assert.NotEqual(file2.GetHashCode(), file1.GetHashCode());
            Assert.NotEqual(file2.GetHashCode(), file3.GetHashCode());
            Assert.NotEqual(file3.GetHashCode(), file1.GetHashCode());
            Assert.NotEqual(file3.GetHashCode(), file2.GetHashCode());
        }

        [Fact]
        public void CanAddCsUsing()
        {
            var file = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            file.AddUsing(new CSharpUsing(new CSharpNamespace("Baz.Bah")));

            Assert.Single(file.UsingDirectives);
            Assert.Equal(new CSharpUsing(new CSharpNamespace("Baz.Bah")), file.UsingDirectives.Single());
        }

        [Fact]
        public void CanRemoveCsUsing()
        {
            var file = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            file.AddUsing(new CSharpUsing(new CSharpNamespace("Baz.Bah")));
            Assert.Single(file.UsingDirectives);

            file.RemoveUsing(new CSharpUsing(new CSharpNamespace("Baz.Bah")));
            Assert.Empty(file.UsingDirectives);
        }

        [Fact]
        public void CannotRemoveUsingWithIncorrectName()
        {
            var file = new CSharpFile(
                "Path/To/file",
                new CSharpNamespace("Foo.Bah"),
                null
                );

            file.AddUsing(new CSharpUsing(new CSharpNamespace("Baz.Bah")));
            Assert.Single(file.UsingDirectives);

            file.RemoveUsing(new CSharpUsing(new CSharpNamespace("Baz.Bah")));
            Assert.Empty(file.UsingDirectives);
        }
    }
}
