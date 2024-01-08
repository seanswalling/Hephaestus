using System;
using System.Collections.Generic;
using Hephaestus.Core.Version1.Domain;

namespace Hephaestus.Core.Version1
{
    public class RepositoryV1ProviderStoreAdapter : IRepositoryV1Store
    {
        private readonly RepositoryV1Provider _repositoryProvider;
        private readonly List<Action> _subscriptions;

        public RepositoryV1ProviderStoreAdapter(RepositoryV1Provider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
            _subscriptions = new List<Action>();
        }

        public void Store(CodeRepositoryV1 repository)
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
