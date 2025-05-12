using System;
using System.Linq;
using System.Xml.Linq;

namespace Hephaestus.Core.Parsing.Sdk
{
    public class SdkTestProjectParser : ITestProjectParser
    {
        public bool Parse(XDocument project)
        {
            var packages = new SdkPackageReferenceParser(project).Parse();
            return packages.Any(x =>
                x.Id.Equals("xunit.v3", StringComparison.OrdinalIgnoreCase) ||
                x.Id.Equals("Microsoft.NET.Test.Sdk", StringComparison.OrdinalIgnoreCase) ||
                x.Id.Equals("xunit.runner.visualstudio", StringComparison.OrdinalIgnoreCase));
        }
    }
}