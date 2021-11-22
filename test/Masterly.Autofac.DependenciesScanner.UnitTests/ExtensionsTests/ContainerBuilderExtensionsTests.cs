using System;
using System.Reflection;
using Autofac;
using FluentAssertions;
using Xunit;

namespace Masterly.Autofac.DependenciesScanner.UnitTests.Extensions
{
    public class ContainerBuilderExtensionsTests
    {
        [Fact]
        public void ScanDependenciesTest()
        {
            // Arrange
            var builder = new ContainerBuilder();
            Assembly[] assemblies = new[] { typeof(ContainerBuilderExtensionsTests).Assembly };

            // Act
            builder.ScanDependencies(assemblies);
            IContainer container = builder.Build();

            // Assert
            container.Resolve<IDependencyC>().GetType().Should().Be(typeof(DependencyD));
        }

        [Fact]
        public void RegisterDependencyTest()
        {
            // Arrange
            var builder = new ContainerBuilder();
            Type serviceType = typeof(DependencyC);

            // Act
            builder.RegisterDependency(serviceType);
            IContainer container = builder.Build();

            // Assert
            container.Resolve<AbstractDependencyClassB>().GetType().Should().Be(typeof(DependencyC));
        }
    }
}