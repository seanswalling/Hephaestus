using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hephaestus.Core.Application
{
    public class FileSystemLoader
    {
        public Dictionary<string, string> LoadAllFiles(string root)
        {
            return EnumerateFiles(root, "*.cs")
                .Concat(EnumerateFiles(root, "*.csproj"))
                .Concat(EnumerateFiles(root, "*.sln"))
                .Concat(EnumerateFiles(root, "packages.config"))
                .OrderBy(x => x)
                .ToDictionary(x => x, File.ReadAllText);
        }

        public IEnumerable<string> EnumerateFiles(string root, string pattern)
        {
            return Directory.EnumerateFiles(root, pattern, SearchOption.AllDirectories)
                .Where(x =>
                !x.Contains(@"\bin\", StringComparison.OrdinalIgnoreCase) &&
                !x.Contains(@"\obj\", StringComparison.OrdinalIgnoreCase) &&
                !x.Contains(@"\.git\", StringComparison.OrdinalIgnoreCase) &&
                !x.Contains(@"\Templates\", StringComparison.OrdinalIgnoreCase) &&
                !x.Contains("AssemblyInfo.cs", StringComparison.OrdinalIgnoreCase));
        }

    }
}
