using System;
using System.Collections.Generic;

namespace Hephaestus.Core.Version1.Domain
{
    public interface INamespaceLookup
    {
        public IEnumerable<ProjectReferenceV1> GetProjectsForNamespace(string nspace, ProjectV1 project);
        public IEnumerable<PackageReferenceV1> GetPackageReferencesForNamespace(string nspace, ProjectV1 project);
    }

    public class DefaultNamespaceLookup : INamespaceLookup
    {
        //consider a namespace type to add rules to
        private readonly Dictionary<string, ProjectV1> _lookup;
        private readonly List<Tuple<string, ProjectV1>> _collection;
        public DefaultNamespaceLookup(IEnumerable<ProjectV1> projects)
        {
            _collection = new List<Tuple<string, ProjectV1>>();
            _lookup = new Dictionary<string, ProjectV1>();
            foreach (var project in projects)
            {
                foreach (var ns in project.Namespaces)
                {
                    //Not sure how to deal with namespace collisions atm.
                    //Not sure I want to store the whole project here.
                    _lookup.TryAdd(ns, project);
                    _collection.Add(Tuple.Create(ns, project));
                }
            }
        }

        public IEnumerable<ProjectReferenceV1> GetProjectsForNamespace(string nspace, ProjectV1 project)
        {
            if (_lookup.TryGetValue(nspace, out var value))
            {
                return Array.Empty<ProjectReferenceV1>();
            }
            else
            {
                return Array.Empty<ProjectReferenceV1>();
            }
        }

        public IEnumerable<PackageReferenceV1> GetPackageReferencesForNamespace(string nspace, ProjectV1 project)
        {
            throw new NotImplementedException();
        }
    }
}