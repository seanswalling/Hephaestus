using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hephaestus.Core.Version1.Parsing
{
    internal class SolutionV1Parser
    {
        private static string CsharpProject => "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\")";
        private static string AspnetcoreProject => "Project(\"{9A19103F-16F7-4668-BE54-9A1E7A4F7556}\")";

        internal string Content { get; private set; }
        internal string FilePath { get; private set; }
        internal string Name { get; private set; }
        internal IReadOnlyCollection<string> Projects { get; private set; }

        internal SolutionV1Parser(string filePath)
        {
            Content = File.ReadAllText(filePath);
            FilePath = filePath;
            Name = Path.GetFileNameWithoutExtension(filePath);

            Projects = Content
                .Split(Environment.NewLine)
                .Where(line => line.StartsWith(CsharpProject) || line.StartsWith(AspnetcoreProject))
                .Select(line => line.Split(',')[1])
                .Select(l => Path.GetFullPath(Path.Combine(Directory.GetParent(filePath)!.FullName, l.Trim().Replace("\"", string.Empty))))
                .ToList()
                .AsReadOnly();
        }
    }
}
