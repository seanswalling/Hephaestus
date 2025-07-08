using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Hephaestus.Core.Domain;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI.Commands
{
    public class ListNet48LeafNodesCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var repo = RepositoryFactory.SelectAndSetRepo();

            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots9)
                .Start("Loading...", ctx =>
                {
                    ctx.Status("Filtering...");

                    var allReferencedProjects = repo.Solutions
                        .SelectMany(x => x.Projects
                            .SelectMany(p => p.GetProjectReferenceAsAbsolutePaths()))
                            .Distinct()
                            .ToArray();

                    var projects = repo.Solutions
                        .SelectMany(x => x.Projects)
                        .DistinctBy(x => x.Metadata.ProjectPath)
                        .Where(x => x.Metadata.Framework == Framework.net48)
                        .OrderBy(x => x.Metadata.ProjectPath)
                        .Where(x => !allReferencedProjects.Contains(x.Metadata.ProjectPath));

                    ctx.Status("Writing...");

                    var output = projects.Select(x => new
                    {
                        x.Metadata.ProjectPath,
                        x.Metadata.OutputType,
                        x.Metadata.Framework
                    }).ToList();

                    var file = FileLocations.OutputCsvFile(this, DateTime.Now);
                    using var writer = new StreamWriter(file);
                    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                    csv.WriteRecords(output);

                    AnsiConsole.WriteLine($"File Written: {file}");
                });


            return 0;
        }
    }
}
