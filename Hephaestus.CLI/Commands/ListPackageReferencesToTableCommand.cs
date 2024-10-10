using System;
using System.Linq;
using Hephaestus.Core.Application;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI
{
    public class ListPackageReferencesToTableCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var app = new Application(FileLocations.ApplicationRoot);

            var repos = app.KnownRepositories.Select(x => x.Name);

            var option = AnsiConsole.Prompt(new SelectionPrompt<KnownRepository>()
               .Title("Select a Repository")
               .AddChoices(app.KnownRepositories)
               .UseConverter((kr) => kr.Name));

            app.SetRepository(option);

            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots9)
                .Start("Loading...", ctx =>
                {
                    app.LoadRepository();
                });
            var repo = app.Parse();

            var groupedPackages = repo.Solutions
                        .SelectMany(x => x.Projects)
                        .DistinctBy(x => x.Metadata.ProjectPath)
                        .SelectMany(proj => proj.References.PackageReferences)
                        .GroupBy((pr) => $"{pr.Id}-{pr.Version}");


            var table = new Table
            {
                Title = new TableTitle("Packages")
            };
            table.AddColumn("Package Id");
            table.AddColumn("Version");
            table.AddColumn("Count");
            foreach (var group in groupedPackages.OrderBy(x => x.Key))
            {
                var package = group.ToList().First();
                table.AddRow(new Markup($"{package.Id}"), new Markup($"{package.Version}"), new Markup($"{group.Count()}"));
            }
            AnsiConsole.Write(table);
            return 0;
        }
    }
}