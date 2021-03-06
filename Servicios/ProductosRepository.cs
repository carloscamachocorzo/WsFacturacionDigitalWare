using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Modelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicios
{
    public class ProductosRepository : IProductosRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor para inicializar la cadena de Conexion
        /// </summary>
        /// <param name="configuration"></param>
        public ProductosRepository(IConfiguration configuration)
        {            
            _connectionString = configuration.GetConnectionString("FacturacionConnection");
        }
        /// <summary>
        /// Lista de todos productos Habilitados
        /// </summary>
        /// <returns></returns>
        public async Task<List<Productos>> GetAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllProductos", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Productos>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToProductos(reader));
                        }
                    }

                    return response;
                }
            }
        }
        private Productos MapToProductos(SqlDataReader reader)
        {
            return new Productos()
            {
                IdCategoria = (short)reader["IdCategoria"],
                NombreProducto = reader["NombreProducto"].ToString(),
                Existencias = (decimal)reader["Existencias"],
                Valor = (decimal)reader["Valor"],
            };
        }

        /// <summary>
        /// Consultar Producto por Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Productos> GetById(int Id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetProductosById", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", Id));
                    Productos response = null;
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToProductos(reader);
                        }
                    }

                    return response;
                }
            }
        }

        /// <summary>
        /// Insertar nuevo productos
        /// </summary>
        /// <param name="producto"></param>
        /// <returns></returns>
        public async Task <bool> Insert(Productos producto)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("CrearProducto", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@NombreProducto", producto.NombreProducto));
                        cmd.Parameters.Add(new SqlParameter("@Existencias", producto.Existencias));
                        cmd.Parameters.Add(new SqlParameter("@Valor", producto.Valor));
                        cmd.Parameters.Add(new SqlParameter("@ValorIva", producto.ValorIva));
                        cmd.Parameters.Add(new SqlParameter("@IdCategoria", producto.IdCategoria));
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Borrar un producto por el Id
        /// </summary>
        /// <param name="Id">Identificador del producto</param>
        /// <returns></returns>
        public async Task <bool> DeleteById(int Id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteProductosbyId", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", Id));
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Modificar producto por Id
        /// </summary>
        /// <param name="producto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Update(Productos producto, int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("ModificarProductoById", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));
                        cmd.Parameters.Add(new SqlParameter("@NombreProducto", producto.NombreProducto));
                        cmd.Parameters.Add(new SqlParameter("@Existencias", producto.Existencias));
                        cmd.Parameters.Add(new SqlParameter("@Valor", producto.Valor));
                        cmd.Parameters.Add(new SqlParameter("@ValorIva", producto.ValorIva));
                        cmd.Parameters.Add(new SqlParameter("@IdCategoria", producto.IdCategoria));
                        cmd.Parameters.Add(new SqlParameter("@Habilitado", producto.Habilitado));
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return true;
                    }
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
