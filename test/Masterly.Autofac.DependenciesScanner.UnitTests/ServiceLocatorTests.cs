using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using FluentAssertions;
using Masterly.Autofac.DependenciesScanner.UnitTests.SampleInterfacesAndClasses;
using Xunit;

namespace Masterly.Autofac.DependenciesScanner.UnitTests
{
    public class ServiceLocatorTests
    {
        [Fact]
        public void RegisterDependncyUsingScannerModuleTest()
        {
            // Arrange
            Assembly[] assemblies = new[] { typeof(ServiceLocatorTests).Assembly };

            // Act
            IContainer container = ContainerBootstrapper.Bootstrap(assemblies);
            ServiceLocator.SetLocatorProvider(container);

            // Assert
            ServiceLocator.Current.GetService<IEnumerable<IApi>>().Count().Should().Be(2);
        }
    }
}