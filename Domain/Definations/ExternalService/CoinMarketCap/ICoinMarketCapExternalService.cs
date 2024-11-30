namespace Domain.Definations.ExternalService.CoinMarketCap;

public interface ICoinMarketCapExternalService
{
    Task<decimal?> GetCryptoPriceInUSDAsync(string cryptoCode,CancellationToken ct);
}