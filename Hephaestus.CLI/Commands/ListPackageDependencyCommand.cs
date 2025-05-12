using System.Linq;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI.Commands
{
    public class ListPackageDependencyCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var repo = RepositoryFactory.SelectAndSetRepo();
            var package = AnsiConsole.Ask<string>("PackageId?");
            var usages = repo.Solutions
                .SelectMany(x => x.Projects)
                .Where(p => p.References.PackageReferences.Any(pr => pr.Id == package))
                .Select(x => new { project = x, packageVersion = x.References.PackageReferences.Single(pr => pr.Id == package).Version })
                .DistinctBy(x => x.project.Metadata.ProjectPath);

            var table = new Table
            {
                Title = new TableTitle($"Package usages for {package}")
            };
            table.AddColumn("Project");
            table.AddColumn("Output Type");
            table.AddColumn("Version");
            foreach (var tuple in usages.OrderBy(x => x.project.Metadata.ProjectPath).ThenBy(x => x.packageVersion))
            {
                table.AddRow(
                    new Markup($"{tuple.project.Metadata.ProjectPath}"),
                    new Markup($"{tuple.project.Metadata.OutputType}"),
                    new Markup($"{tuple.packageVersion}"));
            }
            AnsiConsole.Write(table);
            return 0;
        }
    }
}