using System;
using System.IO;

namespace Hephaestus.Core.Domain
{
    public class Glob
    {
        internal string FileExtension;
        internal string RootPath;

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
            return new Uri(EnsureSeparator(RootPath)).IsBaseOf(new Uri(path));
        }

        private static string EnsureSeparator(string path)
        {
            return Path.GetFullPath(path).EndsWith(Path.DirectorySeparatorChar.ToString()) ? path : path + Path.DirectorySeparatorChar;
        }
    }


}
