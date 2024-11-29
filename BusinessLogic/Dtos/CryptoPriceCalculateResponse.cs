namespace BusinessLogic.Dtos;

public record CryptoPriceCalculateResponse
{
    public string? CryptoCode { get; set; }
    public Dictionary<string, decimal>? Rates { get; set; }
}
