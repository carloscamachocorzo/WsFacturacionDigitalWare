using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Servicios
{
    class ProductosRepository
    {
        private readonly string _connectionString;

        public ProductosRepository(IConfiguration configuration)
        {            
            _connectionString = configuration.GetConnectionString("FacturacionConnection");
        }
    }
}
