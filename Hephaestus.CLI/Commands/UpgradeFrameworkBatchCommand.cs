using System;
using System.IO;
using System.Linq;
using Hephaestus.Core.Building;
using Hephaestus.Core.Domain;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI
{
    public class UpgradeFrameworkBatchCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var repo = RepositoryFactory.SelectAndSetRepo();

            var inputOptions = Directory.EnumerateFiles(FileLocations.InputFolder);

            if (!inputOptions.Any())
                AnsiConsole.WriteLine($"No Input(s) avaliable at {FileLocations.InputFolder}");

            var fmwk = AnsiConsole.Prompt(new SelectionPrompt<Framework>()
                .Title("Select a Framework")
                .AddChoices(Enum.GetValues<Framework>()));

            var input = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Select an Input File")
                .AddChoices(inputOptions));

            var inputLines = File.ReadAllLines(input); //expect line delimited file list

            var projects = repo.Solutions
                .SelectMany(x => x.Projects)
                .DistinctBy(x => x.Metadata.ProjectPath)
                .Where(x => inputLines.Contains(x.Metadata.ProjectPath, StringComparer.OrdinalIgnoreCase))
                .ToList();

            AnsiConsole.Progress()
                .Columns(
                [
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                ])
                .Start(ctx =>
                {
                    var task = ctx.AddTask("Updating Projects");
                    task.MaxValue = projects.Count();

                    foreach (var project in projects)
                    {
                        project.Metadata.Framework = fmwk;
                        var content = new SdkProjectFileBuilder(project).Build();
                        File.WriteAllText(project.Metadata.ProjectPath, content);
                        task.Increment(1);
                    }
                });

            return 0;
        }
    }
}