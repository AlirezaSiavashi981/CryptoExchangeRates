namespace Domain.Definations.ExternalService.CoinMarketCap.Dtos;

public class CryptoResponse
{
    public Dictionary<string, CryptoData> Data { get; set; }
}

public class CryptoData
{
    public Dictionary<string, QuoteData> Quote { get; set; }
}

public class QuoteData
{
    public decimal Price { get; set; }
}
