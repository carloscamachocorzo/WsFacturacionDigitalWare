using System;
using System.Collections.Generic;
using System.Text;

namespace Modelos
{
    class Productos
    {
        /// <summary>
        /// Nombre del Producto
        /// </summary>
        public string NombreProducto { get; set; }
        /// <summary>
        /// Existencias de Inventario del producto
        /// </summary>
        public int Existencias { get; set; }
        /// <summary>
        /// Valor total del producto
        /// </summary>
        public int Valor { get; set; }
        /// <summary>
        /// Valor de Iva del producto
        /// </summary>
        public int ValorIva { get; set; }
        /// <summary>
        /// Producto Habilidado
        /// </summary>
        public bool Habilitado { get; set; }
        /// <summary>
        /// Categoria del producto
        /// </summary>
        public int IdCategoria { get; set; }
    }
}
