namespace Modelos
{
    /// <summary>
    /// Modelo de facturas detalle
    /// </summary>
    public class FacturasDetalle
    {
        public long IdProducto { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorIva { get; set; }

    }
}
