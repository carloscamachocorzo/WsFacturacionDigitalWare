using Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicios
{
    /// <summary>
    /// Interfaz de las acciones / operaciones a realizar en productos
    /// </summary>
    public interface IProductosRepository
    {
        Task<List<Productos>> GetAll();

        Task<Productos> GetById(int Id);

        Task<bool> Insert(Productos producto);

        Task<bool> DeleteById(int Id);

        Task<bool> Update(Productos producto, int id);
    }
}
