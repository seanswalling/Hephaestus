using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hephaestus.Core.Parsing
{
    internal class SolutionParser
    {
        private static string CsharpProject => "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\")";
        private static string AspnetcoreProject => "Project(\"{9A19103F-16F7-4668-BE54-9A1E7A4F7556}\")";

        internal string Content { get; private set; }
        internal string FilePath { get; private set; }
        internal string Name { get; private set; }
        internal IReadOnlyCollection<string> Projects { get; private set; }
        internal SolutionParser(string filePath)
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

            //        //public static IEnumerable<Func<Project>> ParseFile(string slnPath, IEnumerable<string> lines)
            //        //{
            //        //    foreach (var line in lines)
            //        //    {
            //        //        if (!line.StartsWith("Project"))
            //        //        {
            //        //            continue;
            //        //        }

            //        //        var itemTypeIdentifier = Guid.Parse(line.AsSpan(10, 36));

            //        //        if (!_solutionItemTypeDictionary.TryGetValue(itemTypeIdentifier, out SolutionItemType solutionItemType))
            //        //        {
            //        //            throw new ArgumentException($"Unknown Solution Item Type: {itemTypeIdentifier}");
            //        //        }

            //        //        if (solutionItemType != SolutionItemType.CSharpProject)
            //        //        {
            //        //            continue;
            //        //        }

            //        //        var targetRelativePath = line.Split(',')[1];
            //        //        var currParent = Directory.GetParent(slnPath)?.FullName;
            //        //        if (currParent == null) throw new InvalidOperationException($"No Parent Path for {slnPath}");
            //        //        var targetFullPath = Path.Combine(currParent, targetRelativePath);

            //        //        //yield return () =>
            //        //    }
        }


        //private static readonly Dictionary<Guid, SolutionItemType> _solutionItemTypeDictionary = new()
        //{
        //    { new Guid("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC"), SolutionItemType.CSharpProject },
        //    { new Guid("9A19103F-16F7-4668-BE54-9A1E7A4F7556"), SolutionItemType.CSharpProject },
        //    { new Guid("2150E333-8FDC-42A3-9474-1A3956D46DE8"), SolutionItemType.SolutionFolder },
        //};
    }

    //public enum SolutionItemType
    //{
    //    Unknown,
    //    SolutionFolder,
    //    CSharpProject
    //}



}
