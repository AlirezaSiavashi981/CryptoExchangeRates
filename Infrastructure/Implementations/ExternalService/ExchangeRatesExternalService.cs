using Domain.Definations.ExternalService;
using Domain.Definations.ExternalService.Dto;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Infrastructure.Implementations.ExternalService;

public class ExchangeRatesExternalService : IExchangeRatesExternalService
{
    private readonly IHttpClientFactory _httpClient;
    private readonly ILogger<ExchangeRatesExternalService> _logger;

    public ExchangeRatesExternalService(IHttpClientFactory httpClient, ILogger<ExchangeRatesExternalService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    #region Config

    /// <summary>
    /// In some cases, a safer and better way is to make the api call configurations secret in appsettings.
    /// </summary>
    private string BaseUrl = "https://api.exchangeratesapi.io";
    private string Token = "03f0e3b7-22d1-42ed-97c5-77cfe33d9eac";

    #endregion

    public async Task<ExchangeRateResponseDto> GetExchangeRates(ExchangeRateRequestDto request, CancellationToken ct)
    {
        var httpClient = _httpClient.CreateClient();

        // authenticate
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");

        var jserializeData = JsonSerializer.Serialize(new
        {
            Base = request.Base,
            symbols = request.Symbols,
        });

        var uri = $"{BaseUrl}/v1/latest";

        try
        {
            var httpResponse = await httpClient
                .PostAsync(uri, new StringContent(jserializeData, Encoding.UTF8, "application/json"), ct);

            var result = await httpResponse.Content.ReadAsStringAsync(ct);
            if (!httpResponse.IsSuccessStatusCode)
            {
                _logger.LogError($"Status Code : {httpResponse.StatusCode}");

                return new ExchangeRateResponseDto
                {
                    Base = null,
                    Rates = null,
                    Status = false,
                    TimeStamp = DateTime.Now,
                };
            }

            var response = JsonSerializer.Deserialize<ExchangeRateResponseDto>(result);
            if (response == null)
                return new ExchangeRateResponseDto
                {
                    Base = null,
                    Status = false,
                    Rates = null,
                    TimeStamp = DateTime.Now,
                };


            return new ExchangeRateResponseDto
            {
                Base = response.Base,
                Status = response.Status,
                Rates = response.Rates,
                TimeStamp = response.TimeStamp,
            };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error Message : {e.Message} & Exception : {e.InnerException}");

            return new ExchangeRateResponseDto
            {
                Base = null,
                Status = false,
                Rates = null,
                TimeStamp = DateTime.Now,
            };
        }

    }
}
