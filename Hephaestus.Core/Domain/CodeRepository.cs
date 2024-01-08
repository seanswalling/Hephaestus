using System;
using System.Collections.Generic;

namespace Hephaestus.Core.Domain
{
    public class CodeRepository
    {
        public string Name { get; }
        public IEnumerable<Solution> Solutions { get; }

        public CodeRepository(string name, IEnumerable<Solution> solutions)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(solutions, nameof(solutions));
            Name = name;
            Solutions = solutions;
        }
    }
}
