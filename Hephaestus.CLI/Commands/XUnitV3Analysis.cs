using System;
using System.Collections.Generic;
using System.Linq;
using Hephaestus.Core.Domain;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Hephaestus.CLI.Commands
{
    public class XUnitV3AnalysisCommand : Command
    {
        //I want to analyse and print a list of solutions by their upgrade status
        // - Not done = no test projects in solution are upgraded
        // - Partial = some test projects in solution are upgraded
        // - Done = all test projects in solution are done
        public override int Execute(CommandContext context)
        {
            var repo = RepositoryFactory.SelectAndSetRepo();
            var results = repo.Solutions.Select(sln => new
            {
                Solution = sln,
                Status = AnalyseStatus(sln)
            });


            var table = new Table
            {
                Title = new TableTitle($"Xunit Upgrade Status")
            };
            table.AddColumn("Solution");
            table.AddColumn("Status");

            foreach (var tuple in results.OrderBy(x => x.Solution.Name))
            {
                table.AddRow(
                    new Markup($"{tuple.Solution.Name}"),
                    new Markup($"{tuple.Status}"));
            }

            AnsiConsole.Write(table);
            return 0;
        }

        private static AnalysisResult AnalyseStatus(Solution solution)
        {
            var tests = GetTestProjects(solution);

            var results = tests.Select(IsUpgraded);

            if (results.All(x => x))
                return AnalysisResult.Done;
            if (results.All(x => !x))
                return AnalysisResult.NotDone;
            return AnalysisResult.Partial;
        }

        private static bool IsUpgraded(Project project)
        {
            var hasXunit3 = project.References.PackageReferences.Any(x => x.Id.Equals("xunit.v3", StringComparison.OrdinalIgnoreCase));
            var hasVunit2 = project.References.PackageReferences.Any(x => x.Id.Equals("xunit", StringComparison.OrdinalIgnoreCase));
            var isNet8 = project.Metadata.Framework == Framework.net80;

            return hasXunit3 && !hasVunit2 && isNet8;
        }

        private static IEnumerable<Project> GetTestProjects(Solution solution)
        {
            return solution.Projects.Where(p => p.Name.Contains(".tests", StringComparison.OrdinalIgnoreCase));
        }

        public enum AnalysisResult
        {
            NotDone, Partial, Done
        }


    }

    //XunitV3Upgrader
    //Steps
    // -> Remove old xunit dependency 
    // -> add new xunit v3 dependency
    // -> bump to net8
    // -> swap propertydata for member data
    // -> swap a bunch of usings when "Theory" detected
}
