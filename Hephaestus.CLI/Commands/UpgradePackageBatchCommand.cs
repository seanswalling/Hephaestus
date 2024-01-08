using System;
using System.IO;
using System.Linq;
using Hephaestus.Core.Building;
using Hephaestus.Core.Domain;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI
{
    public class UpgradePackageBatchCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var repo = RepositoryFactory.SelectAndSetRepo();

            var packageId = AnsiConsole.Ask<string>("Which Package to upgrade?");
            var oldVersion = AnsiConsole.Ask<string>("Which version to upgrade from?");
            var newVersion = AnsiConsole.Ask<string>("Which version to upgrade to?");

            var oldPackage = new PackageReference(packageId, oldVersion);
            var newPackage = new PackageReference(packageId, newVersion);

            var projects = repo.Solutions
               .SelectMany(x => x.Projects)
               .Where(x => x.References.PackageReferences.Contains(oldPackage));

            AnsiConsole.Progress()
               .Columns(
               [
                   new TaskDescriptionColumn(),
                   new ProgressBarColumn(),
               ])
               .Start(ctx =>
               {
                   var task = ctx.AddTask("Updating Projects");
                   task.MaxValue = projects.Count();

                   foreach (var project in projects)
                   {
                       project.References.Upgrade(oldPackage, newPackage);
                       var content = new SdkProjectFileBuilder(project).Build();
                       File.WriteAllText(project.Metadata.ProjectPath, content);
                       task.Increment(1);
                   }
               });

            return 0;
        }
    }
}