using BusinessLogic.Dtos;
using BusinessLogic.Services;
using Domain.Definations.ExternalService.CoinMarketCap;
using Domain.Definations.ExternalService.ExchangeRate;
using Moq;

namespace Test.CryptoQuoteServiceTests;

public class CryptoQuoteServiceTests
{
    [Fact]
    public async Task GetCryptoQuoteAsync_Returns_Valid_Quotes()
    {
        // Arrange : mock data 
        var cryptoCode = "BTC";
        var usdPrice = 50000m;
        var exchangeRates = new Dictionary<string, decimal>
        {
            { "USD", 1m },
            { "EUR", 0.85m },
            { "BRL", 5.25m },
            { "GBP", 0.75m },
            { "AUD", 1.35m }
        };
        var expectedRates = new Dictionary<string, decimal>
        {
            { "USD", 50000m },
            { "EUR", 42500m },
            { "BRL", 262500m },
            { "GBP", 37500m },
            { "AUD", 67500m }
        };

        var requestDto = new CryptoQuoteRequestDto { CryptoCode = cryptoCode };

        var coinMarketCapServiceMock = new Mock<ICoinMarketCapExternalService>();
        coinMarketCapServiceMock
            .Setup(service => service.GetCryptoPriceInUSDAsync(cryptoCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(usdPrice);

        var exchangeRatesServiceMock = new Mock<IExchangeRatesExternalService>();
        exchangeRatesServiceMock
            .Setup(service => service.GetConvertedPricesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRates);

        var service = new CryptoPriceCalculateService(
            exchangeRatesServiceMock.Object,
            coinMarketCapServiceMock.Object
        );

        // Act
        var result = await service.GetCryptoQuoteAsync(requestDto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cryptoCode, result.CryptoCode);
        Assert.Equal(expectedRates, result.Rates);

        coinMarketCapServiceMock.Verify(
            service => service.GetCryptoPriceInUSDAsync(cryptoCode, It.IsAny<CancellationToken>()), 
            Times.Once);

        exchangeRatesServiceMock.Verify(
            service => service.GetConvertedPricesAsync(It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}