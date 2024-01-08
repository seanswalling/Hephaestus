using System;
using System.IO;
using System.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Version1.Building;
using Hephaestus.Core.Version1.Domain;

namespace Hephaestus.Core.Version1.Application
{
    internal class Misc
    {
        static void WriteSdkProject(ProjectV1 project, bool writeEnabled)
        {
            var builder = new SdkProjectV1FileBuilder(project);
            var result = builder.Build();

            if (writeEnabled)
            {
                var pkgConfigPath = Path.Combine(Path.GetDirectoryName(project.Path)!, "packages.config");
                File.WriteAllText(project.Path, result);
                File.Delete(pkgConfigPath);
            }
        }


        static void ProcessSolutions(RepositoryV1Provider provider, CodeRepositoryV1 repo)
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
