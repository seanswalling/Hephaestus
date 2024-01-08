using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Version1.Domain;

namespace Hephaestus.Core.Version1.Building
{
    public class SdkProjectV1FileBuilder
    {
        private readonly ProjectV1 _project;

        public SdkProjectV1FileBuilder(ProjectV1 project)
        {
            _project = project;
        }

        public string[] ListDependencies()
        {
            return _project.Packages.Select(p => p.Name)
                .Concat(_project.References.Select(r => Path.GetFileNameWithoutExtension(r.RelativePath)))
                .ToArray();
        }

        public string Build()
        {
            var sb = new StringBuilder();
            sb.Append(Defaults(_project.ProjectName, _project.OutputType));
            sb.Append(EmbeddedFileBlock(
                _project.EmbeddedResources
                    .Where(x => x.RelativePath.Contains(".sql") || x.RelativePath.Contains(".docx"))
                    .ToArray()
            ));
            sb.AppendLine();
            sb.Append(ProjectReferenceBlock(_project.DirectReferences));
            sb.AppendLine();
            sb.Append(PackageReferenceBlock(_project.DirectPackages));
            sb.AppendLine();
            sb.Append(EmptyFoldersBlock(_project.EmptyFolders));
            sb.Append("</Project>");
            return sb.ToString();
        }

        private static string EmbeddedFileBlock(EmbeddedResourceV1[] scripts)
        {
            var sb = new StringBuilder();
            sb.AppendLine(StartItemGroup());
            foreach (var script in scripts)
            {
                if (script.LinkedPath == null)
                {
                    sb.AppendLine(RemoveNone(script.RelativePath));
                }
            }
            sb.AppendLine(EndItemGroup());
            sb.AppendLine();

            sb.AppendLine(StartItemGroup());
            foreach (var script in scripts)
            {
                sb.AppendLine(AddEmbeddedResource(script.RelativePath, script.LinkedPath));
            }
            sb.AppendLine(EndItemGroup());
            return sb.ToString();
        }

        private static string ProjectReferenceBlock(IEnumerable<ProjectReferenceV1> projectReferences)
        {
            var sb = new StringBuilder();
            sb.AppendLine(StartItemGroup());
            foreach (var projectReference in projectReferences)
            {
                sb.AppendLine(ProjectReference(projectReference));
            }
            sb.AppendLine(EndItemGroup());
            return sb.ToString();
        }

        private static string PackageReferenceBlock(IEnumerable<PackageReferenceV1> packageReferences)
        {
            var sb = new StringBuilder();
            sb.AppendLine(StartItemGroup());
            foreach (var packageReference in packageReferences)
            {
                if (packageReference.Name.Equals("xunit.runner.visualstudio", StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine(PackageReferenceNoTerminate(packageReference));
                    sb.AppendLine(PrivateAsset(new[] { "all" }));
                    sb.AppendLine(IncludeAssets(new[] { "runtime", "build", "native", "contentfiles", "analyzers", "buildtransitive" }));
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

        private static string EmptyFoldersBlock(IEnumerable<string> emptyFolders)
        {
            var sb = new StringBuilder();
            sb.AppendLine(StartItemGroup());
            foreach (var emptyFolder in emptyFolders)
            {
                sb.AppendLine(EmptyFolder(emptyFolder));
            }
            sb.AppendLine(EndItemGroup());
            return sb.ToString();
        }

        private static string StartItemGroup()
        {
            return "<ItemGroup>".Indent();
        }

        private static string EndItemGroup()
        {
            return "</ItemGroup>".Indent();
        }

        private static string ProjectReference(ProjectReferenceV1 projectReference)
        {
            return $"<ProjectReference Include=\"{projectReference.RelativePath}\" />".DoubleIndent();
        }

        private static string PackageReference(PackageReferenceV1 packageReference)
        {
            return $"<PackageReference Include=\"{packageReference.Name}\" Version=\"{packageReference.Version}\" />".DoubleIndent();
        }

        private static string PackageReferenceNoTerminate(PackageReferenceV1 packageReference)
        {
            return $"<PackageReference Include=\"{packageReference.Name}\" Version=\"{packageReference.Version}\" >".DoubleIndent();
        }

        private static string PackageReferenceTerminate()
        {
            return "</PackageReference>".DoubleIndent();
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

        private static string Defaults(string name, OutputType outputType)
        {
            var sb = new StringBuilder();
            //Maybe replace big format block one day?
            sb.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk\">");
            sb.AppendFormat(@$"
  <PropertyGroup>
    <TargetFramework>net48</TargetFramework>
    <Title>{name}</Title>
    <OutputType>{outputType}</OutputType>
    <Authors>Cash Converters</Authors>
    <Copyright>Copyright © 2015 Cash Converters Pty Ltd</Copyright>
    <AssemblyName>{name}</AssemblyName>
    <RootNamespace>{name}</RootNamespace>
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
