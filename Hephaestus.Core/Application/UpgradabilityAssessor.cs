using System;
using System.Collections.Generic;
using System.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Application
{
    public static class UpgradabilityAssessor
    {
        private static readonly Framework[] OldFrameworks =
        [
            Framework.Unknown,
            Framework.net11,
            Framework.net20,
            Framework.net35,
            Framework.net40,
            Framework.net403,
            Framework.net45,
            Framework.net451,
            Framework.net452,
            Framework.net46,
            Framework.net461,
            Framework.net462,
            Framework.net47,
            Framework.net471,
            Framework.net472,
            Framework.net48
        ];

        private static readonly string[] IncompatiblePackages = [];

        public static SystemUpgradeResult AssessSystemUpgradability(Dictionary<string, Project> system)
        {
            var systemUpgradeResult = new SystemUpgradeResult();

            foreach (var project in system)
            {
                systemUpgradeResult.ProjectUpgradeResults.Add(project.Key, null);
            }

            foreach (var project in system)
            {
                AssessProjectUpgradabilityRecursively(project.Value, system, systemUpgradeResult);
            }

            systemUpgradeResult.RefreshSummary();

            return systemUpgradeResult;
        }

        public static void AssessProjectUpgradabilityRecursively(Project project, Dictionary<string, Project> system, SystemUpgradeResult currentResultStatus)
        {
            var currentResult = currentResultStatus.ProjectUpgradeResults[project.Metadata.ProjectPath];

            //We've already managed to assess this project
            if (currentResult != null)
                return;

            var result = new ProjectUpgradeResult();

            if (OldFrameworks.Contains(project.Metadata.Framework))
            {
                //blockers for GAC references
                foreach (var gac in project.References.GacReferences)
                {
                    result.Blockers.Add($"Blocked by GacReferences {gac.Id}");
                }

                //Blockers for Package References
                foreach (var pkgRef in project.References.PackageReferences)
                {
                    if (IncompatiblePackages.Contains(pkgRef.Id))
                        result.Blockers.Add($"Blocked by Package {pkgRef.Id}");
                }

                //blockers based on our poroject references
                foreach (var projRef in project.GetProjectReferenceAsAbsolutePaths())
                {
                    //Our dependency is net48
                    if (system[projRef].Metadata.Framework == Framework.net48)
                    {
                        result.Blockers.Add($"Blocked by net48 ref: {projRef}");
                    }

                    //We don't have results for one of our dependencies
                    if (currentResultStatus.ProjectUpgradeResults[projRef] == null)
                    {
                        //Recurse into our dependency
                        AssessProjectUpgradabilityRecursively(system[projRef], system, currentResultStatus);
                    }

                    var dependencyResult = currentResultStatus.ProjectUpgradeResults[projRef] ??
                        throw new NullReferenceException("This should not be null, a bug has occured");

                    //Should be populated now
                    if (dependencyResult.Blockers.Count != 0)
                    {
                        result.Blockers.Add($"Blocked by Blocker in ref: {projRef}");
                    }
                }
            }
            else
            {
                result.IsUpgraded = true;
            }

            //Assign our results
            currentResultStatus.ProjectUpgradeResults[project.Metadata.ProjectPath] = result;
        }
    }
}
