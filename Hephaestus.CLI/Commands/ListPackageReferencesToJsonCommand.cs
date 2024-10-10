using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Hephaestus.Core.Application;
using Spectre.Console.Cli;

namespace Hephaestus.CLI
{
    public class ListPackageReferencesToJsonCommand : Command
    {
        private static readonly JsonSerializerOptions _options = JsonSettingsFactory.Build();

        public override int Execute(CommandContext context)
        {
            var app = new Application(FileLocations.ApplicationRoot);

            var repo = RepositoryFactory.SelectAndSetRepo();

            var distinctPackages = repo.Solutions
                        .SelectMany(x => x.Projects)
                        .DistinctBy(x => x.Metadata.ProjectPath)
                        .SelectMany(proj => proj.References.PackageReferences)
                        .DistinctBy((pr) => $"{pr.Id}-{pr.Version}")
                        .OrderBy(x => x.Id)
                        .ThenBy(x => x.Version);

            var content = JsonSerializer.Serialize(distinctPackages, _options);
            var path = FileLocations.OutputJsonFile(this, DateTime.Now);
            File.WriteAllText(path, content);

            return 0;
        }
    }
}