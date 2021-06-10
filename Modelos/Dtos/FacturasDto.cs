using System;
using System.Collections.Generic;
using System.Text;

namespace Modelos.Dtos
{
    class FacturasDto
    {
        public decimal NumeroFactura { get; set; }
        public string PrefijoFactura { get; set; }
        public string Estado { get; set; }
        public decimal Valor { get; set; }
    }
}
