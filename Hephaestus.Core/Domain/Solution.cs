using System.Collections.Generic;
using System.Linq;

namespace Hephaestus.Core.Domain
{
    public class Solution
    {
        public string Path { get; }
        public string Name { get; }
        public IReadOnlyCollection<string> Projects => _projects.ToList().AsReadOnly();

        private readonly HashSet<string> _projects;

        internal Solution(string name, string path, IEnumerable<string> projects)
        {
            Path = path;
            Name = name;
            _projects = new HashSet<string>(projects);
        }
    }
}