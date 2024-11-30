using BusinessLogic.Dtos;
using Domain.Definations.ExternalService.CoinMarketCap;
using Domain.Definations.ExternalService.ExchangeRate;

namespace BusinessLogic.Services;

public class CryptoPriceCalculateService(
    IExchangeRatesExternalService exchangeRatesExternalService,
    ICoinMarketCapExternalService coinMarketCapExternalService)
{
    public async Task<CryptoQuoteResponseDto> GetCryptoQuoteAsync(CryptoQuoteRequestDto request, CancellationToken ct)
    {
        var usdPrice = await coinMarketCapExternalService.GetCryptoPriceInUSDAsync(request.CryptoCode, ct);
        if (usdPrice is null) return null;

        var exchangeRates = await exchangeRatesExternalService.GetConvertedPricesAsync(ct);

        var selectedCurrencies = new[] { "USD", "EUR", "BRL", "GBP", "AUD" };
        var rates = selectedCurrencies
            .ToDictionary(currency => currency, currency => exchangeRates[currency] * usdPrice.Value);

        return new CryptoQuoteResponseDto
        {
            CryptoCode = request.CryptoCode,
            Rates = rates
        };
    }
}