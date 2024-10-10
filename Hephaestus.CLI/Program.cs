using System;
using Hephaestus.CLI.Commands;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI
{
    internal partial class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();

            FileLocations.EnsureFileStructures();
            FileLocations.EnsureApplicationFileStructures();

            app.Configure(config =>
            {
                app.Configure(config =>
                {
                    config.PropagateExceptions();
                });

                config.AddCommand<InitialiseCommand>("init");
                config.AddCommand<AnalysePackagesCommand>("analyse");

                config.AddBranch("list", list =>
                {
                    list.AddCommand<ListProjectsCommand>("projects");
                    list.AddBranch("packages", packages =>
                    {
                        packages.AddCommand<ListPackageDependencyCommand>("dependencies");
                        packages.AddCommand<ListPackageDependencyFrameworksCommand>("frameworks");
                        packages.AddBranch("references", references =>
                        {
                            references.AddCommand<ListPackageReferencesToTableCommand>("table");
                            references.AddCommand<ListPackageReferencesToJsonCommand>("json");
                            references.AddCommand<ListPackageReferencesToCsvCommand>("csv");
                        });
                    });
                });

                config.AddCommand<RebuildCacheCommand>("rebuild");

                config.AddBranch("upgrade", upgrade =>
                {
                    upgrade.AddBranch("batch", batch =>
                    {
                        batch.AddCommand<UpgradeFrameworkBatchCommand>("framework");
                        batch.AddCommand<UpgradePackageBatchCommand>("package");
                    });
                    upgrade.AddBranch("single", single =>
                    {
                        single.AddCommand<UpgradeFormatOnSingleProjectCommand>("format");
                    });
                });

                config.AddBranch("remove", remove =>
                {
                    remove.AddBranch("batch", batch =>
                    {
                        batch.AddCommand<RemovePackageBatchCommand>("package");
                    });
                });
            });

            try
            {
                return app.Run(args);
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                return -99;
            }
        }
    }
}