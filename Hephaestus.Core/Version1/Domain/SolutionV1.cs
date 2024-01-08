using System.Collections.Generic;
using System.Linq;

namespace Hephaestus.Core.Version1.Domain
{
    public class SolutionV1
    {
        public string Path { get; }
        public string Name { get; }
        public IReadOnlyCollection<string> Projects => _projects.ToList().AsReadOnly();

        private readonly HashSet<string> _projects;

        internal SolutionV1(string name, string path, IEnumerable<string> projects)
        {
            Path = path;
            Name = name;
            _projects = new HashSet<string>(projects);
        }
    }
}