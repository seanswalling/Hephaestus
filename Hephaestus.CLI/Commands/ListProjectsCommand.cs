using System.Collections.Generic;
using System.Linq;
using Hephaestus.Core.Application;
using Hephaestus.Core.Domain;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI
{
    public class ListProjectsCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var app = new Application(FileLocations.ApplicationRoot);
            IEnumerable<Project> projects = new List<Project>();

            var repos = app.KnownRepositories.Select(x => x.Name);

            var option = AnsiConsole.Prompt(new SelectionPrompt<KnownRepository>()
               .Title("Select a Repository")
               .AddChoices(app.KnownRepositories)
               .UseConverter((kr) => kr.Name));
            app.SetRepository(option);

            AnsiConsole.Clear();
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots9)
                .Start("Loading...", ctx =>
                {
                    app.LoadRepository();
                    ctx.Status("Parsing...");
                    var repo = app.Parse();
                    ctx.Status("Filtering...");
                    projects = repo.Solutions
                        .SelectMany(x => x.Projects)
                        .Where(x => x.Metadata.Format == ProjectFormat.Framework);
                });

            var table = new Table
            {
                Title = new TableTitle("Projects")
            };
            table.AddColumn("Project");
            table.AddColumn("Framework Status");
            foreach (var project in projects.OrderBy(x => x.Name))
            {
                var mkup = project.Metadata.Format == ProjectFormat.Sdk ?
                    "green" : "darkorange3";

                table.AddRow(new Markup($"[{mkup}]{project.Name}[/]"), new Markup($"[{mkup}]{project.Metadata.Format}[/]"));
            }
            AnsiConsole.Write(table);
            return 0;
        }
    }
}