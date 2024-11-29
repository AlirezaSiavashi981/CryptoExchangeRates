namespace Domain.Definations.ExternalService.Dto;

public record ExchangeRateRequestDto
{
    /// <summary>
    /// Returns the three-letter currency code of the base currency used for this request.
    /// </summary>
    public string? Base { get; set; }

    /// <summary>
    /// Returns exchange rate data for the currencies you have requested.
    /// </summary>
    public List<string>? Symbols { get; set; }

}
