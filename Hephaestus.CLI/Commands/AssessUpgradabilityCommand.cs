using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Hephaestus.Core.Application;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI.Commands
{
    public class AssessUpgradabilityCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var repo = RepositoryFactory.SelectAndSetRepo();

            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots9)
                .Start("Loading...", ctx =>
                {
                    ctx.Status("Filtering...");

                    var system = repo.Solutions.SelectMany(x => x.Projects)
                        .DistinctBy(p => p.Metadata.ProjectPath)
                        .ToDictionary(p => p.Metadata.ProjectPath, p => p);

                    var systemResult = UpgradabilityAssessor.AssessSystemUpgradability(system);

                    ctx.Status("Writing...");

                    var rawResults = systemResult.ProjectUpgradeResults.Select(x => new
                    {
                        Project = x.Key,
                        x.Value!.IsUpgraded,
                        x.Value!.IsUpgradable,
                        x.Value!.IsBlocked,
                        x.Value!.Blockers,
                    })
                    .ToList();

                    var output = new
                    {
                        systemResult.Summary,
                        Upgraded = rawResults.Where(x => x.IsUpgraded).Select(x => x.Project).OrderBy(x => x).ToList(),
                        Upgradable = rawResults.Where(x => x.IsUpgradable).Select(x => x.Project).OrderBy(x => x).ToList(),
                        Blocked = rawResults.Where(x => x.IsBlocked).Select(x => x.Project).OrderBy(x => x).ToList(),
                        Results = rawResults.OrderBy(x => x.Project),
                    };

                    var file = FileLocations.OutputJsonFile(this, DateTime.Now);
                    JsonSerializerOptions options = new()
                    {
                        WriteIndented = true,
                    };
                    var json = JsonSerializer.Serialize(output, options: options);
                    File.WriteAllText(file, json);
                    AnsiConsole.WriteLine($"File Written: {file}");
                });


            return 0;
        }


    }
}
