using System;
using System.Collections.Generic;
using System.Linq;

namespace Hephaestus.Core.Domain
{
    public class CodeRepository
    {
        public string Name { get; }
        public string Path { get; }
        public IReadOnlyCollection<Solution> Solutions => _solutions.Select(x => x.Value).ToList().AsReadOnly();
        public IReadOnlyCollection<Project> Projects => _projects.Select(x => x.Value).ToList().AsReadOnly();
        public Dictionary<string, List<Project>> NamespaceLookup;

        private readonly Dictionary<string, Solution> _solutions;
        private readonly Dictionary<string, Project> _projects;
        private readonly Dictionary<string, string> _fileContents;

        internal CodeRepository(string path, string name)
        {
            Path = path;
            Name = name;
            _solutions = new Dictionary<string, Solution>(StringComparer.OrdinalIgnoreCase);
            _projects = new Dictionary<string, Project>(StringComparer.OrdinalIgnoreCase);
            _fileContents = new Dictionary<string, string>();
            NamespaceLookup = new Dictionary<string, List<Project>>();
        }

        internal bool TryAddSolution(Solution solution)
        {
            return _solutions.TryAdd(solution.Path, solution);
        }

        internal bool TryAddProject(Project project)
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
                    NamespaceLookup.Add(nspace, new List<Project> { project });
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

        public Project GetProject(string filePath)
        {
            return _projects[filePath];
        }
    }
}