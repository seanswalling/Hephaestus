using System.Xml.Linq;

namespace Hephaestus.Core.Tests.Parsing.Legacy
{
    public class LegacyFormatTestBase
    {
        protected XDocument Packages;
        protected XElement PackagesRoot;
        protected XDocument Project;
        protected XElement ProjectRoot;
        protected XNamespace Namespace = "http://schemas.microsoft.com/developer/msbuild/2003";
        public LegacyFormatTestBase()
        {
            Packages = new XDocument();
            PackagesRoot = new XElement("packages");
            Packages.Add(PackagesRoot);
            Project = new XDocument();
            ProjectRoot = new XElement(Namespace + "Project");
            Project.Add(ProjectRoot);
        }
    }
}