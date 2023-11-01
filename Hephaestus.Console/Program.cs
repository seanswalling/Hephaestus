using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hephaestus.Core;
using Hephaestus.Core.Building;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Setup;
using Hephaestus.Mercury;

namespace Hephaestus.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var provider = new RepositoryProvider();
            var adapter = new RepositoryProviderStoreAdapter(provider);
            var setup = new CoreHost(MercuryConstants.RepoLocation, MercuryConstants.RepoName, adapter);
            setup.Init();
            var repo = provider.FetchUpdates();
            var projects = repo.Projects.Where(x =>
                x.ProjectName.Equals("Mercury.InitialState", StringComparison.OrdinalIgnoreCase))
                .ToArray();
            foreach (var project in projects)
            {
                ProcessProject(project, repo);
                WriteSdkProject(project, true);
            }
            //ProcessSolutions(provider, repo);
        }


        static void ProcessProject(Project project, CodeRepository repo)
        {
            project.ProcessDirectDependencies(repo.NamespaceLookup, ResolverProvider.Provide().Concat(ResolverProvider.Default()).ToList());

            project.ProcessAddedProjectDependencies(new List<ProcessDirectDependencyResolver>
            {
                new (x =>
                        x.Contains("plugin", StringComparison.OrdinalIgnoreCase) &&
                        project.ProjectName.EndsWith("Database"),
                    x => new[]{ x })
            });
        }

        static void WriteSdkProject(Project project, bool writeEnabled)
        {
            var builder = new SdkProjectFormatBuilder(project);
            var result = builder.Build();

            if (writeEnabled)
            {
                var pkgConfigPath = Path.Combine(Path.GetDirectoryName(project.Path)!, "packages.config");
                File.WriteAllText(project.Path, result);
                File.Delete(pkgConfigPath);
            }
        }


        static void ProcessSolutions(RepositoryProvider provider, CodeRepository repo)
        {
            var slns = repo.Solutions;
            var result = slns.Select(s => new
            {
                solution = s,
                legacyCount = s.Projects.Select(repo.GetProject).Count(p => p.Format == ProjectFormat.Framework)
            })
            .OrderByDescending(x => x.legacyCount)
            .ToList();

            foreach (var res in result)
            {
                System.Console.WriteLine($@"{res.solution.Name}, {res.legacyCount}");
            }
        }

    }


}
