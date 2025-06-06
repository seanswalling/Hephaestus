﻿using System;
using System.Collections.Generic;
using System.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Factories;

namespace Hephaestus.Core.Application
{
    public class Application
    {
        private readonly CodeRespositoryParser _parser;
        private readonly FileSystemLoader _fileSystemLoader;
        private readonly BasicFileCollection _files;
        private readonly KnownRepositoryManager _repositoryManager;

        public string Name { get; private set; }
        public string Path { get; private set; }
        public CacheManager CacheManager { get; private set; }
        public IEnumerable<KnownRepository> KnownRepositories { get; private set; }

        public Application(string applicationRoot)
        {
            _repositoryManager = new KnownRepositoryManager(applicationRoot);
            KnownRepositories = _repositoryManager.Load();

            _fileSystemLoader = new FileSystemLoader();

            Name = string.Empty;
            Path = string.Empty;

            CacheManager = CacheManager.Empty();

            _files = new BasicFileCollection(CacheManager);
            _parser = new CodeRespositoryParser(
                new SolutionParser(
                    new ProjectParser(
                        new ReferenceParserFactory(),
                        new EmbeddedResourceParserFactory(),
                        new ProjectMetadataParser(
                            new ProjectFormatParser(),
                            new ProjectFrameworkParserFactory(new TfmTranslator()),
                            new ProjectOutputTypeParserFactory(new OutputTypeTranslator()),
                            new AssemblyNameParserFactory(),
                            new RootNamespaceParserFactory(),
                            new TitleParserFactory(),
                            new WarningsParserFactory(),
                            new TestProjectParserFactory()),
                        new CSharpFileListerFactory(_files),
                        new CSharpFileParser(
                            new CSharpFileNamespaceDeclarationParser(),
                            new CSharpFileUsingDirectiveParser()),
                        _files),
                    _files),
                _files);
        }

        public void SetRepository(KnownRepository kr)
        {
            Name = kr.Name;
            Path = kr.Path;
        }

        public void LoadRepository()
        {
            CacheManager = CacheManager.Load(Name);
            _files.Update(CacheManager);
        }

        public void AddRepository(string name, string path)
        {
            var kr = new KnownRepository(name, path);
            if (KnownRepositoryManager.Exists(kr))
            {
                KnownRepositories = KnownRepositories.Append(kr);
                _repositoryManager.Save(KnownRepositories);
            }
        }

        public string GetFileContent(string fullPath)
        {
            return _files.GetContent(fullPath);
        }

        public CodeRepository Parse()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(Name);
            ArgumentException.ThrowIfNullOrWhiteSpace(Path);

            if (CacheManager.IsEmpty())
            {
                throw new InvalidOperationException("Caches are Empty, cannot parse");
            }

            return _parser.Parse(Name, Path);
        }

        public void RebuildCache()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(Name);
            ArgumentException.ThrowIfNullOrWhiteSpace(Path);

            var files = _fileSystemLoader.LoadAllFiles(Path);
            CacheManager = CacheManager.Build(Name, files);
            _files.Update(CacheManager);
            CacheManager.Save();
        }

        public void Clear()
        {
            Name = string.Empty;
            Path = string.Empty;
            CacheManager = CacheManager.Empty();
            _files.Update(CacheManager);
        }
    }
}
