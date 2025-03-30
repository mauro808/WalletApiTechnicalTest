using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletAPI.Application.DTOs
{
    public class CreateTransactionDto
    {
        [Required(ErrorMessage = "El WalletId es obligatorio.")]
        public int WalletId { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "El ToWalletId es obligatorio.")]
        public int ToWalletId { get; set; }
    }
}
