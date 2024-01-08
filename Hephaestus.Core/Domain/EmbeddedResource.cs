using System;

namespace Hephaestus.Core.Domain
{
    public class EmbeddedResource
    {
        public string FilePath { get; }
        public string? LinkedPath { get; private set; }

        public EmbeddedResource(string filePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath, nameof(filePath));
            FilePath = filePath;
        }

        public void Link(string linkedPath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(linkedPath, nameof(linkedPath));
            LinkedPath = linkedPath;
        }

        public bool Equals(EmbeddedResource? other)
        {
            if (other is null) return false;
            var filePathEq = other.FilePath.Equals(FilePath, StringComparison.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(LinkedPath) ^ string.IsNullOrWhiteSpace(other.LinkedPath)) return false;
            if (other.LinkedPath == null) return filePathEq;

            var linkPathEq = other.LinkedPath.Equals(LinkedPath, StringComparison.OrdinalIgnoreCase);
            return filePathEq && linkPathEq;
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(FilePath) ^
                 StringComparer.OrdinalIgnoreCase.GetHashCode(LinkedPath ?? string.Empty);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not EmbeddedResource) return false;
            return Equals(obj as EmbeddedResource);
        }
    }
}
