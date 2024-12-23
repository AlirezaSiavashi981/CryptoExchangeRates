﻿namespace Domain.Definations.ExternalService.ExchangeRate;

public interface IExchangeRatesExternalService
{
    Task<Dictionary<string, decimal>> GetConvertedPricesAsync(CancellationToken ct);
}
