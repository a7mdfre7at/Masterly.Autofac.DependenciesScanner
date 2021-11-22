using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace Masterly.Autofac.DependenciesScanner.UnitTests.SampleInterfacesAndClasses
{
    public interface IApi { }

    public class Api1 : IApi { public Api1(string url) { } }
    public class Api2 : IApi { public Api2(string url) { } }

    public static class ApiFactory
    {
        public static object Create(Type type, string url)
        {
            var x = Activator.CreateInstance(type, url);
            return x;
        }
    }


    public class SampleScannerModule : ScannerModule
    {
        public override void Scan(ContainerBuilder builder, params Assembly[] assemblies)
        {
            foreach (var serviceType in assemblies.SelectMany(assembly => assembly.GetExportedTypes().Where(t => typeof(IApi).IsAssignableFrom(t))))
                Register(builder, serviceType);
        }

        private void Register(ContainerBuilder builder, Type serviceType)
        {
            if (serviceType.IsInterface || serviceType.IsAbstract || !typeof(IApi).IsAssignableFrom(serviceType))
                return;

            string url = "https://example.com";

            builder.RegisterComponent(RegistrationBuilder
                .ForDelegate((ctx, parameters) => ApiFactory.Create(serviceType, url))
                .As(serviceType)
                .As<IApi>()
                .InstancePerDependency()
                .CreateRegistration());
        }
    }
}