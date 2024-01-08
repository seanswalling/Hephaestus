using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Hephaestus.Core.Application;
using Spectre.Console.Cli;

namespace Hephaestus.CLI
{
    public class ListPackageReferencesToCsvCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var app = new Application();

            var repo = RepositoryFactory.SelectAndSetRepo();

            var distinctPackages = repo.Solutions
                        .SelectMany(x => x.Projects)
                        .DistinctBy(x => x.Metadata.ProjectPath)
                        .SelectMany(proj => proj.References.PackageReferences)
                        .Select(x => new DependencyCsvRow { Id = x.Id, Version = x.Version })
                        .DistinctBy((pr) => $"{pr.Id}-{pr.Version}")
                        .OrderBy(x => x.Id)
                        .ThenBy(x => x.Version)
                        .ToArray();

            var path = FileLocations.OutputCsvFile(this, DateTime.Now);

            using var writer = new StreamWriter(path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(distinctPackages);

            return 0;
        }

        public class DependencyCsvRow
        {
            public required string Id { get; set; }
            public required string Version { get; set; }
        }
    }
}