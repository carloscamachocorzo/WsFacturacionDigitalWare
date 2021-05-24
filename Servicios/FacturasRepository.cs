using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Modelos;
using Servicios.Utilidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Servicios
{
    public class FacturasRepository : IFacturasRepository
    {
        private readonly string _connectionString;
        private readonly Logger _log = null;
        /// <summary>
        /// Constructor para inicializar la cadena de Conexion
        /// </summary>
        /// <param name="configuration"></param>
        public FacturasRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("FacturacionConnection");
        }
        /// <summary>
        ///  lista total de facturas
        /// </summary>
        /// <returns></returns>
        public async Task<List<Facturas>> GetAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllFacturas", sql))
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

        /// <summary>
        /// Asignacion de valores por campo
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
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
        /// Mapeo de campos del detalle
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private FacturasDetalle MapToFacturasDetalle(SqlDataReader reader)
        {
            return new FacturasDetalle()
            {
                IdProducto = (long)reader["IdProducto"],
                Valor = (decimal)reader["valor"],
                ValorIva = (decimal)reader["ValorIva"],

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
                using (SqlCommand cmd = new SqlCommand("GetFacturasById", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", Id));
                    Facturas response = null;
                    var respondeDetalle = new List<FacturasDetalle>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToFacturas(reader);
                        }
                        reader.NextResult();
                        while (await reader.ReadAsync())
                        {
                            respondeDetalle.Add(MapToFacturasDetalle(reader));
                        }
                        response.DetalleFactura = respondeDetalle;
                    }

                    return response;
                }
            }
        }

        /// <summary>
        /// Insertar nueva factura
        /// </summary>
        /// <param name="factura"></param>
        /// <returns></returns>
        public async Task<bool> Insert(Facturas factura)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("CrearFactura", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@NumeroFactura", factura.NumeroFactura));
                        cmd.Parameters.Add(new SqlParameter("@PrefijoFactura", factura.PrefijoFactura));
                        cmd.Parameters.Add(new SqlParameter("@Estado", factura.Estado));
                        cmd.Parameters.Add(new SqlParameter("@Valor", factura.Valor));
                        cmd.Parameters.Add(new SqlParameter("@ValorIva", factura.ValorIva));
                        cmd.Parameters.Add(new SqlParameter("@Fecha", factura.Fecha));
                        cmd.Parameters.Add(new SqlParameter("@IdCliente", factura.IdCliente));
                        var parametroDetalle = new SqlParameter("@ListaFactura", SqlDbType.Structured);

                        var tableDetalle = new DataTable();
                        tableDetalle.Columns.Add("IdProducto", typeof(int));
                        tableDetalle.Columns.Add("Valor", typeof(int));
                        tableDetalle.Columns.Add("ValorIva", typeof(int));

                        foreach (var itemfactura in factura.DetalleFactura)
                        {
                            var row = tableDetalle.NewRow();
                            row["IdProducto"] = itemfactura.IdProducto;
                            row["Valor"] = itemfactura.Valor;
                            row["ValorIva"] = itemfactura.ValorIva;
                            tableDetalle.Rows.Add(row);
                        }

                        parametroDetalle.Value = tableDetalle;
                        parametroDetalle.TypeName = "dbo.Type_FacturaDetalle";
                        cmd.Parameters.Add(parametroDetalle);                        
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Write(String.Format("{0} : {1}", ex.Message, ex.StackTrace));
                return false;
            }
        }
        /// <summary>
        /// Borrar una factura por el Id
        /// </summary>
        /// <param name="Id">Identificador del producto</param>
        /// <returns></returns>
        public async Task<bool> DeleteById(int Id)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteFacturasById", sql))
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
                    using (SqlCommand cmd = new SqlCommand("ModificarFacturaById", sql))
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
