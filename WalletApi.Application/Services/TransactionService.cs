using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApi.Application.Interfaces;
using WalletAPI.Domain.Entities;
using WalletAPI.Infrastructure.Persistence;

namespace WalletAPI.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionHistory> CreateTransactionAsync(TransactionHistory transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<List<TransactionHistory>?> GetTransactionsByWalletIdAsync(int walletId)
        {
            var walletExists = await _context.Wallets.AnyAsync(w => w.Id == walletId);

            if (!walletExists)
                return null;

            return await _context.Transactions
                .Where(t => t.WalletId == walletId)
                .ToListAsync();
        }
    }
}
