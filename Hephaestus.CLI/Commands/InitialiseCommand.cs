using System;
using System.Linq;
using Hephaestus.Core.Application;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI.Commands
{
    public class InitialiseCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var app = new Application(FileLocations.ApplicationRoot);

            var repos = app.KnownRepositories;
            //enter repo name and path
            var name = AnsiConsole.Prompt(new TextPrompt<string>("Name of Repository?"));

            if (repos.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                AnsiConsole.WriteLine($"A Repository with that name is already Initialised");
                return 1;
            }

            var path = AnsiConsole.Prompt(new TextPrompt<string>("Path of Repository?"));

            if (repos.Any(x => x.Path.Equals(path, StringComparison.OrdinalIgnoreCase)))
            {
                AnsiConsole.WriteLine($"A Repository with that path is already Initialised");
                return 1;
            }

            FileLocations.EnsureRepositoryFolder(name);

            app.AddRepository(name, path);

            app.SetRepository(new KnownRepository(name, path));

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
