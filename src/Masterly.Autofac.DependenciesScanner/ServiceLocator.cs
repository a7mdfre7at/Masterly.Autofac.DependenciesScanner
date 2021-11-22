using System;
using Autofac;

namespace Masterly.Autofac.DependenciesScanner
{
    public class ServiceLocator
    {
        private readonly ILifetimeScope _currentServiceProvider;
        private static ILifetimeScope _serviceProvider;

        public ServiceLocator(ILifetimeScope currentServiceProvider) => _currentServiceProvider = currentServiceProvider;

        public static ServiceLocator Current => new ServiceLocator(_serviceProvider);

        public static void SetLocatorProvider(ILifetimeScope serviceProvider) => _serviceProvider = serviceProvider;

        public object GetService(Type serviceType) => _currentServiceProvider.Resolve(serviceType);

        public TService GetService<TService>() => _currentServiceProvider.Resolve<TService>();
    }
}