using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Hephaestus.Core.Application
{
    public class ContentCache
    {
        public string Name { get; init; }
        public FrozenDictionary<string, string> FileContent { get; init; }

        private string _filePath;
        private ContentCache(string name, FrozenDictionary<string, string> fileContent)
        {
            FileContent = fileContent;
            Name = name;
            _filePath = GetCacheLocation(name);
        }

        public void Save()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(FileContent, options));
        }

        public bool IsEmpty()
        {
            return FileContent.Count == 0;
        }

        public static string GetCacheLocation(string name)
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Hephaestus");
            return Path.Combine(folder, $"hephaestus-{name}-content-cache.json");
        }

        public static ContentCache Load(string name)
        {
            var cache = GetCacheLocation(name);

            if (!File.Exists(cache))
                throw new FileNotFoundException(cache);

            var content = File.ReadAllText(cache);

            if (content == null)
                throw new InvalidDataException("Cache Content Missing");

            var contents = JsonSerializer.Deserialize<Dictionary<string, string>>(content)!.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

            return new ContentCache(name, contents);
        }

        public static ContentCache Empty()
        {
            return new ContentCache(string.Empty, FrozenDictionary<string, string>.Empty);
        }

        public static bool Exists(string name)
        {
            return File.Exists(GetCacheLocation(name));
        }

        public static ContentCache Build(string name, IDictionary<string, string> fileCollection)
        {
            return new ContentCache(name, fileCollection.ToFrozenDictionary());
        }
    }
}
