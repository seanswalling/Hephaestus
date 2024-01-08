using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Version1.Domain;
using Hephaestus.Core.Version1.Parsing;

namespace Hephaestus.Core.Version1.Building
{
    internal class DomainV1Builder
    {
        private readonly IFileProvider _fileProvider;

        public DomainV1Builder(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public CodeRepositoryV1 BuildCodeRepository(string repoPath, string repoName)
        {
            var repo = new CodeRepositoryV1(repoPath, repoName);
            foreach (var key in Projects())
            {
                var project = BuildProject(key);
                repo.TryAddProject(project);
                repo.AddFileContent(project.Path, _fileProvider.GetFile(project.Path));
            }

            foreach (var key in Solutions())
            {
                var sln = BuildSolution(key);
                repo.TryAddSolution(sln);
            }

            foreach (var project in repo.Projects)
            {
                foreach (var reference in project.References)
                {
                    var referenceFullPathLocation = Path.GetFullPath(Path.Join(Path.GetDirectoryName(project.Path), reference.RelativePath));
                    repo.GetProject(referenceFullPathLocation).AddUsage(project.Path);
                }
            }
            return repo;
        }

        private IReadOnlyCollection<string> Projects()
        {
            return _fileProvider.QueryByExtension(".csproj").Select(x => x.Key).ToArray().AsReadOnly();
        }

        private IReadOnlyCollection<string> Solutions()
        {
            return _fileProvider.QueryByExtension(".sln").Select(x => x.Key).ToArray().AsReadOnly();
        }

        public SolutionV1 BuildSolution(string path)
        {
            var parser = new SolutionV1Parser(path);
            return new SolutionV1(parser.Name, parser.FilePath, parser.Projects);
        }

        public ProjectV1 BuildProject(string projectPath)
        {
            var parser = new ProjectV1Parser(projectPath, _fileProvider);

            return new ProjectV1(
                parser.ProjectName,
                parser.FilePath,
                parser.Format,
                parser.Framework,
                parser.OutputType,
                parser.Globs,
                parser.ProjectReferences,
                parser.PackageReferences,
                parser.EmbeddedResources,
                parser.EmptyFolders,
                parser.UsingDirectives,
                parser.CompiledFiles,
                parser.Namespaces);
        }
    }


}
