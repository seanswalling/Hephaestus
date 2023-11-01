using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    internal class SdkProjectFormatParser : IProjectParser
    {
        private readonly XDocument _content;
        private readonly string _projectPath;
        private readonly string _directory;
        private readonly IFileProvider _fileProvider;

        public SdkProjectFormatParser(XDocument content, string projectPath, IFileProvider fileProvider)
        {
            _content = content;
            _projectPath = projectPath;
            _fileProvider = fileProvider;
            _directory = Directory.GetParent(_projectPath)!.FullName + Path.DirectorySeparatorChar;
        }

        public string ParseProjectName()
        {
            return _content.Descendants("AssemblyName").SingleOrDefault()?.Value ??
                   _content.Descendants("RootNamespace").SingleOrDefault()?.Value ?? "Unknown";
        }

        public OutputType ParseOutputType()
        {
            return OutputTypeParser.Parse(_content.Descendants("OutputType").SingleOrDefault()?.Value);
        }

        public Framework ParseTargetFrameworkMoniker()
        {
            if (_content.Descendants("TargetFrameworks").Any())
            {
                return TfmParser.Parse(_content.Descendants("TargetFrameworks").SingleOrDefault()?.Value.Split(",")
                    .First());
                //TODO Not Ideal only takes the first in a collection.  Need a better way.
            }

            return TfmParser.Parse(_content.Descendants("TargetFramework").SingleOrDefault()?.Value);
        }

        //public ICollection<Target> ParseBuildTargets()
        //{
        //    return MsBuildTargets.TargetTypes.SelectMany(targetType =>
        //        _content.Descendants(targetType)
        //            .Where(x => x.Attribute("Remove") == null)
        //            .Select(x =>
        //            {
        //                var name = (x.Attribute("Include") ?? x.Attribute("Update"))!.Value;
        //                var linkedPath = x.Attribute("Link")?.Value;
        //                return new Target(name, targetType, linkedPath);
        //            })).ToArray();
        //}

        public ICollection<EmbeddedResource> ParseEmbeddedResources()
        {
            return _content.Descendants("EmbeddedResource")
                .Select(x =>
                {
                    var include = x.Attribute("Include");
                    if (include == null) return new EmbeddedResource("Unknown Embedded Resource", null);
                    var relativePath = include.Value;
                    var linkedPath = x.Attribute("Link")?.Value;
                    return new EmbeddedResource(relativePath, linkedPath);
                }).ToArray();
        }

        public ICollection<ProjectReference> ParseProjectReferences()
        {
            return _content.Descendants("ProjectReference")
                .Select(x =>
                {
                    var relativePath = x.Attribute("Include")?.Value ?? throw new InvalidDataException();

                    //var path = Path.GetFullPath(Path.Combine(_dir, relativePath));

                    return new ProjectReference(relativePath);
                }).ToArray();
        }

        public ICollection<PackageReference> ParsePackageReferences()
        {
            return _content.Descendants("ItemGroup").Descendants("PackageReference")
                .Select(x =>
                {
                    var name = x.Attribute("Include")?.Value ?? x.Attribute("include")?.Value ?? throw new InvalidDataException();
                    var version = x.Attribute("Version")?.Value ?? x.Attribute("version")?.Value ?? throw new InvalidDataException();

                    return new PackageReference(name, version);
                }).ToArray();
        }

        public ICollection<string> ParseEmptyFolders()
        {
            return _content.Descendants("Folder")
                .Select(x => x.Attribute("Include")?.Value ?? string.Empty).ToList();
        }

        public IEnumerable<string> ParseUsingDirectives()
        {
            return _fileProvider.QueryByExtension(".cs")
                .Where(x => x.Key.Contains(_directory))
                .Select(x => x.Value)
                .SelectMany(GetUsingLines)
                .Distinct();
            //.ToArray();
        }

        public IEnumerable<string> ParseNamespaces()
        {
            return _fileProvider.QueryByExtension(".cs")
                .Where(IsFileRelevantToProject)
                .Select(x => x.Value)
                .Select(GetNamespaceIndex)
                .Where(FileContainsNamespace)
                .Select(ProcessFileForNamespace)
                .Select(ProcessNamespaceForLocation)
                .Distinct();
            //.ToArray();
        }

        private static IEnumerable<string> GetUsingLines(string fileContent)
        {
            using var str = new StringReader(fileContent);
            while (str.Peek() != -1)
            {
                var line = str.ReadLine()!;
                if (line.StartsWith("using", StringComparison.OrdinalIgnoreCase))
                {
                    yield return line.Contains("=") ?
                        line.Split("=")[1].Trim().Trim(';') :
                        line.StartsWith("using static", StringComparison.OrdinalIgnoreCase) ?
                            line.Split(" ")[2].Trim().Trim(';') :
                            line.Split(" ")[1].Trim().Trim(';');
                }
                //Ignore global using(s) for now.

                //We've hit namespace, escape to save time reading rest of string.
                //Using(s) could still occur, but assume otherwise for sanity.
                if (line.StartsWith("namespace", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }
        }

        private static (bool, int, string) GetNamespaceIndex(string fileContent)
        {
            var index = fileContent.IndexOf("namespace", StringComparison.Ordinal);
            return (index != -1, index, fileContent);
        }

        private bool IsFileRelevantToProject(KeyValuePair<string, string> input)
        {
            return input.Key.Contains(_directory);
        }

        private static bool FileContainsNamespace((bool, int, string) input)
        {
            return input.Item1;
        }

        private static string ProcessFileForNamespace((bool, int, string) input)
        {
            var start = input.Item2;
            var end = input.Item3.IndexOf(Environment.NewLine, start, StringComparison.Ordinal);
            return input.Item3[start..end];
        }

        private static string ProcessNamespaceForLocation(string line)
        {
            var start = line.IndexOf(" ", StringComparison.OrdinalIgnoreCase) + 1;
            var nonNestedNamespace = line[^1] == ';';
            return nonNestedNamespace ? line[start..^1] : line[start..];
        }

        //private static string GetUsingDirective(string lineFeed)
        //{
        //    //This might be an alias or a static, CURSE YOU!
        //    return lineFeed.Contains("=") ?
        //        lineFeed.Split("=")[1].Split(";")[0].Trim() :
        //        lineFeed.StartsWith("using static", StringComparison.OrdinalIgnoreCase) ?
        //            lineFeed.Split(" ")[2].Split(";")[0] :
        //            lineFeed.Split(" ")[1].Split(";")[0];
        //}

        public IEnumerable<string> ParseCompiledFiles()
        {
            return _fileProvider.QueryByExtension(".cs")
                .Where(x => x.Key.Contains(Path.GetDirectoryName(_projectPath)!))
                .Select(x => x.Key);
        }
    }
}