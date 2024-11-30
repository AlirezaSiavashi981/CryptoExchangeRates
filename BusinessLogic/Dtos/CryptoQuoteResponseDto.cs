namespace BusinessLogic.Dtos;

public record CryptoQuoteResponseDto
{
    public string CryptoCode { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }
}