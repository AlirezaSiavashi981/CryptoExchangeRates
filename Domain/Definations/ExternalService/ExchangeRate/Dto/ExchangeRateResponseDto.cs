namespace Domain.Definations.ExternalService.ExchangeRate.Dto;

public record ExchangeRateResponseDto
{
    /// <summary>
    ///  Returns true or false depending on whether or not your API request has succeeded.
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    ///  Returns the exact date and time (UNIX time stamp) the given rates were collected.
    /// </summary>
    public DateTime TimeStamp { get; set; }

    /// <summary>
    ///  Returns the three-letter currency code of the base currency used for this request.
    /// </summary>
    public string Base { get; set; }

    /// <summary>
    ///  Returns exchange rate data for the currencies you have requested.
    /// </summary>
    public Dictionary<string, decimal> Rates { get; set; }
}
