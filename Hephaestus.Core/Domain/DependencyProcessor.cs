using System;
using System.Collections.Generic;
using System.Linq;

namespace Hephaestus.Core.Domain
{
    public class DependencyProcessor
    {
        private readonly INamespaceLookup _namespaceLookup;
        private readonly ITestDependencyProvider _testDependencyProvider;
        private readonly ITestProjectPredicate _testProjectPredicate;
        public DependencyProcessor(INamespaceLookup namespaceLookup, ITestDependencyProvider testDependencyProvider, ITestProjectPredicate testProjectPredicate)
        {
            _namespaceLookup = namespaceLookup;
            _testDependencyProvider = testDependencyProvider;
            _testProjectPredicate = testProjectPredicate;
        }

        //List<ProcessDirectDependencyResolver> usingToDependencyResolvers
        public void ProcessDirectDependencies(Project project)
        {
            var existingDependencies = project.References.ToDictionary(x => System.IO.Path.GetFileNameWithoutExtension(x.RelativePath), x => x, StringComparer.OrdinalIgnoreCase);
            var packageDependencies = project.Packages.ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);
            //var backupPackages = LoadBackupPackages().ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);
            var directPackageDependencies = new List<PackageReference>();
            var directProjectDependencies = new List<ProjectReference>();

            foreach (var usingDirective in project.UsingDirectives)
            {
                //refactor below, too many args
                AddReferencesForNamespace(usingDirective, existingDependencies, packageDependencies, directProjectDependencies, directPackageDependencies);
            }

            if (_testProjectPredicate.IsTestProject(project.ProjectName))
            {
                AddMandatoryNonDirectDependencies(project, directPackageDependencies, directProjectDependencies);

                var subjectNamespaces = project.Namespaces.Select(ProcessTestNamespaceForTarget).Distinct();

                foreach (var subjectNamespace in subjectNamespaces)
                {
                    var projectReferences = _namespaceLookup.GetProjectsForNamespace(subjectNamespace);

                    foreach (var projectReference in projectReferences)
                    {
                        var key = System.IO.Path.GetFileNameWithoutExtension(projectReference.RelativePath);
                        if (existingDependencies.ContainsKey(key)) //Only add if already a declared dependency
                        {
                            directProjectDependencies.AddReference(projectReference);
                        }
                    }
                }
            }
        }


        private void AddReferencesForNamespace(
            string usingDirective,
            Dictionary<string, ProjectReference> existingProjectDependencies,
            Dictionary<string, PackageReference> existingPackageDependencies,
            List<ProjectReference> directProjectDependencies,
            List<PackageReference> directPackageDependencies)
        {
            var projectReferences = _namespaceLookup.GetProjectsForNamespace(usingDirective).ToArray();
            var packageReferences = _namespaceLookup.GetPackageReferencesForNamespace(usingDirective).ToArray();

            if (projectReferences.Length == 0 && packageReferences.Length == 0)
                throw new InvalidOperationException(
                    $"Could not determine which dependency owns using statement: {usingDirective}");

            foreach (var projectReference in projectReferences)
            {
                var key = System.IO.Path.GetFileNameWithoutExtension(projectReference.RelativePath);
                if (existingProjectDependencies.TryGetValue(key, out var directProjectDependency))
                {
                    directProjectDependencies.AddReference(directProjectDependency);
                }
            }

            foreach (var packageReference in packageReferences)
            {
                if (existingPackageDependencies.TryGetValue(packageReference.Name, out var directPackageDependency))
                {
                    directPackageDependencies.AddReference(directPackageDependency);
                }
            }
        }

        /// <summary>
        /// Currently assumes Subject under Test are linked like so:
        /// x.y.z.test where x.y.z is the subject, and the test extends the subjects namespace
        /// this enables test classes to avoid linking to subjects via using statements
        /// </summary>
        /// <param name="ns">namespace of test</param>
        /// <returns>namespace of subject</returns>
        private static string ProcessTestNamespaceForTarget(string ns)
        {
            var end = ns.IndexOf(".Tests", StringComparison.OrdinalIgnoreCase);

            return end == -1 ? string.Empty : ns[..end];
        }

        /// <summary>
        /// Some dependencies are required always for runtime,
        /// but are not linked at compile time.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="packages"></param>
        /// <param name="projects"></param>
        private void AddMandatoryNonDirectDependencies(Project project, List<PackageReference> packages, List<ProjectReference> projects)
        {
            foreach (var packageReference in _testDependencyProvider.ProvidePackageReferences(project))
            {
                packages.AddReference(packageReference);
            }

            foreach (var projectReference in _testDependencyProvider.ProvideProjectReferences(project))
            {
                projects.AddReference(projectReference);
            }
        }
    }

    public interface INamespaceLookup
    {
        public IEnumerable<ProjectReference> GetProjectsForNamespace(string nspace);
        public IEnumerable<PackageReference> GetPackageReferencesForNamespace(string nspace);
    }
}
