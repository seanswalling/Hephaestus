using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Hephaestus.Core.Domain;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI.Commands
{
    public partial class ListPackageDependencyFrameworksCommand : Command
    {
        private static JsonSerializerOptions _options = JsonSettingsFactory.Build();

        public override int Execute(CommandContext context)
        {
            var repo = RepositoryFactory.SelectAndSetRepo();

            var targets = Directory.EnumerateFiles(FileLocations.OutputFolder, $"{nameof(ListPackageReferencesToJsonCommand)}-*.json");
            var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
               .Title("Select a Package List")
               .AddChoices(targets));
            var outputFile = FileLocations.OutputJsonFile(this, DateTime.Now);

            var nugetRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget");
            var packageRoot = Path.Combine(nugetRoot, "packages");

            var packages = JsonSerializer.Deserialize<IEnumerable<PackageReference>>(File.ReadAllText(option), _options);

            if (packages == null)
            {
                return 0;
            }

            var results = PackageAnalyser.GetFrameworkForPackages(packages);

            File.WriteAllText(
                outputFile,
                JsonSerializer.Serialize(results.Value, _options)
            );

            results.Errors.Handle();

            return 0;
        }
    }
}