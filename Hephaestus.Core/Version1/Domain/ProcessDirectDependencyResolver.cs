using System;
using System.Collections.Generic;

namespace Hephaestus.Core.Version1.Domain
{
    public class ProcessDirectDependencyResolver
    {
        public Predicate<string> Selector { get; }
        public Func<string, IEnumerable<string>> Transformer { get; }
        public Predicate<string> ThrowOnError { get; }

        public ProcessDirectDependencyResolver(Predicate<string> selector, Func<string, IEnumerable<string>> transformer, Predicate<string>? throwOnError = null)
        {
            Selector = selector;
            Transformer = transformer;
            ThrowOnError = throwOnError ?? ((x) => true);
        }
    }
}
