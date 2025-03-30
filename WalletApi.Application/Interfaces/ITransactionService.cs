using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletAPI.Domain.Entities;

namespace WalletApi.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionHistory>?> GetTransactionsByWalletIdAsync(int walletId);
        Task<TransactionHistory> CreateTransactionAsync(TransactionHistory transaction);
    }
}
