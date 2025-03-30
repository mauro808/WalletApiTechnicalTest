using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WalletApi.Application.DTOs;
using WalletApi.Application.Interfaces;
using WalletAPI.Application.DTOs;
using WalletAPI.Domain.Entities;

namespace WalletApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wallet>>> GetWallets()
        {
            var wallets = await _walletService.GetAllWalletsAsync();
            return Ok(wallets);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Wallet>> GetWallet(int id)
        {
            var wallet = await _walletService.GetWalletByIdAsync(id);
            if (wallet == null)
                return NotFound("Billetera no encontrada");

            return Ok(wallet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateWallet([FromBody] CreateWalletDto walletDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { message = "Solicitud incorrecta", errors });
            }

            try
            {
                var wallet = await _walletService.CreateWalletAsync(new Wallet
                {
                    DocumentId = walletDto.DocumentId,
                    Name = walletDto.Name
                });

                return CreatedAtAction(nameof(GetWallet), new { id = wallet.Id }, wallet);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWallet(int id)
        {
            var deleted = await _walletService.DeleteWalletAsync(id);
            if (!deleted)
                return NotFound("Billetera no encontrada");

            return Ok(new { message = "Billetera eliminada exitosamente" });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWallet(int id, [FromBody] UpdateWalletDto walletDto)
        {
            var allPropertiesNull = new List<bool>
             {
                 string.IsNullOrWhiteSpace(walletDto.DocumentId),
                 string.IsNullOrWhiteSpace(walletDto.Name),
                 !walletDto.Balance.HasValue
             }.All(p => p);
            if (allPropertiesNull)
                return BadRequest(new { message = "Debe proporcionar al menos un campo válido para actualizar." });
            if (!decimal.TryParse(walletDto.Balance?.ToString(), out var balance) && walletDto.Balance.HasValue)
            {
                return BadRequest(new { message = "El balance debe ser un número válido." });
            }
            var result = await _walletService.UpdateWalletAsync(id, walletDto);
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
