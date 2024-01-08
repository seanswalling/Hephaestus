using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Version1.Domain;

namespace Hephaestus.Core.Version1.Parsing
{
    internal class LegacyProjectParser : IProjectParserV1
    {
        private readonly XDocument _content;
        private static readonly XNamespace Namespace = "http://schemas.microsoft.com/developer/msbuild/2003";
        private readonly string _directory;
        private readonly string _directoryPlusSep;
        private readonly IFileProvider _fileProvider;

        public LegacyProjectParser(XDocument content, string projectPath, IFileProvider fileProvider)
        {
            _directory = Directory.GetParent(projectPath)!.FullName;
            _directoryPlusSep = _directory + Path.DirectorySeparatorChar;
            _content = content;
            _fileProvider = fileProvider;
        }

        public string ParseProjectName()
        {
            return _content.Descendants(Namespace + "AssemblyName").SingleOrDefault()?.Value ??
                   _content.Descendants(Namespace + "RootNamespace").Single().Value;
        }

        public OutputType ParseOutputType()
        {
            return new OutputTypeTranslator().Translate(_content.Descendants(Namespace + "OutputType").SingleOrDefault()?.Value);
        }

        public Framework ParseTargetFrameworkMoniker()
        {
            return new TfmTranslator().Translate(_content.Descendants(Namespace + "TargetFrameworkVersion").SingleOrDefault()?.Value);
        }

        public ICollection<EmbeddedResourceV1> ParseEmbeddedResources()
        {
            return _content.Descendants(Namespace + "EmbeddedResource")
                .Select(x =>
                {
                    var relativePath = x.Attribute("Include")!.Value;
                    var linkedPath = x.Descendants(Namespace + "Link").SingleOrDefault()?.Value;
                    return new EmbeddedResourceV1(relativePath, linkedPath);
                }).ToArray();
        }

        public ICollection<ProjectReferenceV1> ParseProjectReferences()
        {
            return _content.Descendants(Namespace + "ProjectReference")
                .Select(x =>
                {
                    //var name = x.Descendants(Namespace + "Name").SingleOrDefault()?.Value ?? "Unknown";
                    var relativePath = x.Attribute("Include")?.Value ?? throw new InvalidDataException();
                    // var path = Path.GetFullPath(Path.Combine(_directory, location));
                    return new ProjectReferenceV1(relativePath);
                }).ToList();
        }

        public ICollection<PackageReferenceV1> ParsePackageReferences()
        {
            var pkgConfPath = Path.Combine(_directory, "packages.config");

            //This file might not exist, so we didn't read it in.
            if (!_fileProvider.HasFile(pkgConfPath)) return Array.Empty<PackageReferenceV1>();
            //if (!SingletonFileContentCache.Instance.HasFile(pkgConfPath)) return Array.Empty<PackageReference>();
            var packageConfigContent = _fileProvider.GetFile(pkgConfPath);
            //var packageConfigContent = SingletonFileContentCache.Instance.GetFile(pkgConfPath);

            return XDocument.Parse(string.Concat(packageConfigContent))
                .Descendants("package")
                .Select(x =>
                {
                    var idAttribute = x.Attribute("id");
                    var versionAttribute = x.Attribute("version");

                    if (idAttribute == null)
                    {
                        throw new NullReferenceException("Package Reference Id was null");
                    }

                    if (versionAttribute == null)
                    {
                        throw new NullReferenceException("Package Reference Version was null");
                    }

                    return new PackageReferenceV1(idAttribute.Value, versionAttribute.Value);
                }).ToArray();
        }

        public ICollection<string> ParseEmptyFolders()
        {
            return _content.Descendants(Namespace + "Folder")
                .Select(x => x.Attribute("Include")?.Value ?? string.Empty).ToList();
        }

        public IEnumerable<string> ParseUsingDirectives()
        {
            return _fileProvider.QueryByExtension(".cs")
                .Where(x => x.Key.Contains(_directoryPlusSep))
                .Select(x => x.Value)
                .SelectMany(GetUsingLines)
                .Distinct();
            //.ToArray();
        }

        public IEnumerable<string> ParseNamespaces()
        {
            return _fileProvider.QueryByExtension(".cs")
                .Where(x => x.Key.Contains(_directoryPlusSep))
                .Select(x => x.Value)
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

        private static bool FileContainsNamespace(string fileContent)
        {
            return fileContent.IndexOf("namespace", StringComparison.Ordinal) != -1;
        }

        private static string ProcessFileForNamespace(string fileContent)
        {
            var start = fileContent.IndexOf("namespace", StringComparison.Ordinal);
            var end = fileContent.IndexOf(Environment.NewLine, start, StringComparison.OrdinalIgnoreCase);
            return fileContent[start..end];
        }

        private static string ProcessNamespaceForLocation(string line)
        {
            var start = line.IndexOf(" ", StringComparison.OrdinalIgnoreCase) + 1;
            return line[start..];
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
                .Where(x => x.Key.Contains(_directoryPlusSep))
                .Select(x => x.Key);
        }
    }
}