# Masterly.Autofac.DependenciesScanner
Assembly scanning and decoration extensions for Autofac.

## Features

- Scan the given assemblies and exteract all services.
- Register extracted services and determine its lifetime according to the implemented interface (eg. `ISocopedDependnecy` => Instance Per Lifetime Scope ...etc).
- Support custom registration by inheret from `ScannerModule`.
- Scan both concrete and generic class types.

## Give a Star! :star:

If you like or are using this project please give it a star. Thanks!

## Installation

Install the [Masterly.Autofac.DependenciesScanner NuGet Package](https://www.nuget.org/packages/Masterly.Autofac.DependenciesScanner).

### Package Manager Console

```
Install-Package Masterly.Autofac.DependenciesScanner
```

### .NET Core CLI

```
dotnet add package Masterly.Autofac.DependenciesScanner
```

## Usage

`IScopedDependency` => `IDependencyA` lifetime would be Instance Per Lifetime Scope
```c#
public interface IDependencyA : IScopedDependency { }
```

`IDependencyB` going to ineheret IDependencyA lifetime with is Instance Per Lifetime Scope
```c#
public interface IDependencyB : IDependencyA { }
```

`ISingletonDependency` => `IDependencyC` lifetime would be Single Instance
```c#
public interface IDependencyC : ISingletonDependency { }
```
```c#
public abstract class AbstractDependencyClassA : IDependencyB { }

// ITransientDependency => AbstractDependencyClassB lifetime would be Instance per dependency
public abstract class AbstractDependencyClassB : ITransientDependency { }

public class DependencyA : IDependencyA { }
public class DependencyB : AbstractDependencyClassA { }
public class DependencyC : AbstractDependencyClassB { }
public class DependencyD : IDependencyC { }
``` 
As you can see above, all services going to be registered as all implemented interfaces and inhereted abstract classes, in addition to itself.
For example, `DependencyB` can be resolved as `IDependencyA`, `IDependencyB`, `AbstractDependencyClassA`, in addition to `DependencyB`.

In the beginning use UseServiceProviderFactory extension in Program.cs as below
```c#
public class Program
{
  public static void Main(string[] args)
  {
    // ASP.NET Core 3.0+:
    // The UseServiceProviderFactory call attaches the
    // Autofac provider to the generic hosting mechanism.
    var host = Host.CreateDefaultBuilder(args)
        .UseAutofacServiceProviderFactory()
        .ConfigureWebHostDefaults(webHostBuilder => {
          webHostBuilder
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>();
        })
        .Build();

    host.Run();
  }
}
```
You can use ContainerBootstraper to prepare and config Autofac container and scan services and modules in all given assemblies. Here, it registers services and Autofac modules and excexutes ScannerModules, then returns a built container (not recomended for Asp Core)
```c#
IContainer container = ContainerBootstrapper.Bootstrap(assemblies);
```

For Asp Core, use ContainerBootstrapper as below

```c#
// ConfigureContainer is where you can register things directly
// with Autofac. This runs after ConfigureServices so the things
// here will override registrations made in ConfigureServices.
// Don't build the container; that gets done for you by the factory.
public void ConfigureContainer(ContainerBuilder containerBuilder)
{
    // Register your own things directly with Autofac here. Don't
    // call containerBuilder.Populate(), that happens in AutofacServiceProviderFactory
    // for you.
    ContainerBootstrapper.Bootstrap(builder, assemblies);
}
```

To register single service
```c#
containerBuilder.RegisterDependency(serviceType);
```
Scan all services those implements `IScopedDependency`, `ITransientDependency` or `ISingletonDependency` and register them in the `ContainerBuilder`, also exceute scanner modules those inheret from `ScannerModule`.
```c#
containerBuilder.ScanDependencies(assemblies);
```

### `ScannerModule`
`ScannerModule` gives tha ability to customize scanning and component registration.
For example:
```c#
public interface IApi { }

public class Api1 : IApi { public Api1(string url) { } }
public class Api2 : IApi { public Api2(string url) { } }

public static class ApiFactory
{
    public static object Create(Type type, string url) {
        object api =  Activator.CreateInstance(type, url);
        return api;
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
```

#### Note
The default lifetime for any service deos not implement any of the mentioned interfaces, it is going to be Instance Per Lifetime Scope

## License

MIT

**Free Software, Hell Yeah!**