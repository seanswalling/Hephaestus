using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public class SolutionParser : ISolutionParser
    {
        private static string CsharpProject => "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\")";
        private static string AspnetcoreProject => "Project(\"{9A19103F-16F7-4668-BE54-9A1E7A4F7556}\")";

        private readonly IProjectParser _projectParser;
        private readonly IFileCollection _fileCollection;

        public SolutionParser(IProjectParser projectParser, IFileCollection fileCollection)
        {
            _projectParser = projectParser;
            _fileCollection = fileCollection;
        }

        public Solution Parse(string filePath, string fileContent)
        {
            var listOfProjectFiles = fileContent
                .Split(Environment.NewLine)
                .Where(line => line.StartsWith(CsharpProject) || line.StartsWith(AspnetcoreProject))
                .Select(line => line.Split(',')[1])
                .Select(l => Path.GetFullPath(Path.Combine(Directory.GetParent(filePath)!.FullName, l.Trim().Replace("\"", string.Empty))));

            var projects = listOfProjectFiles.Select(p => _projectParser.Parse(p, XDocument.Parse(_fileCollection.GetContent(p))));

            var fileName = Path.GetFileName(filePath);

            return new Solution(fileName, projects);
        }
    }
}
