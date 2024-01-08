using System.Xml.Linq;

namespace Hephaestus.Core.Tests.Parsing.Sdk
{
    public abstract class SdkFormatTestBase
    {
        protected XDocument Project;
        protected XElement SdkElement;

        public SdkFormatTestBase()
        {
            Project = new XDocument();
            SdkElement = new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"));
            Project.Add(SdkElement);
        }
    }
}
