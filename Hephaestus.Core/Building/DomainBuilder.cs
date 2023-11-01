using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;

namespace Hephaestus.Core.Building
{
    internal class DomainBuilder
    {
        private readonly IFileProvider _fileProvider;

        public DomainBuilder(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public CodeRepository BuildCodeRepository(string repoPath, string repoName)
        {
            var repo = new CodeRepository(repoPath, repoName);
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

        public Solution BuildSolution(string path)
        {
            var parser = new SolutionParser(path);
            return new Solution(parser.Name, parser.FilePath, parser.Projects);
        }

        public Project BuildProject(string projectPath)
        {
            var parser = new ProjectParser(projectPath, _fileProvider);

            return new Project(
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
