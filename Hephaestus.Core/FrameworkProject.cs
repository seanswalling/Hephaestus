namespace Hephaestus.Core
{
    //public class FrameworkProject : Project
    //{
    //    private readonly XNamespace _xmlNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";
    //    private readonly XDocument _packagesContent;
    //    public FrameworkProject(string path, XDocument content) : base(path, content)
    //    {
    //        IsSdk = false;
    //        TargetFramework = GetElementValue("TargetFrameworkVersion");
    //        var packageconfig = Path.Combine(Directory.GetParent(FilePath).FullName, "packages.config");
    //        if (File.Exists(packageconfig))
    //        {
    //            _packagesContent = XDocument.Load(packageconfig);
    //            PackageDependecies = _packagesContent.Descendants("package").Select(x => PackageFactory.MakeFramework(x)).ToArray();
    //        }
    //        else
    //        {
    //            PackageDependecies = Array.Empty<Package>();
    //        }

    //        ProjectDependencies = GetElements("ProjectReference")
    //            .Select(x => x.Attribute("Include"))
    //            .Select(x => x.Value)
    //            .Select(path => Path.GetFullPath(Path.Combine(Directory.GetParent(FilePath).FullName, path)))
    //            .Select(x => ProjectFactory.Make(x))
    //            .ToArray();
    //    }

    //    protected override XElement GetElement(string elementName, int index = 0)
    //    {
    //        var elements = GetElements(elementName);
    //        return elements.Length == 0 ? null : elements[index];
    //    }

    //    protected override XElement[] GetElements(string elementName)
    //    {
    //        return Content.Descendants(_xmlNamespace + elementName).ToArray();
    //    }

    //    protected override string GetElementValue(string elementName, int index = 0)
    //    {
    //        var element = GetElement(elementName, index);
    //        return element?.Value ?? string.Empty;
    //    }

    //    protected override string[] GetElementValues(string elementName)
    //    {
    //        return GetElements(elementName).Select(x => x.Value).ToArray();
    //    }
    //}


}