using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    internal class ProjectParser
    {
        internal XDocument Content { get; private set; }
        internal string ProjectName { get; private set; }
        internal string FilePath { get; private set; }
        internal ProjectFormat Format { get; private set; }
        internal OutputType OutputType { get; private set; }
        internal Framework Framework { get; private set; }
        internal ICollection<EmbeddedResource> EmbeddedResources { get; private set; }
        internal ICollection<ProjectReference> ProjectReferences { get; private set; }
        internal ICollection<PackageReference> PackageReferences { get; private set; }
        internal ICollection<string> EmptyFolders { get; private set; }
        internal List<Glob> Globs { get; private set; }
        public IEnumerable<string> UsingDirectives { get; set; }
        public IEnumerable<string> CompiledFiles { get; set; }
        public IEnumerable<string> Namespaces { get; set; }

        internal ProjectParser(string path, IFileProvider fileProvider)
        {
            Content = XDocument.Load(new StringReader(fileProvider.GetFile(path)));
            FilePath = path;
            Format = ParseProjectFormat(Content);
            IProjectParser innerParser;

            if (Format == ProjectFormat.Sdk)
            {
                innerParser = new SdkProjectFormatParser(Content, FilePath, fileProvider);
                Globs = new List<Glob>
                {
                    new()
                    {
                        FileExtension = ".cs",
                        RootPath = Path.GetDirectoryName(FilePath)!
                    },
                    new()
                    {
                        FileExtension = ".resx",
                        RootPath = Path.GetDirectoryName(FilePath)!
                    },
                };
            }
            else
            {
                innerParser = new LegacyProjectFormatParser(Content, FilePath, fileProvider);
                Globs = new List<Glob>();
            }

            ProjectName = innerParser.ParseProjectName();//Path.GetFileNameWithoutExtension(FilePath);
            OutputType = innerParser.ParseOutputType();
            Framework = innerParser.ParseTargetFrameworkMoniker();
            EmbeddedResources = innerParser.ParseEmbeddedResources();
            ProjectReferences = innerParser.ParseProjectReferences();
            PackageReferences = innerParser.ParsePackageReferences();
            EmptyFolders = innerParser.ParseEmptyFolders();
            CompiledFiles = innerParser.ParseCompiledFiles();
            UsingDirectives = innerParser.ParseUsingDirectives();
            Namespaces = innerParser.ParseNamespaces();
        }

        internal static ProjectFormat ParseProjectFormat(XDocument content)
        {
            if (content.XPathEvaluate("//@Sdk") is not IEnumerable<object> xpathResult) throw new ArgumentException("invalid xml");

            return xpathResult.Any() ? ProjectFormat.Sdk : ProjectFormat.Framework;
        }
    }
}