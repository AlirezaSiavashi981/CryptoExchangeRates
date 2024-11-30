using System.Net.Http.Json;
using System.Text.Json;
using Domain.Definations.ExternalService.CoinMarketCap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Implementations.ExternalService;

public class CoinMarketCapService(
    IHttpClientFactory client,
    IConfiguration config,
    ILogger<CoinMarketCapService> logger)
    : ICoinMarketCapExternalService
{
    public async Task<decimal?> GetCryptoPriceInUSDAsync(string cryptoCode, CancellationToken ct)
    {
        var httpClient = client.CreateClient();

        // Configure connection to API
        var baseUrl = config["CoinMarketCapConfig:BaseUrl"];
        var apiKey = config["CoinMarketCapConfig:ApiKey"];

        // Set the API key in the DefaultRequestHeaders
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apiKey);
        httpClient.DefaultRequestHeaders.Add("Accepts", "application/json");

        var uri = $"{baseUrl}/v1/cryptocurrency/quotes/latest?symbol={cryptoCode}";

        try
        {
            var httpResponse = await httpClient.GetFromJsonAsync<dynamic>(uri, cancellationToken: ct);
            if (httpResponse == null)
                throw new Exception("Failed to fetch cryptocurrency price from CoinMarketCap API.");

            var result = await httpResponse.Content.ReadAsStringAsync(ct);
            if (!httpResponse.IsSuccessStatusCode)
            {
                logger.LogError($"Status Code : {httpResponse.StatusCode}");

                throw new Exception("The request encountered an error.");
            }

            var response = JsonSerializer.Deserialize<decimal>(result);
            if (response == null)
                throw new Exception("The data result is empty.");

            var usdPrice = response.Data[cryptoCode].Quote["USD"].Price;

            return usdPrice;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}