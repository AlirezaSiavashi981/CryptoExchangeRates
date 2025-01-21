using BusinessLogic.Dtos;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    [Route("api/crypto")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly CryptoPriceCalculateService _calculateService;

        public CryptoController(CryptoPriceCalculateService calculateService)
        {
            _calculateService = calculateService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CryptoQuote([FromBody] CryptoQuoteModel model, CancellationToken ct)
        {
            var result = await _calculateService.GetCryptoQuoteAsync(new CryptoQuoteRequestDto
            {
                CryptoCode = model.CryptoCode
            }, ct);
            if (result == null)
                return NotFound("sorry : Cryptocurrency not found.");

            return Ok(result);
        }
    }
}