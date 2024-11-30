using BusinessLogic.Services;
using Domain.Definations.ExternalService.CoinMarketCap;
using Domain.Definations.ExternalService.ExchangeRate;
using Infrastructure.Implementations.ExternalService;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DependencyExtensions;

public static class IoCContainer
{
    public static IServiceCollection AddIoCContainerConfig(this IServiceCollection services)
    {
        // IoC Application
        services.AddScoped<CryptoPriceCalculateService>();

        // IoC External
        services.AddScoped<IExchangeRatesExternalService, ExchangeRatesExternalService>();
        services.AddScoped<ICoinMarketCapExternalService, CoinMarketCapExternalService>();
        //services.AddScoped<IHttpClientFactory>();
        
        return services;
    }
}