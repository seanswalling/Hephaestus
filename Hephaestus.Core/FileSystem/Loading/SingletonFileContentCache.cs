﻿using System;

namespace Hephaestus.Core.FileSystem.Loading
{
    internal class SingletonFileContentCache : FileCache<string>
    {
        private static readonly Lazy<SingletonFileContentCache> Singleton = new(() => new SingletonFileContentCache());

        private SingletonFileContentCache() { }

        internal static SingletonFileContentCache Instance => Singleton.Value;
    }
}