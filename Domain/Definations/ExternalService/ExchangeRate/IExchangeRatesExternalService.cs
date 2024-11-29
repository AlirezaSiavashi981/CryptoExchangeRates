using Domain.Definations.ExternalService.ExchangeRate.Dto;

namespace Domain.Definations.ExternalService.ExchangeRate;

public interface IExchangeRatesExternalService
{
    Task<ExchangeRateResponseDto> GetExchangeRates(ExchangeRateRequestDto request, CancellationToken ct);
}
