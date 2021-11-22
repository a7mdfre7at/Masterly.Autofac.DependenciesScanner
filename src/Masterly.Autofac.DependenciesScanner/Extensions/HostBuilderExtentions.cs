using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Masterly.Autofac.DependenciesScanner
{
    public static class IHostBuilderExtentions
    {
        public static IHostBuilder UseAutofacServiceProviderFactory(this IHostBuilder hostBuilder, Action<ContainerBuilder> configurationAction = null)
            => hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory(configurationAction));
    }
}