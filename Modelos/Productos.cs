namespace Modelos
{
    public class Productos
    {
        /// <summary>
        /// Nombre del Producto
        /// </summary>
        public string NombreProducto { get; set; }
        /// <summary>
        /// Existencias de Inventario del producto
        /// </summary>
        public decimal Existencias { get; set; }
        /// <summary>
        /// Valor total del producto
        /// </summary>
        public decimal Valor { get; set; }
        /// <summary>
        /// Valor de Iva del producto
        /// </summary>
        public int ValorIva { get; set; }        
        /// <summary>
        /// Categoria del producto
        /// </summary>
        public short IdCategoria { get; set; }
        /// <summary>
        /// Indica si el producto esta habilitado
        /// </summary>
        public bool Habilitado { get; set; }
    }
}
