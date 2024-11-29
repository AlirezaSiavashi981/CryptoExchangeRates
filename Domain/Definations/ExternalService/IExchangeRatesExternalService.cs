using Domain.Definations.ExternalService.Dto;

namespace Domain.Definations.ExternalService;

public interface IExchangeRatesExternalService
{
    Task<ExchangeRateResponseDto> GetExchangeRates(ExchangeRateRequestDto request, CancellationToken ct);
}
