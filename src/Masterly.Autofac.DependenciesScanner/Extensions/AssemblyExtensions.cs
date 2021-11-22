using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Masterly.Autofac.Interfaces;

namespace Masterly.Autofac.DependenciesScanner
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Gets the public types defined in this assembly that are visible outside the assembly and those are not (interface or abstarct).
        /// </summary>
        /// <typeparam name="T">A base type of the exported type</typeparam>
        /// <param name="assembly">An assembly to scan</param>
        /// <returns>An IEnumerable represents all types those inheret from a given type.</returns>
        public static IEnumerable<Type> GetExportedTypes<T>(this Assembly assembly)
            => assembly
            .GetExportedTypes()
            .Where(type => !type.IsAbstract && typeof(T).IsAssignableFrom(type))
            .ToList();

        /// <summary>
        /// Gets the public types defined in this assembly that are visible outside the assembly and implements <see cref="IDependency"/>.
        /// </summary>
        /// <param name="assembly">An assembly to scan</param>
        /// <returns>An IEnumerable represents all types those implements <see cref="IDependency"/>.</returns>
        public static IEnumerable<Type> GetExportedServicesTypes(this Assembly assembly)
            => assembly.GetExportedTypes<IDependency>().ToList();
    }
}