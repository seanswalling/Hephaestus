using System.Collections.Generic;

namespace Hephaestus.CLI
{
    public class PackageReferenceAndFramework
    {
        public required string Id { get; set; }
        public required List<PkgRefVersion> Versions { get; set; }
    }

    public class PkgRefVersion
    {
        public required string Id { get; set; }
        public required string Version { get; set; }
        public required string[] Frameworks { get; set; }
        public required bool? IsStandardCompliant { get; set; }
        public required List<PackageProjectLink> Projects { get; set; }
    }

    public class PackageProjectLink
    {
        public required string ProjectName { get; set; }
        public required string ProjectPath { get; set; }
    }
}