using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hephaestus.Core.Domain;
using Spectre.Console;

namespace Hephaestus.CLI
{
    public static class PackageAnalyser
    {
        public static Result<SortedDictionary<string, PackageReferenceAndFramework>> GetFrameworkForPackages(IEnumerable<PackageReference> packages)
        {
            var nugetRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget");
            var packageRoot = Path.Combine(nugetRoot, "packages");

            SortedDictionary<string, PackageReferenceAndFramework> results = [];
            List<string> errors = [];

            foreach (var package in packages)
            {
                var packageLocation = Path.Combine(packageRoot, package.Id);
                var versionLocation = Path.Combine(packageLocation, package.Version);

                if (!Directory.Exists(versionLocation))
                {
                    errors.Add($"ERROR: Could not find {versionLocation}");
                    //continue;
                }

                var lib = Path.Combine(versionLocation, "lib");
                var build = Path.Combine(versionLocation, "build");

                List<string> frameworks = [];

                if (!Directory.Exists(lib) && !Directory.Exists(build))
                {
                    errors.Add($"ERROR: Could not find {lib} or {build}");
                    //continue;
                }
                else
                {
                    frameworks.AddRange((Directory.Exists(lib) ?
                        Directory.EnumerateDirectories(lib, "*") :
                        Directory.EnumerateDirectories(build, "*"))
                        .Select(x => x.Split(Path.DirectorySeparatorChar).Last()));
                }

                bool? isStandardCompliant =
                    frameworks.Count != 0 ?
                        frameworks.Any(x =>
                        {
                            return x.Contains("netstandard");
                        })
                        : null;

                if (results.TryGetValue(package.Id, out PackageReferenceAndFramework? value))
                {
                    value.Versions.Add(new PkgRefVersion
                    {
                        Id = package.Id,
                        Frameworks = frameworks.ToArray(),
                        Version = package.Version,
                        IsStandardCompliant = isStandardCompliant,
                        Projects = []
                    });
                }
                else
                {
                    results.Add(package.Id, new PackageReferenceAndFramework
                    {
                        Id = package.Id,
                        Versions =
                        [
                            new PkgRefVersion
                            {
                                Id = package.Id,
                                Frameworks = frameworks.ToArray(),
                                Version = package.Version,
                                IsStandardCompliant = isStandardCompliant,
                                Projects = []
                            }
                        ],
                    });
                }
            }

            return new(results, errors);
        }

        public static Result<(IEnumerable<PkgRefVersion> standard, IEnumerable<PkgRefVersion> nonstandard)> SplitStandardAndNonStandardPackages(CodeRepository repo)
        {
            var distinctPackages = repo.Solutions
                .SelectMany(x => x.Projects)
                .DistinctBy(x => x.Metadata.ProjectPath)
                .SelectMany(proj => proj.References.PackageReferences)
                .DistinctBy((pr) => $"{pr.Id}-{pr.Version}")
                .OrderBy(x => x.Id)
                .ThenBy(x => x.Version);

            var analysisResult = GetFrameworkForPackages(distinctPackages);

            var results = analysisResult.Value;

            //Figure out better option to the standard compliance.
            var nonStandardPackages = results.Values.SelectMany(x => x.Versions).Where(x => !x.IsStandardCompliant.Equals("Yes"));
            var standardPackages = results.Values.SelectMany(x => x.Versions).Where(x => !x.IsStandardCompliant.Equals("Yes"));

            return new((standardPackages, nonStandardPackages), analysisResult.Errors);

            //var standardOutput = Path.Combine(FileLocations.OutputFolder, "standard-compliant-packages.json");
            //var nonStandardOutput = Path.Combine(FileLocations.OutputFolder, "non-standard-compliant-packages.json");
        }
    }
}