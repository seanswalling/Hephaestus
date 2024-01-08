using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Sdk
{
    public class SdkWarningParser : IWarningsParser
    {
        public Warnings Parse(XDocument document)
        {
            var level = document.Descendants("WarningLevel").SingleOrDefault()?.Value;
            var warningsAsErrors = document.Descendants("TreatWarningsAsErrors").SingleOrDefault()?.Value;
            var warningsNotAsErrors = document.Descendants("WarningsNotAsErrors").SingleOrDefault()?.Value.Split(",");

            return new Warnings(
                level ?? string.Empty,
                warningsAsErrors != null ? bool.Parse(warningsAsErrors) : null,
                warningsNotAsErrors ?? []);
        }
    }


}
