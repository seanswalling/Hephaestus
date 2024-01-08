using System.Linq;
using Spectre.Console;

namespace Hephaestus.Console
{
    public class MainView
    {
        MainModel _model;
        MainController _controller;

        public MainView(MainModel model, MainController controller)
        {
            _model = model;
            _controller = controller;
        }

        public void Title()
        {
            AnsiConsole.MarkupLine($"Welcome to [bold green]Hephaestus[/]");
            SelectRepository();
        }

        public void MainMenu()
        {
            var options = new[]
            {
                "Parse",
                "Rebuild Cache",
                "Back"
            };

            var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
               .Title($"Main Menu -- {_model.Name} -- {_model.CacheState}")
               .AddChoices(options));

            switch (option)
            {
                case "Parse": Parse(); break;
                case "Rebuild Cache": RebuildCache(); break;
                case "Back": Title(); break;
            }
        }

        public void Parse()
        {
            AnsiConsole.Clear();
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots9)
                .Start("Parsing...", ctx =>
                {
                    _controller.Parse();
                    ctx.Refresh();
                });
            AnsiConsole.Clear();

            var table = new Table();
            table.AddColumn("Solutions");
            foreach (var sln in _model.Solutions)
            {
                table.AddRow(sln);
            }
            AnsiConsole.Write(table);
            if (AnsiConsole.Confirm("Continue?"))
            {
                AnsiConsole.Clear();
                MainMenu();
            }
            else
            {
                System.Environment.Exit(0);
            }
        }

        public void RebuildCache()
        {
            AnsiConsole.Clear();
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots9)
                .Start("Rebuilding...", ctx =>
                {
                    _controller.RebuildCache();
                    ctx.Refresh();
                });
            AnsiConsole.Clear();
            MainMenu();
        }

        public void SelectRepository()
        {
            var repos = _model.KnownRepositories.Select(x => x.Name);
            var options = repos.Append("Exit").Append("Add New");

            var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
               .Title("Select a Repository")
               .AddChoices(options));

            switch (option)
            {
                case "Exit":
                    System.Environment.Exit(0);
                    break;
                case "Add New":
                    AddRepository();
                    break;
                default:
                    _controller.SetRepository(_model.KnownRepositories.Single(x => x.Name == option));
                    MainMenu();
                    break;
            }
        }

        public void AddRepository()
        {
            var name = AnsiConsole.Ask<string>("Name?: ");
            var location = AnsiConsole.Ask<string>("Full path?: ");

            _controller.AddRepository(name, location);
            SelectRepository();
        }
    }
}
