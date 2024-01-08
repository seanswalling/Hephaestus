using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Version1.Domain;

namespace Hephaestus.Core.Version1.Parsing
{
    internal class ProjectV1Parser
    {
        internal XDocument Content { get; private set; }
        internal string ProjectName { get; private set; }
        internal string FilePath { get; private set; }
        internal ProjectFormat Format { get; private set; }
        internal OutputType OutputType { get; private set; }
        internal Framework Framework { get; private set; }
        internal ICollection<EmbeddedResourceV1> EmbeddedResources { get; private set; }
        internal ICollection<ProjectReferenceV1> ProjectReferences { get; private set; }
        internal ICollection<PackageReferenceV1> PackageReferences { get; private set; }
        internal ICollection<string> EmptyFolders { get; private set; }
        internal List<Glob> Globs { get; private set; }
        public IEnumerable<string> UsingDirectives { get; set; }
        public IEnumerable<string> CompiledFiles { get; set; }
        public IEnumerable<string> Namespaces { get; set; }

        internal ProjectV1Parser(string path, IFileProvider fileProvider)
        {
            Content = XDocument.Load(new StringReader(fileProvider.GetFile(path)));
            FilePath = path;
            Format = ParseProjectFormat(Content);
            IProjectParserV1 innerParser = null;

            if (Format == ProjectFormat.Sdk)
            {
                //innerParser = new SdkProjectFormatParser(Content, FilePath, fileProvider);
                Globs =
                [
                    new Glob(".cs", Path.GetDirectoryName(FilePath)!),
                    new Glob(".resx", Path.GetDirectoryName(FilePath)!),
                ];
            }
            else
            {
                //innerParser = new LegacyProjectFormatParser(Content, FilePath, fileProvider);
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