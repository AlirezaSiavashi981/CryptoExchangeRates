using System.Net.Http.Json;
using Domain.Definations.ExternalService.ExchangeRate;
using Domain.Definations.ExternalService.ExchangeRate.Dto;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

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
            var httpResponse = await httpClient.GetFromJsonAsync<dynamic>(uri, cancellationToken: ct);
            if (httpResponse == null)
                throw new Exception("Failed to fetch exchange rates from ExchangeRatesAPI.");

            var result = await httpResponse.Content.ReadAsStringAsync(ct);
            if (!httpResponse.IsSuccessStatusCode)
            {
                logger.LogError($"Status Code : {httpResponse.StatusCode}");

                throw new Exception("The request encountered an error.");
            }

            var response = JsonSerializer.Deserialize<decimal>(result);
            if (response == null)
                throw new Exception("The data result is empty.");

            return response.Rates ?? new Dictionary<string, decimal>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}