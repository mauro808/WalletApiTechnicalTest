using WalletAPI.Domain.Entities;
using WalletApi.Application.Interfaces;
using WalletAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using WalletAPI.Application.DTOs;

namespace WalletAPI.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly ApplicationDbContext _context;

        public WalletService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet?> GetWalletByIdAsync(int id)
        {
            return await _context.Wallets.FindAsync(id);
        }

        public async Task<List<Wallet>> GetAllWalletsAsync()
        {
            return await _context.Wallets.ToListAsync();
        }

        public async Task<Wallet> CreateWalletAsync(Wallet wallet)
        {
            bool exists = await _context.Wallets.AnyAsync(w => w.DocumentId == wallet.DocumentId);
            if (exists)
            {
                throw new ArgumentException("El DocumentId ya está en uso.");
            }
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<bool> DeleteWalletAsync(int id)
        {
            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet == null) return false;

            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TransferBalanceAsync(int fromWalletId, int toWalletId, decimal amount)
        {
            var fromWallet = await _context.Wallets.FindAsync(fromWalletId);
            var toWallet = await _context.Wallets.FindAsync(toWalletId);

            if (fromWallet == null || toWallet == null)
                throw new Exception("Una de las billeteras no existe.");

            fromWallet.Withdraw(amount);
            toWallet.Deposit(amount);

            _context.Transactions.Add(new TransactionHistory(fromWallet.Id, amount, "Débito"));

            _context.Transactions.Add(new TransactionHistory(toWallet.Id, amount, "Crédito"));

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OperationResult> UpdateWalletAsync(int id, UpdateWalletDto walletDto)
        {
            var wallet = await _context.Wallets.FindAsync(id);
            bool exists = await _context.Wallets
                .AnyAsync(w => w.DocumentId == walletDto.DocumentId && w.Id != id);
            if (exists)
            {
                throw new ArgumentException("El DocumentId ya está en uso.");
            }
            if (wallet == null)
                return new OperationResult(false, "La billetera no existe.");

            if (walletDto.Balance.HasValue && walletDto.Balance.Value < 0)
                return new OperationResult(false, "El saldo no puede ser negativo.");
            if (!string.IsNullOrWhiteSpace(walletDto.DocumentId))
            {
                wallet.DocumentId = walletDto.DocumentId;
            }
            if (!string.IsNullOrWhiteSpace(walletDto.Name))
            {
                wallet.Name = walletDto.Name;
            }
            wallet.UpdateWallet(walletDto.DocumentId, walletDto.Name, walletDto.Balance);

            await _context.SaveChangesAsync();
            return new OperationResult(true, "Billetera actualizada con éxito.");
        }
    }
}
