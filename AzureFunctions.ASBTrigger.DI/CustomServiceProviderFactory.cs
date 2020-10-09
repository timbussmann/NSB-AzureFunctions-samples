using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

class CustomServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
{
    private readonly IServiceCollection _functionsServiceCollection;

    public CustomServiceProviderFactory(IServiceCollection functionsServiceCollection)
    {
        _functionsServiceCollection = functionsServiceCollection;
    }

    public IServiceCollection CreateBuilder(IServiceCollection services)
    {
        return _functionsServiceCollection.Add(services);
    }

    public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
    {
        return containerBuilder.BuildServiceProvider();
    }
}