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

            var fmwk = AnsiConsole.Prompt(new SelectionPrompt<Framework>()
                .Title("Select a Framework")
                .AddChoices(Enum.GetValues<Framework>()));

            var outputType = AnsiConsole.Prompt(new SelectionPrompt<OutputType>()
                .Title("Select an Output")
                .AddChoices(Enum.GetValues<OutputType>()));

            var subselection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Tests Libs or Normal Libs")
                .AddChoices(["Tests", "Normal"]));

            var projectsWithOutput = repo.Solutions
                .SelectMany(x => x.Projects)
                .Where(x => x.Metadata.OutputType == outputType);

            var projects = subselection == "Tests" ?
                projectsWithOutput.Where(x => x.Metadata?.ProjectPath?.Contains(".Tests.") ?? false) :
                projectsWithOutput.Where(x => !x.Metadata?.ProjectPath?.Contains(".Tests.") ?? true);

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