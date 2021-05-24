using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Servicios.Utilidades
{
    public class Logger
    {
        public static IConfiguration _configuration;
        public Logger() { }

        /// <summary>
        /// Registrar Log Fisico
        /// </summary>
        /// <param name="msg">Detalle del error</param>
        public void Write(string msg)
        {
            try
            {
                if (!Directory.Exists(_configuration["LocalLogger:path"]))
                {
                    Directory.CreateDirectory(_configuration["LocalLogger:path"]);
                }

                string path = String.Format(@"{0}\log_wstopaz_{1}.log", _configuration["LocalLogger:path"], DateTime.Now.ToString("yyyyMMdd"));
                StreamWriter sw = new System.IO.StreamWriter(path, true);
                sw.WriteLine(String.Format("{0} : {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg));
                sw.Close();
            }
            catch (IOException) { }
            catch (Exception) { throw; }
        }
    }
}
