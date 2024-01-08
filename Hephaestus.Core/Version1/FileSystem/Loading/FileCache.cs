using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Hephaestus.Core.Version1.FileSystem.Loading
{
    public class FileCache<T>
    {
        private readonly ConcurrentDictionary<string, T> _files;

        internal FileCache()
        {
            _files = new ConcurrentDictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        }

        public T GetFile(string key)
        {
            return _files[key];
        }

        public bool HasFile(string key)
        {
            return _files.ContainsKey(key);
        }

        internal bool TryAddFile(string key, T file)
        {
            return _files.TryAdd(key, file);
        }

        internal IReadOnlyCollection<KeyValuePair<string, T>> Entries()
        {
            return _files;
        }

        internal ICollection<string> Keys()
        {
            return _files.Keys;
        }

        internal void UpdateFile(string key, T file)
        {
            _files[key] = file;
        }

        internal void Remove(string key)
        {
            _files.Remove(key, out _);
        }

        internal void Set(string key, T file)
        {
            if (HasFile(key))
            {
                UpdateFile(key, file);
            }
            else
            {
                TryAddFile(key, file);
            }
        }


        internal Dictionary<string, T> Copy()
        {
            return new Dictionary<string, T>(_files);
        }
    }
}
