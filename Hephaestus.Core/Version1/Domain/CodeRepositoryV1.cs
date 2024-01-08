using System;
using System.Collections.Generic;
using System.Linq;

namespace Hephaestus.Core.Version1.Domain
{
    public class CodeRepositoryV1
    {
        public string Name { get; }
        public string Path { get; }
        public IReadOnlyCollection<SolutionV1> Solutions => _solutions.Select(x => x.Value).ToList().AsReadOnly();
        public IReadOnlyCollection<ProjectV1> Projects => _projects.Select(x => x.Value).ToList().AsReadOnly();
        public Dictionary<string, List<ProjectV1>> NamespaceLookup;

        private readonly Dictionary<string, SolutionV1> _solutions;
        private readonly Dictionary<string, ProjectV1> _projects;
        private readonly Dictionary<string, string> _fileContents;

        internal CodeRepositoryV1(string path, string name)
        {
            Path = path;
            Name = name;
            _solutions = new Dictionary<string, SolutionV1>(StringComparer.OrdinalIgnoreCase);
            _projects = new Dictionary<string, ProjectV1>(StringComparer.OrdinalIgnoreCase);
            _fileContents = new Dictionary<string, string>();
            NamespaceLookup = new Dictionary<string, List<ProjectV1>>();
        }

        internal bool TryAddSolution(SolutionV1 solution)
        {
            return _solutions.TryAdd(solution.Path, solution);
        }

        internal bool TryAddProject(ProjectV1 project)
        {
            if (!_projects.TryAdd(project.Path, project)) return false;

            foreach (var nspace in project.Namespaces)
            {
                if (NamespaceLookup.TryGetValue(nspace, out var list))
                {
                    list.Add(project);
                }
                else
                {
                    NamespaceLookup.Add(nspace, new List<ProjectV1> { project });
                }
            }
            return true;

        }

        internal void AddFileContent(string fileName, string fileContent)
        {
            _fileContents.TryAdd(fileName, fileContent);
        }

        public string GetFileContent(string fileName)
        {
            return !_fileContents.ContainsKey(fileName) ? string.Empty : _fileContents[fileName];
        }

        public ProjectV1 GetProject(string filePath)
        {
            return _projects[filePath];
        }
    }
}