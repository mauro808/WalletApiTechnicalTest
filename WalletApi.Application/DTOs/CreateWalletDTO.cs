using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WalletApi.Application.DTOs
{
    public class CreateWalletDto
    {
        [Required]
        [JsonProperty("documentId")]
        [DefaultValue("12345678")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El DocumentId solo puede contener números.")]
        public string DocumentId { get; set; } = string.Empty;

        [Required]
        [JsonProperty("name")]
        [DefaultValue("John Doe")]
        public string Name { get; set; } = string.Empty;
    }
}
