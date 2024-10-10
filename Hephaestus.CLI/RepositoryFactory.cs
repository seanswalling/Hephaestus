using System.Linq;
using Hephaestus.Core.Application;
using Hephaestus.Core.Domain;
using Spectre.Console;

namespace Hephaestus.CLI
{
    public static class RepositoryFactory
    {
        public static CodeRepository SelectAndSetRepo()
        {
            var app = new Application(FileLocations.ApplicationRoot);

            var repos = app.KnownRepositories.Select(x => x.Name);

            var option = AnsiConsole.Prompt(new SelectionPrompt<KnownRepository>()
               .Title("Select a Repository")
               .AddChoices(app.KnownRepositories)
               .UseConverter((kr) => kr.Name));

            FileLocations.EnsureRepositoryFolder(option.Name);

            app.SetRepository(option);

            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots9)
                .Start("Loading...", ctx =>
                {
                    app.LoadRepository();
                });
            return app.Parse();
        }
    }
}
