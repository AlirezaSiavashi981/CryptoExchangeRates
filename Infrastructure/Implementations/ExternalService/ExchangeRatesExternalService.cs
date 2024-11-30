using System.Net.Http.Json;
using Domain.Definations.ExternalService.ExchangeRate;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Domain.Definations.ExternalService.ExchangeRate.Dto;

namespace Infrastructure.Implementations.ExternalService;

public class ExchangeRatesExternalService(
    IHttpClientFactory client,
    ILogger<ExchangeRatesExternalService> logger,
    IConfiguration config)
    : IExchangeRatesExternalService
{
    public async Task<Dictionary<string, decimal>> GetConvertedPricesAsync(CancellationToken ct)
    {
        var httpClient = client.CreateClient();

        // configuration 
        var baseUrl = config["ExchangeRatesConfig:BaseUrl"];
        var apiKey = config["ExchangeRatesConfig:ApiKey"];

        var uri = $"{baseUrl}/v1/latest?access_key={apiKey}";

        try
        {
            var httpResponse = await httpClient.GetFromJsonAsync<ExchangeRatesResponse>(uri, cancellationToken: ct);
            if (httpResponse == null || httpResponse.Rates == null)
            {
                throw new Exception("Failed to fetch exchange rates or rates data is missing.");
            }

            return httpResponse.Rates;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}