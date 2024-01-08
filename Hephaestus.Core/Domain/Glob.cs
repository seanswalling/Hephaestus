using System;
using System.IO;

namespace Hephaestus.Core.Domain
{
    public class Glob
    {
        public string FileExtension { get; }
        public Uri RootPath { get; }

        public Glob(string fileExtension, string rootPath)
        {
            FileExtension = fileExtension;
            if (Path.HasExtension(rootPath))
                throw new ArgumentException("Specify a directory, not a file");
            RootPath = EnsureSeparator(new Uri(rootPath));
        }

        public bool IncludesFile(string path)
        {
            return HasCorrectExtension(path) && IsBaseOf(path);
        }

        public bool HasCorrectExtension(string path)
        {
            return Path.GetExtension(path) == FileExtension;
        }

        public bool IsBaseOf(string path)
        {
            return RootPath.IsBaseOf(new Uri(path));
        }

        private static Uri EnsureSeparator(Uri path)
        {
            return Path.GetFullPath(path.AbsolutePath)
                .EndsWith(Path.DirectorySeparatorChar.ToString()) ?
                path :
                new Uri(path.AbsolutePath + Path.DirectorySeparatorChar);
        }

        public bool Equals(Glob? other)
        {
            throw new NotImplementedException();
        }
    }
}
