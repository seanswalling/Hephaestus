using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Version1.Domain
{
    public class ProjectV1
    {
        public string ProjectName { get; }
        public string Path { get; }
        public ProjectFormat Format { get; }
        public Framework Framework { get; }
        public OutputType OutputType { get; }
        public IReadOnlyCollection<Glob> Globs { get; }
        public IReadOnlyCollection<string> Solutions => _solutions.ToList().AsReadOnly();
        public IReadOnlyCollection<string> Usages => _usages.ToList().AsReadOnly();
        public IReadOnlyCollection<ProjectReferenceV1> References => _projectReferences.AsReadOnly();
        public IReadOnlyCollection<PackageReferenceV1> Packages => _packages.AsReadOnly();
        public IEnumerable<EmbeddedResourceV1> EmbeddedResources => _embeddedResources.ToArray().AsReadOnly();
        public IEnumerable<string> EmptyFolders => _emptyFolders.AsReadOnly();
        public IEnumerable<string> UsingDirectives => _usingDirectives.AsReadOnly();
        public IReadOnlyCollection<ProjectReferenceV1> DirectReferences => _directProjectReferences.AsReadOnly();
        public IReadOnlyCollection<PackageReferenceV1> DirectPackages => _directPackageReferences.AsReadOnly();
        public readonly string[] Namespaces;

        private readonly HashSet<string> _solutions;
        private readonly HashSet<string> _usages;

        private readonly string[] _usingDirectives;
        //private readonly string[] _compiledFiles;

        private readonly EmbeddedResourceV1[] _embeddedResources;
        private readonly ProjectReferenceV1[] _projectReferences;
        private ProjectReferenceV1[] _directProjectReferences;
        private readonly PackageReferenceV1[] _packages;
        private PackageReferenceV1[] _directPackageReferences;
        private readonly string[] _emptyFolders;

        private ITestProjectPredicate _testProjectPredicate;
        private ITestDependencyProvider _testDependencyProvider;

        internal ProjectV1(
            string name,
            string path,
            ProjectFormat format,
            Framework framework,
            OutputType outputType,
            IReadOnlyCollection<Glob> globs,
            IEnumerable<ProjectReferenceV1> projectReferences,
            IEnumerable<PackageReferenceV1> packageReferences,
            IEnumerable<EmbeddedResourceV1> embeddedResources,
            IEnumerable<string> emptyFolders,
            IEnumerable<string> usingDirectives,
            IEnumerable<string> compiledFiles,
            IEnumerable<string> namespaces)
        {
            ProjectName = name;
            Path = path;
            Format = format;
            OutputType = outputType;
            Framework = framework;
            Globs = globs;
            _projectReferences = projectReferences.ToArray();
            _packages = packageReferences.ToArray();
            _embeddedResources = embeddedResources.ToArray();
            _emptyFolders = emptyFolders.ToArray();
            _usages = new HashSet<string>();
            _solutions = new HashSet<string>();
            _usingDirectives = usingDirectives.ToArray();
            Namespaces = namespaces.ToArray();
            _directPackageReferences = Array.Empty<PackageReferenceV1>();
            _directProjectReferences = Array.Empty<ProjectReferenceV1>();
        }

        internal void LinkSln(string slnPath)
        {
            _solutions.Add(slnPath);
        }

        internal void AddUsage(string projectPath)
        {
            _usages.Add(projectPath);
        }

        public void ProcessDirectDependencies(Dictionary<string, List<ProjectV1>> namespaceLookup, List<ProcessDirectDependencyResolver> usingToDependencyResolvers)
        {
            var projectLookup = _projectReferences.ToDictionary(x => System.IO.Path.GetFileNameWithoutExtension(x.RelativePath), x => x, StringComparer.OrdinalIgnoreCase);
            var packageLookup = _packages.ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);
            var backupPackages = LoadBackupPackages().ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);
            var packages = new List<PackageReferenceV1>();
            var projects = new List<ProjectReferenceV1>();

            foreach (var usingDirective in _usingDirectives)
            {
                if (namespaceLookup.TryGetValue(usingDirective, out var candidates))
                {
                    //local dependency, fetch from projects
                    foreach (var candidate in candidates)
                    {
                        var key = System.IO.Path.GetFileNameWithoutExtension(candidate.Path);
                        if (projectLookup.TryGetValue(key, out var project))
                        {
                            projects.AddReference(project);
                        }
                    }
                }
                else
                {
                    var resolvers = usingToDependencyResolvers.Where(r => r.Selector(usingDirective)).ToArray();
                    foreach (var resolver in resolvers)
                    {
                        var dependencyNames = resolver.Transformer(usingDirective);
                        foreach (var dependencyName in dependencyNames)
                        {
                            if (packageLookup.TryGetValue(dependencyName, out var package))
                            {
                                packages.AddReference(package);
                            }
                            else if (backupPackages.TryGetValue(dependencyName, out var backupPackage))
                            {
                                packages.AddReference(backupPackage);
                            }
                            else
                            {
                                if (dependencyName == "System" || Namespaces.Contains(usingDirective)) continue;


                                if (resolver.ThrowOnError(usingDirective))
                                    throw new InvalidOperationException(
                                        $"Could not determine dependency named {dependencyName}");
                            }
                        }
                    }
                }
            }

            if (_testProjectPredicate.IsTestProject(ProjectName))
            {
                ProcessTestDependencies(packages, projects);

                //I'm assuming our pre-test namespace is all the same, unsure of scenarios where it deviates.
                var targets = Namespaces.Select(ProcessTestNamespaceForTarget).Distinct();

                foreach (var target in targets)
                {
                    if (namespaceLookup.TryGetValue(target, out var candidates))
                    {
                        //local dependency, fetch from projects
                        foreach (var candidate in candidates)
                        {
                            var key = System.IO.Path.GetFileNameWithoutExtension(candidate.Path);
                            if (projectLookup.TryGetValue(key, out var project))
                            {
                                projects.AddReference(project);
                            }
                        }
                    }
                }
            }

            _directPackageReferences = packages.DistinctBy(x => x.Name).ToArray();
            _directProjectReferences = _directProjectReferences.Concat(projects).DistinctBy(x => x.RelativePath).ToArray();
        }

        private static string ProcessTestNamespaceForTarget(string ns)
        {
            var end = ns.IndexOf(".Tests", StringComparison.OrdinalIgnoreCase);

            return end == -1 ? string.Empty : ns[..end];
        }

        private void ProcessTestDependencies(List<PackageReferenceV1> packages, List<ProjectReferenceV1> projects)
        {
            foreach (var package in _testDependencyProvider.ProvidePackageReferences(this))
            {
                packages.AddReference(package);
            }

            foreach (var project in _testDependencyProvider.ProvideProjectReferences(this))
            {
                projects.AddReference(project);
            }
        }


        public void ProcessAddedProjectDependencies(List<ProcessDirectDependencyResolver> projectReferencePredicates)
        {
            var projects = new List<ProjectReferenceV1>();
            foreach (var predicate in projectReferencePredicates)
            {
                foreach (var reference in _projectReferences)
                {
                    if (predicate.Selector(reference.RelativePath))
                        projects.Add(reference);
                }
            }

            _directProjectReferences = _directProjectReferences.Concat(projects).DistinctBy(x => x.RelativePath).ToArray();
        }

        //some projects don't have a package config!
        private PackageReferenceV1[] LoadBackupPackages()
        {
            var backup = Assembly.GetExecutingAssembly()
                .GetManifestResourceNames()
                .Single(mrn => mrn.EndsWith("backup.txt"));

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(backup);
            using var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();

            try
            {
                return XDocument.Parse(content)
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
            catch (Exception _)
            {
                throw new Exception($"Failed to parse: {content}", _);
            }
        }
    }
}
