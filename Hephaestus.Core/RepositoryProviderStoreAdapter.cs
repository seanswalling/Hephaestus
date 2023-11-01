using System;
using System.Collections.Generic;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core
{
    public class RepositoryProviderStoreAdapter : IRepositoryStore
    {
        private readonly RepositoryProvider _repositoryProvider;
        private readonly List<Action> _subscriptions;

        public RepositoryProviderStoreAdapter(RepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
            _subscriptions = new List<Action>();
        }

        public void Store(CodeRepository repository)
        {
            _repositoryProvider.Update(repository);
            NotifySubscribers();
        }

        public void OnModelUpdate(Action subscription)
        {
            _subscriptions.Add(subscription);
        }

        private void NotifySubscribers()
        {
            foreach (var subscription in _subscriptions)
            {
                subscription();
            }
        }
    }
}
