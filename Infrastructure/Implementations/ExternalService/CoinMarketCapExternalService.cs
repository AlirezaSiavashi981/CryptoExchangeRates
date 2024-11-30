using System.Net.Http.Json;
using System.Text.Json;
using Domain.Definations.ExternalService.CoinMarketCap;
using Domain.Definations.ExternalService.CoinMarketCap.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Implementations.ExternalService;

public class CoinMarketCapExternalService(
    IHttpClientFactory client,
    IConfiguration config,
    ILogger<CoinMarketCapExternalService> logger)
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
            var httpResponse = await httpClient.GetFromJsonAsync<CryptoResponse>(uri, cancellationToken: ct);


            if (httpResponse == null || httpResponse.Data == null || !httpResponse.Data.ContainsKey(cryptoCode))
            {
                throw new Exception("The data result is empty or invalid.");
            }

            var usdPrice = httpResponse!.Data[cryptoCode].Quote["USD"].Price;

            return usdPrice;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}