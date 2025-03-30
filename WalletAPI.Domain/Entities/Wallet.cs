using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletAPI.Domain.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public string DocumentId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; private set; } = 0;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public ICollection<TransactionHistory> Transactions { get; set; } = new List<TransactionHistory>();

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("El monto debe ser mayor a 0.");

            Balance += amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("El monto debe ser mayor a 0.");

            if (Balance < amount)
                throw new InvalidOperationException("Fondos insuficientes.");

            Balance -= amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateWallet(string? documentId, string? name, decimal? balance)
        {
            DocumentId = documentId ?? DocumentId;
            Name = name ?? Name;
            if (balance.HasValue && balance.Value >= 0)
            {
                Balance = balance.Value;
            }
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
