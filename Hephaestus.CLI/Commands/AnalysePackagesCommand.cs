using System.IO;
using System.Linq;
using System.Text.Json;
using Spectre.Console.Cli;

namespace Hephaestus.CLI.Commands
{
    public class AnalysePackagesCommand : Command
    {
        private static JsonSerializerOptions _options = JsonSettingsFactory.Build();

        public override int Execute(CommandContext context)
        {
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

            var results = analysisResult.Value;

            var nonStandardPackages = results.Values.SelectMany(x => x.Versions).Where(x => !x.IsStandardCompliant);
            var standardPackages = results.Values.SelectMany(x => x.Versions).Where(x => !x.IsStandardCompliant);

            var standardOutput = Path.Combine(FileLocations.OutputFolder, "standard-compliant-packages.json");
            var nonStandardOutput = Path.Combine(FileLocations.OutputFolder, "non-standard-compliant-packages.json");

            File.WriteAllText(
                standardOutput,
                JsonSerializer.Serialize(standardPackages, _options)
            );
            File.WriteAllText(
                nonStandardOutput,
                JsonSerializer.Serialize(nonStandardPackages, _options)
            );

            return 0;
        }
    }
}
