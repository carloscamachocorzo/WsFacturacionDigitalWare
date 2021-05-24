using Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicios
{
    /// <summary>
    /// Interfaz de las acciones / operaciones a realizar en Facturas
    /// </summary>
    public interface IFacturasRepository
    {
        Task<List<Facturas>> GetAll();

        Task<Facturas> GetById(int Id);

        Task<bool> Insert(Facturas producto);

        Task<bool> DeleteById(int Id);

        Task<bool> Update(Facturas producto, int id);
    }
}
