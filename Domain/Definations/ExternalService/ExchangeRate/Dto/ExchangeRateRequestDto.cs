namespace Domain.Definations.ExternalService.ExchangeRate.Dto;

public record ExchangeRateRequestDto
{
    /// <summary>
    /// Returns the three-letter currency code of the base currency used for this request.
    /// </summary>
    public decimal Base { get; set; }

    /// <summary>
    /// Returns exchange rate data for the currencies you have requested.
    /// </summary>
    public IEnumerable<string> Symbols { get; set; }

}
