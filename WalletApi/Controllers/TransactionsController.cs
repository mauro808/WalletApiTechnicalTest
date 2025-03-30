using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApi.Application.Interfaces;
using WalletAPI.Application.DTOs;
using WalletAPI.Application.Services;
using WalletAPI.Domain.Entities;

namespace WalletApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IWalletService _walletService;

        public TransactionsController(ITransactionService transactionService, IWalletService walletService)
        {
            _transactionService = transactionService;
            _walletService = walletService;
        }

        [HttpGet("{walletId}")]
        public async Task<ActionResult<IEnumerable<TransactionHistory>>> GetTransactions(int walletId)
        {
            var transactions = await _transactionService.GetTransactionsByWalletIdAsync(walletId);

            if (transactions == null)
                return NotFound(new { message = "La billetera especificada no existe." });

            if (!transactions.Any())
                return NotFound(new { message = "No se encontraron transacciones para esta billetera." });

            return Ok(transactions);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateTransaction([FromBody] CreateTransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new { message = "Solicitud incorrecta", errors });
            }
            var fromWallet = await _walletService.GetWalletByIdAsync(transactionDto.WalletId);
            var toWallet = await _walletService.GetWalletByIdAsync(transactionDto.ToWalletId);

            if (fromWallet == null || toWallet == null)
            {
                return NotFound(new { message = "Una de las billeteras especificadas no existe." });
            }
            if(fromWallet.Id == toWallet.Id)
            {
                return BadRequest(new { message = "No puedes enviar dinero a tu misma cuenta." });
            }
            if (fromWallet.Balance < transactionDto.Amount)
            {
                return BadRequest(new { message = "Saldo insuficiente en la billetera de origen." });
            }
            var debitTransaction = new TransactionHistory(fromWallet.Id, transactionDto.Amount, "Débito");
            fromWallet.Withdraw(transactionDto.Amount);
            var creditTransaction = new TransactionHistory(toWallet.Id, transactionDto.Amount, "Crédito");
            toWallet.Deposit(transactionDto.Amount);
            await _transactionService.CreateTransactionAsync(debitTransaction);
            await _transactionService.CreateTransactionAsync(creditTransaction);
            await _walletService.UpdateWalletAsync(fromWallet.Id, new UpdateWalletDto
            {
                DocumentId = fromWallet.DocumentId,
                Name = fromWallet.Name,
                Balance = fromWallet.Balance
            });
            await _walletService.UpdateWalletAsync(toWallet.Id, new UpdateWalletDto
            {
                DocumentId = toWallet.DocumentId,
                Name = toWallet.Name,
                Balance = toWallet.Balance
            });
            return Ok(new { message = "Transferencia realizada con éxito." });
        }
    }
}
