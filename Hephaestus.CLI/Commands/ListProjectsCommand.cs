using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Hephaestus.Core.Domain;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI
{
    public class ListProjectsCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var repo = RepositoryFactory.SelectAndSetRepo();

            var outputType = AnsiConsole.Prompt(new SelectionPrompt<OutputType>()
                .Title("Select an Output (Unknown for All)")
                .AddChoices(Enum.GetValues<OutputType>()));

            var framework = AnsiConsole.Prompt(new SelectionPrompt<Framework>()
                .Title("Select a Framework (Unknown for All)")
                .AddChoices(Enum.GetValues<Framework>()));

            var includeTests = AnsiConsole.Prompt(new ConfirmationPrompt("Include Tests?"));

            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots9)
                .Start("Loading...", ctx =>
                {
                    ctx.Status("Filtering...");
                    var projects = repo.Solutions
                        .SelectMany(x => x.Projects)
                        .DistinctBy(x => x.Metadata.ProjectPath)
                        .Where(x => outputType == OutputType.Unknown || x.Metadata.OutputType == outputType)
                        .Where(x => framework == Framework.Unknown || x.Metadata.Framework == framework)
                        .Where(x => includeTests || !x.Metadata.IsTestProject)
                        .OrderBy(x => x.Metadata.ProjectPath);
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