using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hephaestus.Avalonia.ViewModels;
using Hephaestus.Core.Application;
using Hephaestus.Core.Domain;

namespace Hephaestus.Avalonia.Models
{
    public class RepositoryProviderUIAdapter
    {
        private readonly CodeRepository _repository;
        private readonly Application _application;
        private readonly Dictionary<string, Project> _projects;
        private readonly Dictionary<string, Solution> _solutions;
        private readonly Dictionary<string, int> _sdkUsages;
        private readonly Dictionary<string, int> _frameworkUsages;
        private readonly Dictionary<string, List<Project>> _usages;
        private readonly List<Action> _subscriptions;

        public RepositoryProviderUIAdapter(Application application)
        {
            _application = application;
            _repository = application.Parse();
            _solutions = _repository.Solutions.ToDictionary(x => x.Name, x => x);
            _projects = _repository.Solutions
                .SelectMany(p => p.Projects)
                .DistinctBy(x => x.Metadata.ProjectPath)
                .ToDictionary(x => x.Metadata.ProjectPath, x => x);
            _sdkUsages = new Dictionary<string, int>();
            _frameworkUsages = new Dictionary<string, int>();
            _usages = new Dictionary<string, List<Project>>();

            foreach (var item in _projects)
            {
                _sdkUsages.Add(item.Key, 0);
                _frameworkUsages.Add(item.Key, 0);
                _usages.Add(item.Key, new List<Project>());
            }

            foreach (var proj in _projects.Values)
            {
                foreach (var usedProj in proj.References.ProjectReferences)
                {
                    var fullPath = Path.GetFullPath(Path.Join(Path.GetDirectoryName(proj.Metadata.ProjectPath), usedProj.RelativePath));
                    _usages[fullPath].Add(proj);
                    if (proj.Metadata.Format == ProjectFormat.Sdk)
                    {
                        _sdkUsages[fullPath]++;
                    }
                    else
                    {
                        _frameworkUsages[fullPath]++;
                    }
                }
            }

            _subscriptions = new List<Action>();
        }

        public SolutionViewModel[] GetSolutions()
        {
            return _repository.Solutions.Select(s => new SolutionViewModel(s.Name, string.Empty)).ToArray();
        }

        public ProjectViewModel[] GetProjects()
        {
            return _projects.Values
                .Select(MapProject)
                .ToArray();
        }

        public ProjectViewModel GetProject(string filePath)
        {
            return MapProject(_projects[filePath]);
        }

        public string GetFileContent(string filePath)
        {
            return _application.GetFileContent(filePath);
        }

        private ProjectViewModel MapProject(Project p)
        {
            var references = p.References.ProjectReferences;
            //var usages = _sdkUsages[p.Metadata.ProjectPath] + _frameworkUsages[p.Metadata.ProjectPath];
            var frameworkRefCount = references.Select(x => _projects[Path.GetFullPath(Path.Join(Path.GetDirectoryName(p.Metadata.ProjectPath), x.RelativePath))]).Count(x => x.Metadata.Format == ProjectFormat.Framework);

            return new ProjectViewModel(
                p.Name,
                p.Metadata.ProjectPath,
                p.Metadata.Format,
                p.Metadata.OutputType,
                p.Metadata.Framework,
                references.Select(x => Path.GetFullPath(Path.Join(Path.GetDirectoryName(p.Metadata.ProjectPath), x.RelativePath))).ToArray(),
                _usages[p.Metadata.ProjectPath].Select(x => x.Metadata.ProjectPath).ToArray(),
                frameworkRefCount,
                _frameworkUsages[p.Metadata.ProjectPath]);
        }

        public void NotifySubscribers()
        {
            foreach (var subscription in _subscriptions)
            {
                subscription();
            }
        }

        public void Subscribe(Action subscription)
        {
            _subscriptions.Add(subscription);
        }
    }
}
