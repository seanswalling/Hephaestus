using System;
using System.Collections.Generic;

namespace Hephaestus.Core.Domain
{
    public class Solution
    {
        public string Name { get; }
        public IEnumerable<Project> Projects { get; }

        public Solution(string name, IEnumerable<Project> projects)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(projects, nameof(projects));
            Name = name;
            Projects = projects;
        }
    }
}