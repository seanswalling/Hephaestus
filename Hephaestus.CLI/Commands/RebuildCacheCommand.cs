using System.Linq;
using Hephaestus.Core.Application;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI
{
    public class RebuildCacheCommand : Command
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
                .Start("Rebuilding...", ctx =>
                {
                    app.RebuildCache();
                });

            return 0;
        }
    }
}