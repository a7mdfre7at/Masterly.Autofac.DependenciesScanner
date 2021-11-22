using System;
using System.Linq;
using FluentAssertions;
using Masterly.Autofac.Interfaces;
using Xunit;

namespace Masterly.Autofac.DependenciesScanner.UnitTests.Extensions
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void GetInterfacesAndAbstractClasses_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Type type = typeof(DependencyB);
            Type[] expectedTypes = new[] { typeof(AbstractDependencyClassA), typeof(IDependencyB), typeof(IDependencyA), typeof(IDependency), typeof(IScopedDependency) };

            // Act
            Type[] result = type.GetInterfacesAndAbstractClasses();

            // Assert
            result.Count().Should().Be(5);
            result.Intersect(expectedTypes).Count().Should().Be(5);
        }
    }
}