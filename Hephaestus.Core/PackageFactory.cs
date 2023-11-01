//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Xml.Linq;
//using MsBuild.Domain;
//using MsBuild.Parsing;

//namespace MsBuild
//{
//    public class PackageFactory
//    {
//        private static PackageReference MakeFramework(XElement element)
//        {
//            //assumes from package.config file
//            var idAttribute = element.Attribute("id");
//            var versionAttribute = element.Attribute("version");

//            if (idAttribute == null)
//            {
//                throw new NullReferenceException("Package Reference Id was null");
//            }

//            if (versionAttribute == null)
//            {
//                throw new NullReferenceException("Package Reference Version was null");
//            }

//            return new PackageReference(idAttribute.Value, versionAttribute.Value);
//        }

//        private static PackageReference MakeSdk(XElement element)
//        {
//            var includeAttribute = element.Attribute("Include");
//            var versionAttribute = element.Attribute("Version") ?? element.Attribute("version");

//            if (includeAttribute == null)
//            {
//                throw new NullReferenceException("Package Reference Include was null");
//            }

//            if (versionAttribute == null)
//            {
//                throw new NullReferenceException("Package Reference version was null");
//            }

//            return new PackageReference(includeAttribute.Value, versionAttribute.Value);
//        }
//    }

//    public class PackageReferenceParser
//    {
//        private Project _project;
//        public PackageReferenceParser(Project project)
//        {
//            _project = project;
//        }

//        public IEnumerable<PackageReference> Parse()
//        {
//            return _project.ProjectFormat == ProjectFormat.Sdk ?
//                ParseSdkPackages() :
//                ParseLegacyPackages();
//        }

//        private IEnumerable<PackageReference> ParseSdkPackages()
//        {
//            return Enumerable.Empty<PackageReference>();
//        }

//        private IEnumerable<PackageReference> ParseLegacyPackages() { return Enumerable.Empty<PackageReference>(); }
//    }
//}