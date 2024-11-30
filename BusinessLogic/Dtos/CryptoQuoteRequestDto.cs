namespace BusinessLogic.Dtos;

public record CryptoQuoteRequestDto
{
    public required string CryptoCode { get; set; }
}