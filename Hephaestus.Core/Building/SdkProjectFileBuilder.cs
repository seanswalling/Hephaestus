using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;

namespace Hephaestus.Core.Building
{
    public class SdkProjectFileBuilder
    {
        private readonly Project _project;

        public SdkProjectFileBuilder(Project project)
        {
            _project = project;
        }

        public string Build()
        {
            var sb = new StringBuilder();
            sb.Append(MetadataBlock(_project.Metadata));
            sb.AppendLine();
            //sb.Append(PrivateAssetsBlock());
            //sb.AppendLine();
            sb.Append(WarningsBlock(_project.Metadata.Warnings));
            sb.AppendLine();
            sb.Append(EmbeddedFileBlock(
                _project.EmbeddedResources
                    .Where(x => x.FilePath.Contains(".sql") || x.FilePath.Contains(".docx"))
                    .ToArray()
            ));
            sb.AppendLine();
            sb.Append(ProjectReferenceBlock(_project.References.ProjectReferences));
            sb.AppendLine();
            sb.Append(PackageReferenceBlock(_project.References.PackageReferences));
            sb.AppendLine();
            sb.Append(GacReferenceBlock(_project.References.GacReferences));
            sb.AppendLine();
            sb.Append("</Project>");
            return sb.ToString();
        }


        public static string MetadataBlock(ProjectMetadata metadata)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk\">");
            sb.AppendLine(StartPropertyGroup());
            sb.AppendLine(TargetFramework(new TfmTranslator().Translate(metadata.Framework)));

            if (metadata.Title != null)
                sb.AppendLine(Title(metadata.Title));

            sb.AppendLine(OutputType(new OutputTypeTranslator().Translate(metadata.OutputType)));
            sb.AppendLine(Authors());
            sb.AppendLine(Copyright());

            if (metadata.AssemblyName != null)
                sb.AppendLine(AssemblyName(metadata.AssemblyName));

            if (metadata.RootNamespace != null)
                sb.AppendLine(RootNamespace(metadata.RootNamespace));

