using System;

namespace Modelos
{
    public class Facturas
    {
        public decimal NumeroFactura { get; set; }
        public string PrefijoFactura { get; set; }
        public string Estado { get; set; }
        public decimal Valor { get; set; }

        public decimal ValorIva { get; set; }

        public DateTime Fecha { get; set; }

        public long IdCliente { get; set; }
    }
}
