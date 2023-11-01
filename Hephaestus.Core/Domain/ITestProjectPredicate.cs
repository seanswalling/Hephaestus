namespace Hephaestus.Core.Domain
{
    public interface ITestProjectPredicate
    {
        bool IsTestProject(string projectName);
    }
}