            sb.AppendLine(GenerateAssemblyInfo());
            sb.AppendLine(IsPackable());
            sb.AppendLine(AppendTargetFrameworkToOutputPath());
            sb.AppendLine(EndPropertyGroup());
            return sb.ToString();
        }

        private static string PrivateAssetsBlock()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<ItemDefinitionGroup>".Indent());
            sb.AppendLine("<PackageReference>".DoubleIndent());
            sb.AppendLine("<PrivateAssets>compile;contentFiles;build;buildMultitargeting;buildTransitive;analyzers;native</PrivateAssets>".TripleIndent());
            sb.AppendLine("</PackageReference>".DoubleIndent());
            sb.AppendLine("</ItemDefinitionGroup>".Indent());
            return sb.ToString();
        }

        private static string WarningsBlock(Warnings warnings)
        {
            if (warnings.WarningsNotAsErrors.Count() == 0 && warnings.WarningLevel == null && warnings.TreatWarningsAsErrors == null)
                return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine(StartPropertyGroup());

            if (!string.IsNullOrWhiteSpace(warnings.WarningLevel))
            {
                sb.AppendLine($"<WarningLevel>{warnings.WarningLevel}</WarningLevel>".DoubleIndent());
            }

            if (warnings.TreatWarningsAsErrors != null)
            {
                sb.AppendLine($"<TreatWarningsAsErrors>{warnings.TreatWarningsAsErrors}</TreatWarningsAsErrors>".DoubleIndent());
            }

            if (warnings.WarningsNotAsErrors.Count() > 0)//612,618,1591,NU1901,NU1902,NU1903,NU1904 ideally
            {
                sb.AppendLine($"<WarningsNotAsErrors>{string.Join(",", warnings.WarningsNotAsErrors)}</WarningsNotAsErrors>".DoubleIndent());
            }

            sb.AppendLine(EndPropertyGroup());
            return sb.ToString();
        }

        private static string EmbeddedFileBlock(EmbeddedResource[] scripts)
        {
            if (scripts.Length == 0) return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine(StartItemGroup());
            foreach (var script in scripts)
            {
                if (script.LinkedPath == null)
                {
                    sb.AppendLine(RemoveNone(script.FilePath));
                }
            }
            sb.AppendLine(EndItemGroup());
            sb.AppendLine();

            sb.AppendLine(StartItemGroup());
            foreach (var script in scripts)
            {
                sb.AppendLine(AddEmbeddedResource(script.FilePath, script.LinkedPath));
            }
            sb.AppendLine(EndItemGroup());
            return sb.ToString();
        }

        private static string ProjectReferenceBlock(IEnumerable<ProjectReference> projectReferences)
        {
            if (projectReferences.Count() == 0) return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine(StartItemGroup());
            foreach (var projectReference in projectReferences)
            {
                sb.AppendLine(ProjectReference(projectReference));
            }
            sb.AppendLine(EndItemGroup());
            return sb.ToString();
        }

        private static string PackageReferenceBlock(IEnumerable<PackageReference> packageReferences)
        {
            if (packageReferences.Count() == 0) return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine(StartItemGroup());
            foreach (var packageReference in packageReferences)
            {
                if (packageReference.Id.Equals("xunit.runner.visualstudio", StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine(PackageReferenceNoTerminate(packageReference));
                    sb.AppendLine(PrivateAsset(["all"]));
                    sb.AppendLine(IncludeAssets(["runtime", "build", "native", "contentfiles", "analyzers", "buildtransitive"]));
                    sb.AppendLine(PackageReferenceTerminate());
                }
                else
                {
                    sb.AppendLine(PackageReference(packageReference));
                }
            }
            sb.AppendLine(EndItemGroup());
            return sb.ToString();
        }

        private static string GacReferenceBlock(IEnumerable<GacReference> gacReferences)
        {
            if (gacReferences.Count() == 0) return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine(StartItemGroup());
            foreach (var gacReference in gacReferences)
            {
                sb.AppendLine(GacReference(gacReference));
            }
            sb.AppendLine(EndItemGroup());
            return sb.ToString();
        }

        private static string EmptyFoldersBlock(IEnumerable<string> emptyFolders)
        {
            if (emptyFolders.Count() == 0) return string.Empty;
            var sb = new StringBuilder();
            sb.AppendLine(StartItemGroup());
            foreach (var emptyFolder in emptyFolders)
            {
                sb.AppendLine(EmptyFolder(emptyFolder));
            }
            sb.AppendLine(EndItemGroup());
            return sb.ToString();
        }

        private static string StartPropertyGroup()
        {
            return "<PropertyGroup>".Indent();
        }

        private static string EndPropertyGroup()
        {
            return "</PropertyGroup>".Indent();
        }

        private static string StartItemGroup()
        {
            return "<ItemGroup>".Indent();
        }

        private static string EndItemGroup()
        {
            return "</ItemGroup>".Indent();
        }

        private static string ProjectReference(ProjectReference projectReference)
        {
            return $"<ProjectReference Include=\"{projectReference.RelativePath}\" />".DoubleIndent();
        }

        private static string PackageReference(PackageReference packageReference)
        {
            return $"<PackageReference Include=\"{packageReference.Id}\" Version=\"{packageReference.Version}\" />".DoubleIndent();
        }

        private static string PackageReferenceNoTerminate(PackageReference packageReference)
        {
            return $"<PackageReference Include=\"{packageReference.Id}\" Version=\"{packageReference.Version}\" >".DoubleIndent();
        }

        private static string PackageReferenceTerminate()
        {
            return "</PackageReference>".DoubleIndent();
        }

        public static string GacReference(GacReference gacReference)
        {
            return $"<Reference Include=\"{gacReference.Id}\" />".DoubleIndent();
        }

        private static string PrivateAsset(string[] assets)
        {
            return $"<PrivateAssets>{string.Join("; ", assets)}</PrivateAssets>".TripleIndent();
        }

        private static string IncludeAssets(string[] assets)
        {
            return $"<IncludeAssets>{string.Join("; ", assets)}</IncludeAssets>".TripleIndent();
        }

        private static string EmptyFolder(string emptyFolder)
        {
            return $"<Folder Include=\"{emptyFolder}\" />".DoubleIndent();
        }

        private static string RemoveNone(string script)
        {
            return $"<None Remove=\"{script}\" />".DoubleIndent();
        }

        private static string AddEmbeddedResource(string script, string? linkedPath)
        {
            return (linkedPath != null ?
            $"<EmbeddedResource Include=\"{script}\" Link=\"{linkedPath}\" />" :
            $"<EmbeddedResource Include=\"{script}\" />")
                .DoubleIndent();
        }


        private static string TargetFramework(string framework)
        {
            return $"<TargetFramework>{framework}</TargetFramework>".DoubleIndent();
        }

        private static string Title(string title)
        {
            return $"<Title>{title}</Title>".DoubleIndent();
        }

        private static string OutputType(string outputType)
        {
            return $"<OutputType>{outputType}</OutputType>".DoubleIndent();
        }

        private static string Authors()
        {
            return "<Authors>Cash Converters</Authors>".DoubleIndent();
        }

        private static string Copyright()
        {
            return "<Copyright>Copyright © 2015 Cash Converters Pty Ltd</Copyright>".DoubleIndent();
        }
        private static string AssemblyName(string assemblyName)
        {
            return $"<AssemblyName>{assemblyName}</AssemblyName>".DoubleIndent();
        }

        private static string RootNamespace(string rootNamespace)
        {
            return $"<RootNamespace>{rootNamespace}</RootNamespace>".DoubleIndent();
        }

        private static string? GenerateAssemblyInfo()
        {
            return "<GenerateAssemblyInfo>false</GenerateAssemblyInfo>".DoubleIndent();
        }

        private static string IsPackable()
        {
            return "<IsPackable>false</IsPackable>".DoubleIndent();
        }

        private static string AppendTargetFrameworkToOutputPath()
        {
            return "<AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>".DoubleIndent();
        }



        private static string Defaults(ProjectMetadata metadata)
        {
            var sb = new StringBuilder();
            //Maybe replace big format block one day?
            sb.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk\">");
            sb.AppendFormat(@$"
  <PropertyGroup>
    <TargetFramework>net48</TargetFramework>
    <Title>{metadata.Title}</Title>
    <OutputType>{metadata.OutputType}</OutputType>
    <Authors>Cash Converters</Authors>
    <Copyright>Copyright © 2015 Cash Converters Pty Ltd</Copyright>
    <AssemblyName>{metadata.AssemblyName}</AssemblyName>
    <RootNamespace>{metadata.RootNamespace}</RootNamespace>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <IsPackable>false</IsPackable>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
  </PropertyGroup>

  <ItemDefinitionGroup>
    <PackageReference>
      <PrivateAssets>compile;contentFiles;build;buildMultitargeting;buildTransitive;analyzers;native</PrivateAssets>
    </PackageReference>
  </ItemDefinitionGroup>

  <PropertyGroup>
    <WarningLevel>4</WarningLevel>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <WarningsNotAsErrors>612,618,1591</WarningsNotAsErrors>
  </PropertyGroup>
");

            sb.AppendLine();
            return sb.ToString();
        }
    }

    public static class FormatExtensions
    {
        public static string Indent(this string line)
        {
            return line.PadLeft(line.Length + 2);
        }

        public static string DoubleIndent(this string line)
        {
            return line.Indent().Indent();
        }

        public static string TripleIndent(this string line)
        {
            return line.Indent().Indent().Indent();
        }
    }
}
