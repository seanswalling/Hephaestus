using System;
using System.Collections.Generic;
using System.Linq;
using Hephaestus.Core.Domain;
using Microsoft.Extensions.Logging;

namespace Hephaestus.CLI
{
    public class JoinerPrototype(ILogger logger)
    {
        //The idea here is to take a set of projects and packages and then determine the links from Project -> Package
        private readonly ILogger _logger = logger;

        public void Join(List<PkgRefVersion> packages, List<Project> projects)
        {
            foreach (var project in projects)
            {
                _logger.LogInformation($"Project: {project.Metadata.ProjectPath}");
                //Our project has package dependencies
                if (project.References.PackageReferences.Count != 0)
                {
                    foreach (var projectPackage in project.References.PackageReferences)
                    {
                        _logger.LogInformation($"Project-Package: {projectPackage.Id} - {projectPackage.Version}");
                        try
                        {
                            var package = GetPackage(packages, projectPackage.Id, projectPackage.Version);
                            _logger.LogInformation($"Matched with: {package.Id} - {package.Version}");
                            package.Projects.Add(new PackageProjectLink { ProjectName = project.Name, ProjectPath = project.Metadata.ProjectPath });
                        }
                        catch (InvalidOperationException ex)
                        {
                            throw new InvalidOperationException($"Error finding package for Project: {project.Metadata.ProjectPath}", ex);
                        }
                    }
                }
            }
        }

        private PkgRefVersion GetPackage(List<PkgRefVersion> packageReferences, string id, string version)
        {
            var packages = packageReferences.Where(x => x.Id == id && x.Version == version).ToList();

            if (packages.Count == 0)
            {
                throw new InvalidOperationException($"Package not found {id}, {version}");
            }

            if (packages.Count > 1)
            {
                throw new InvalidOperationException($"Too many Packages found, Package: {id}, {version}");
            }

            return packages.Single();
        }

    }
}
