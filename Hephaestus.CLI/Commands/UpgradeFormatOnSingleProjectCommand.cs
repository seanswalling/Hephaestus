using System.IO;
using System.Linq;
using Hephaestus.Core.Application;
using Hephaestus.Core.Building;
using Hephaestus.Core.Domain;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI
{
    public class UpgradeFormatOnSingleProjectCommand : Command
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
            var projects = repo.Solutions
                .SelectMany(x => x.Projects)
                .Where(x => x.Metadata.Format == ProjectFormat.Framework);

            var projectOption = AnsiConsole.Prompt(new SelectionPrompt<Project>()
               .Title("Select a Project")
               .AddChoices(projects)
               .UseConverter((p) => p.Metadata.ProjectPath));

            var builder = new SdkProjectFileBuilder(projectOption);
            var result = builder.Build();

            var pkgConfigPath = Path.Combine(Path.GetDirectoryName(projectOption.Metadata.ProjectPath)!, "packages.config");
            File.WriteAllText(projectOption.Metadata.ProjectPath, result);
            File.Delete(pkgConfigPath);

            return 0;
        }
    }
}