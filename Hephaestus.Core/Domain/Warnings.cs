namespace Hephaestus.Core.Domain
{
    public class Warnings
    {
        public string? WarningLevel { get; }
        public bool? TreatWarningsAsErrors { get; }
        public string[] WarningsNotAsErrors { get; }

        public Warnings(string? warningLevel, bool? treatWarningsAsErrors, string[] warningsNotAsErrors)
        {
            WarningLevel = warningLevel;
            TreatWarningsAsErrors = treatWarningsAsErrors;
            WarningsNotAsErrors = warningsNotAsErrors;
        }
    }
}
