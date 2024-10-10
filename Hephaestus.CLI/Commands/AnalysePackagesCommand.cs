using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

namespace Hephaestus.CLI.Commands
{
    public class AnalysePackagesCommand : Command
    {
        private static readonly JsonSerializerOptions _options = JsonSettingsFactory.Build();

        public override int Execute(CommandContext context)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .SetMinimumLevel(LogLevel.Warning)
                    .AddConsole();
            });

            var repo = RepositoryFactory.SelectAndSetRepo();

            var distinctPackages = repo.Solutions
                .SelectMany(x => x.Projects)
                .DistinctBy(x => x.Metadata.ProjectPath)
                .SelectMany(proj => proj.References.PackageReferences)
                .DistinctBy((pr) => $"{pr.Id}-{pr.Version}")
                .OrderBy(x => x.Id)
                .ThenBy(x => x.Version);

            var analysisResult = PackageAnalyser.GetFrameworkForPackages(distinctPackages);

            analysisResult.Errors.Handle();

            File.WriteAllText(
                Path.Combine(FileLocations.OutputFolder, "analysis-test.json"),
                JsonSerializer.Serialize(analysisResult, _options)
            );

            var results = analysisResult.Value;

            new JoinerPrototype(loggerFactory.CreateLogger("Joiner")).Join(
                results
                    .Values
                    .SelectMany(x => x.Versions)
                    .ToList(),
                repo
                    .Solutions
                    .SelectMany(x => x.Projects)
                    .DistinctBy(x => x.Metadata.ProjectPath)
                    .ToList()
            );



            //var nonStandardPackages = results.Values.SelectMany(x => x.Versions).Where(x => !x.IsStandardCompliant);
            //var standardPackages = results.Values.SelectMany(x => x.Versions).Where(x => !x.IsStandardCompliant);

            //var standardOutput = Path.Combine(FileLocations.OutputFolder, "standard-compliant-packages.json");
            //var nonStandardOutput = Path.Combine(FileLocations.OutputFolder, "non-standard-compliant-packages.json");

            File.WriteAllText(
                Path.Combine(FileLocations.OutputFolder, "test-joiner-out.json"),
                JsonSerializer.Serialize(results, _options)
            );


            return 0;
        }
    }
}
