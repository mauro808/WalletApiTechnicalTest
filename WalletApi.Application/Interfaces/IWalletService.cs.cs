using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletAPI.Application.DTOs;
using WalletAPI.Domain.Entities;

namespace WalletApi.Application.Interfaces
{
    public interface IWalletService
    {
        Task<Wallet?> GetWalletByIdAsync(int id);
        Task<List<Wallet>> GetAllWalletsAsync();
        Task<Wallet> CreateWalletAsync(Wallet wallet);
        Task<bool> DeleteWalletAsync(int id);
        Task<bool> TransferBalanceAsync(int fromWalletId, int toWalletId, decimal amount);
        Task<OperationResult> UpdateWalletAsync(int id, UpdateWalletDto walletDto);
    }
}
