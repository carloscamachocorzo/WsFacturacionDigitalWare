using Microsoft.AspNetCore.Mvc;
using Modelos;
using Modelos.Enum;
using Servicios;
using Servicios.Utilidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WsFacturacionDigitalWare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly IFacturasRepository _repository;
        private readonly Logger _log = null;
        string msg = string.Empty;
        int errorCode = -1;

        public FacturasController(IFacturasRepository repository)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/<ProductosController>
        [HttpGet]
        public async Task<ActionResult<List<Facturas>>> Get()
        {
            return await _repository.GetAll();
        }

        // GET api/<ProductosController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Facturas>> Get(int id)
        {
            var response = await _repository.GetById(id);
            if (response == null) { return NotFound(); }
            return response;
        }

        // POST api/<ProductosController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Facturas factura)
        {


            try
            {
                bool response = await _repository.Insert(factura);
                if (response)
                {
                    msg = "exitoso";
                    errorCode = (int)GeneralErrorCodes.SUCCESS;
                    return Ok(new GeneralResponse<object>(new { Nuevo_productos = "Creado correctamente" }, msg, errorCode));
                }
                else
                {
                    msg = "Error";
                    errorCode = (int)GeneralErrorCodes.UNCONTROLLED;
                    return BadRequest(new GeneralResponse<object>(new { Nuevo_productos = "Error" }, msg, errorCode));

                }

            }
            catch (Exception ex)
            {
                _log.Write(String.Format("{0} : {1}", ex.Message, ex.StackTrace));
                return BadRequest(new GeneralResponse<Object>(string.Empty, ex.Message, (int)GeneralErrorCodes.UNCONTROLLED));
            }
        }

        // PUT api/<ProductosController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Facturas factura)
        {


            try
            {
                bool response = await _repository.Update(factura, id);
                if (response)
                {
                    msg = "exitoso";
                    errorCode = (int)GeneralErrorCodes.SUCCESS;
                    return Ok(new GeneralResponse<object>(new { Nuevo_productos = "Modificado correctamente" }, msg, errorCode));
                }
                else
                {
                    msg = "Error";
                    errorCode = (int)GeneralErrorCodes.UNCONTROLLED;
                    return BadRequest(new GeneralResponse<object>(new { Nuevo_productos = "Error" }, msg, errorCode));

                }

            }
            catch (Exception ex)
            {
                _log.Write(String.Format("{0} : {1}", ex.Message, ex.StackTrace));
                return BadRequest(new GeneralResponse<Object>(string.Empty, ex.Message, (int)GeneralErrorCodes.UNCONTROLLED));
            }
        }

        // DELETE api/<ProductosController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                bool response = await _repository.DeleteById(id);
                if (response)
                {
                    msg = "exitoso";
                    errorCode = (int)GeneralErrorCodes.SUCCESS;
                    return Ok(new GeneralResponse<object>(new { Nuevo_productos = "Eliminado correctamente" }, msg, errorCode));
                }
                else
                {
                    msg = "Error";
                    errorCode = (int)GeneralErrorCodes.UNCONTROLLED;
                    return BadRequest(new GeneralResponse<object>(new { Nuevo_productos = "Error" }, msg, errorCode));

                }

            }
            catch (Exception ex)
            {
                _log.Write(String.Format("{0} : {1}", ex.Message, ex.StackTrace));
                return BadRequest(new GeneralResponse<Object>(string.Empty, ex.Message, (int)GeneralErrorCodes.UNCONTROLLED));
            }
        }
    }
}
