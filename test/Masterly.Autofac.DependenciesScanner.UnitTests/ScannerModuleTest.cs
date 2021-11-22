using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using FluentAssertions;
using Masterly.Autofac.DependenciesScanner.UnitTests.SampleInterfacesAndClasses;
using Xunit;

namespace Masterly.Autofac.DependenciesScanner.UnitTests
{
    public class ScannerModuleTest
    {
        [Fact]
        public void RegisterDependncyUsingScannerModuleTest()
        {
            // Arrange
            Assembly[] assemblies = new[] { typeof(ScannerModuleTest).Assembly };

            // Act
            IContainer container = ContainerBootstrapper.Bootstrap(assemblies);

            // Assert
            container.Resolve<IEnumerable<IApi>>().Count().Should().Be(2);
        }
    }
}
