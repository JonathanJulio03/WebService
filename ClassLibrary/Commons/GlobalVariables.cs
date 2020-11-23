using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Commons
{
    public class GlobalVariables
    {
        public static string Mensaje { get; set; }

        public static string MensajeFEL { get; set; }

        public static string ErrorDB { get; set; }

        public static string UserId { get; set; }

        public static string ConexionDbImages { get; set; }

        public static string ConexionMySql { get; set; }

        public static string ConexionFactElectMySql { get; set; }

        public static string RutaWS { get; set; }

        public static string RutaWSTNS { get; set; }
    }
    public class JsonDataResultMask
    {
        [JsonProperty("success")]
        public bool success { get; set; }

        [JsonProperty("response")]
        public object response { get; set; }

        [JsonProperty("kill_sesion")]
        public string kill_sesion { get; set; }
    }

    public class JsonDataResult
    {
        private bool success;
        private object contenido;


        public JsonDataResult()
        {
            this.success = true;
            this.contenido = "";
        }

        public JsonDataResult(bool s, object c)
        {
            this.success = s;
            this.contenido = c;
        }

        public bool SUCCESS
        {
            get { return this.success; }
            set { this.success = value; }
        }
        public object CONTENIDO
        {
            get { return this.contenido; }
            set { this.contenido = value; }
        }
    }
}
