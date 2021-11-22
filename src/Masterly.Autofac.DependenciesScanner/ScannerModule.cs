using System.Reflection;
using Autofac;

namespace Masterly.Autofac.DependenciesScanner
{
    /// <summary>
    /// Customize scaning the the given assemblies and extract services for custom registration.
    /// </summary>
    public abstract class ScannerModule
    {
        protected ScannerModule() { }

        public abstract void Scan(ContainerBuilder builder, params Assembly[] assemblies);
    }
}