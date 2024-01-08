using System.Collections.Generic;

namespace Hephaestus.CLI
{
    public class Result<T>(T value, List<string> errors)
    {
        public List<string> Errors { get; } = errors;
        public T Value { get; } = value;
    }

}