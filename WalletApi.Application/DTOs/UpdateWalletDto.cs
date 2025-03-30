using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletAPI.Application.DTOs
{
    public class UpdateWalletDto
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "El DocumentId solo puede contener números.")]
        [StringLength(15, ErrorMessage = "El DocumentId no puede tener más de 15 caracteres.")]
        public string? DocumentId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre no puede estar vacío.")]
        public string? Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El balance debe ser un número positivo.")]
        public decimal? Balance { get; set; }
    }
}
