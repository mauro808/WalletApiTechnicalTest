using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApi.Application.DTOs
{
    public class CreateWalletDtoExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(CreateWalletDto))
            {
                schema.Example = new Microsoft.OpenApi.Any.OpenApiObject
                {
                    ["documentId"] = new Microsoft.OpenApi.Any.OpenApiString("12345678"),
                    ["name"] = new Microsoft.OpenApi.Any.OpenApiString("Juan Pérez")
                };
            }
        }
    }
}
