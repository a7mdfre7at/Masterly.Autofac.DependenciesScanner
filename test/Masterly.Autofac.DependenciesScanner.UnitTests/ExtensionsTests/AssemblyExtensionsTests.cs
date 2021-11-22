using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Masterly.Autofac.DependenciesScanner.UnitTests.Extensions
{
    public class AssemblyExtensionsTests
    {
        private readonly Assembly _assembly = typeof(AssemblyExtensionsTests).Assembly;
        [Fact]
        public void GetExportedTypesTest()
        {
            // Act
            IEnumerable<Type> result = _assembly.GetExportedTypes<IDependencyA>();

            // Assert
            result.Count().Should().Be(2);
            result.Any(t => t == typeof(IDependencyA)).Should().BeFalse();
            result.Any(t => t == typeof(DependencyA)).Should().BeTrue();
            result.Any(t => t == typeof(DependencyB)).Should().BeTrue();
        }

        [Fact]
        public void GetExportedServicesTypesTest()
        {
            // Act
            IEnumerable<Type> result = _assembly.GetExportedServicesTypes();

            // Assert
            result.Count().Should().Be(4);
            result.Any(t => t == typeof(IDependencyA)).Should().BeFalse();
            result.Any(t => t == typeof(DependencyA)).Should().BeTrue();
            result.Any(t => t == typeof(DependencyB)).Should().BeTrue();
            result.Any(t => t == typeof(DependencyC)).Should().BeTrue();
            result.Any(t => t == typeof(DependencyD)).Should().BeTrue();
        }
    }
}