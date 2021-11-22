using System.Reflection;
using Autofac;

namespace Masterly.Autofac.DependenciesScanner
{
    public static class ContainerBootstrapper
    {
        /// <summary>
        /// Prepare and config Autofac container and scan services and modules in all given assemblies
        /// </summary>
        /// <param name="assemblies">An assemblies to scan</param>
        /// <returns>Autofac container</returns>
        public static IContainer Bootstrap(params Assembly[] assemblies)
        {
            var containerBuilder = Bootstrap(new ContainerBuilder(), assemblies);
            return containerBuilder.Build();
        }

        /// <summary>
        /// Prepare and config Autofac container and scan services and modules in all given assemblies
        /// </summary>
        /// <param name="containerBuilder">A given container to register services in</param>
        /// <param name="assemblies">An assemblies to scan</param>
        /// <returns>Autofac container builder</returns>
        public static ContainerBuilder Bootstrap(ContainerBuilder containerBuilder, params Assembly[] assemblies)
        {
            containerBuilder.ScanDependencies(assemblies);
            containerBuilder.RegisterAssemblyModules(assemblies);
            containerBuilder.RegisterInstance(assemblies).Named<Assembly[]>("_startupAssemblies").AsSelf().SingleInstance();

            return containerBuilder;
        }
    }
}