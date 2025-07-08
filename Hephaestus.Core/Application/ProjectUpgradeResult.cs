using System.Collections.Generic;
using System.Linq;

namespace Hephaestus.Core.Application
{
    public class ProjectUpgradeResult
    {
        public List<string> Blockers;
        public bool IsUpgradable => Blockers.Count == 0 && !IsUpgraded;
        public bool IsBlocked => Blockers.Count != 0 && !IsUpgraded;
        public bool IsUpgraded;

        public ProjectUpgradeResult()
        {
            Blockers = [];
            IsUpgraded = false;
        }
    }

    public class SystemUpgradeResult
    {
        public SystemUpgradeSummary Summary;
        public Dictionary<string, ProjectUpgradeResult?> ProjectUpgradeResults;

        public SystemUpgradeResult()
        {
            Summary = new SystemUpgradeSummary
            {
                UpgradeableProjects = 0,
                UpgradedProjects = 0,
                BlockedProjects = 0
            };
            ProjectUpgradeResults = [];
        }

        public void RefreshSummary()
        {
            Summary.UpgradeableProjects = ProjectUpgradeResults.Where(x => x.Value!.IsUpgradable).Count();
            Summary.UpgradedProjects = ProjectUpgradeResults.Where(x => x.Value!.IsUpgraded).Count();
            Summary.BlockedProjects = ProjectUpgradeResults.Where(x => x.Value!.IsBlocked).Count();
        }
    }

    public class SystemUpgradeSummary
    {
        public int UpgradeableProjects { get; set; }
        public int UpgradedProjects { get; set; }
        public int BlockedProjects { get; set; }
    }
}
