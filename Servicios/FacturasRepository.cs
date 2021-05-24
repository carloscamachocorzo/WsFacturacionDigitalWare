using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Modelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicios
{
    public class FacturasRepository : IFacturasRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor para inicializar la cadena de Conexion
        /// </summary>
        /// <param name="configuration"></param>
        public FacturasRepository(IConfiguration configuration)
        {            
            _connectionString = configuration.GetConnectionString("FacturacionConnection");
        }
        /// <summary>
        /// Lista de todos facturas Habilitados
        /// </summary>
        /// <returns></returns>
        public async Task<List<Facturas>> GetAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllProductos", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Facturas>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToFacturas(reader));
                        }
                    }

                    return response;
                }
            }
        }
        private Facturas MapToFacturas(SqlDataReader reader)
        {
            return new Facturas()
            {
                NumeroFactura = (decimal)reader["NumeroFactura"],
                PrefijoFactura = reader["PrefijoFactura"].ToString(),
                Estado = reader["Estado"].ToString(),
                Valor = (decimal)reader["Valor"],
                ValorIva = (decimal)reader["ValorIva"],
                Fecha = Convert.ToDateTime(reader["Fecha"]),
                IdCliente = (long)reader["IdCliente"],
            };
        }

        /// <summary>
        /// Consultar factura por Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Facturas> GetById(int Id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetProductosById", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", Id));
                    Facturas response = null;
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToFacturas(reader);
                        }
                    }

                    return response;
                }
            }
        }

        /// <summary>
        /// Insertar nueva factura
        /// </summary>
        /// <param name="producto"></param>
        /// <returns></returns>
        public async Task <bool> Insert(Facturas factura)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("CrearProducto", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@NumeroFactura", factura.NumeroFactura));
                        cmd.Parameters.Add(new SqlParameter("@PrefijoFactura", factura.PrefijoFactura));
                        cmd.Parameters.Add(new SqlParameter("@Estado", factura.Estado));
                        cmd.Parameters.Add(new SqlParameter("@Valor", factura.Valor));
                        cmd.Parameters.Add(new SqlParameter("@ValorIva", factura.ValorIva));
                        cmd.Parameters.Add(new SqlParameter("@Fecha", factura.Fecha));
                        cmd.Parameters.Add(new SqlParameter("@IdCliente", factura.IdCliente));
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
        /// Borrar una factura por el Id
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
        /// Modificar facturas por Id
        /// </summary>
        /// <param name="factura"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Update(Facturas factura, int id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("ModificarProductoById", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));                        
                        cmd.Parameters.Add(new SqlParameter("@NumeroFactura", factura.NumeroFactura));
                        cmd.Parameters.Add(new SqlParameter("@PrefijoFactura", factura.PrefijoFactura));
                        cmd.Parameters.Add(new SqlParameter("@Estado", factura.Estado));
                        cmd.Parameters.Add(new SqlParameter("@Valor", factura.Valor));
                        cmd.Parameters.Add(new SqlParameter("@ValorIva", factura.ValorIva));
                        cmd.Parameters.Add(new SqlParameter("@Fecha", factura.Fecha));
                        cmd.Parameters.Add(new SqlParameter("@IdCliente", factura.IdCliente));
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
