using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Masterly.Autofac.Interfaces;

namespace Masterly.Autofac.DependenciesScanner
{
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Scan all services those implements <see cref="IScopedDependency"/>, <see cref="ITransientDependency"/>or <see cref="ISingletonDependency"/>
        /// and register them in the <see cref="ContainerBuilder"/>, also exceute scanner modules those inheret from <see cref="ScannerModule"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        public static void ScanDependencies(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            ScanAndRegisterServices(builder, assemblies);
            ScanAndExecuteScannerModules(builder, assemblies);
        }

        private static void ScanAndRegisterServices(ContainerBuilder builder, Assembly[] assemblies)
        {
            IEnumerable<Type> serviceTypes = assemblies.SelectMany(x => x.GetExportedServicesTypes());

            foreach (Type serviceType in serviceTypes)
                builder.RegisterDependency(serviceType);
        }

        private static void ScanAndExecuteScannerModules(ContainerBuilder builder, Assembly[] assemblies)
        {
            IEnumerable<Type> scannerModules = assemblies.SelectMany(assembly => assembly.GetExportedTypes<ScannerModule>());

            foreach (Type registration in scannerModules)
                Activator.CreateInstance(registration).As<ScannerModule>().Scan(builder, assemblies);
        }

        /// <summary>
        /// Register a given service type as itself and as all its interfaces and abstract classes
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceType"></param>
        public static void RegisterDependency(this ContainerBuilder builder, Type serviceType)
        {
            Type[] excludedTypes = new[] { typeof(IDependency), typeof(ITransientDependency), typeof(ISingletonDependency), typeof(IScopedDependency) };
            IEnumerable<Type> interfaces = serviceType.GetInterfacesAndAbstractClasses().Except(excludedTypes).Distinct();

            foreach (Type interfaceType in interfaces)
            {
                if (serviceType.IsGenericType)
                    builder.RegisterGeneric(serviceType).As(interfaceType).PropertiesAutowired().RegisterDynamicScope();
                else
                    builder.RegisterType(serviceType).As(interfaceType).PropertiesAutowired().RegisterScope();
            }

            if (serviceType.IsGenericType)
                builder.RegisterGeneric(serviceType).AsSelf().PropertiesAutowired().RegisterDynamicScope();
            else
                builder.RegisterType(serviceType).AsSelf().PropertiesAutowired().RegisterScope();
        }

        private static IRegistrationBuilder<object, ReflectionActivatorData, SingleRegistrationStyle> RegisterScope(
            this IRegistrationBuilder<object, ReflectionActivatorData, SingleRegistrationStyle> registration)
        {
            TypedService typedService = registration.RegistrationData.Services.FirstOrDefault() as TypedService;
            Type type = typedService.ServiceType;

            if (typeof(ISingletonDependency).IsAssignableFrom(type))
                registration.SingleInstance();

            else if (typeof(ITransientDependency).IsAssignableFrom(type))
                registration.InstancePerDependency();

            else
                registration.InstancePerLifetimeScope();


            return registration;
        }

        private static IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> RegisterDynamicScope(
            this IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> registration)
        {
            TypedService typedService = registration.RegistrationData.Services.FirstOrDefault() as TypedService;
            Type type = typedService.ServiceType;

            if (typeof(ISingletonDependency).IsAssignableFrom(type))
                registration.SingleInstance();

            else if (typeof(ITransientDependency).IsAssignableFrom(type))
                registration.InstancePerDependency();

            else
                registration.InstancePerLifetimeScope();

            return registration;
        }
    }
}