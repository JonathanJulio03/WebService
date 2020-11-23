using ClassLibrary.Commons;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public static class HelperWs
    {
        public static string getMessageException(Exception e) 
        {
            if (!e.Message.Contains("--read-only"))
            {
                string message = (e.InnerException != null ? e.InnerException.InnerException != null ? e.InnerException.InnerException.Message : e.InnerException.Message : e.Message);
                message = message.Replace('\\', '/');
                return new Regex("[^a-zA-Z0-9_.-=]+").Replace(message, " ");
            }
            else
                return "Oops! Lo sentimos algo salió mal. Vuelve a intentarlo.";

        }
        /// <summary>
        /// Define la ruta del clientControl con y sin internet
        /// </summary>
        /// <returns></returns>
        public static string FoundRouteClientControlMySql()
        {
            if (CheckForInternetConnection())
            {
                GlobalVariables.ConexionFactElectMySql = ConfigurationManager.ConnectionStrings["DBFactElect_Web"].ConnectionString;
                return System.Configuration.ConfigurationManager.ConnectionStrings["DBClientControl_Web"].ConnectionString;
            }
            else
            {
                GlobalVariables.ConexionFactElectMySql = "server=" + Constants.server + ";user id=" + Constants.userId + ";password=" + Constants.password + ";persist security info=True;database=" + Constants.databaseFel;
                return "server=" + Constants.server + ";user id=" + Constants.userId + ";password=" + Constants.password + ";persist security info=True;database=" + Constants.database;
            }
        }

        /// <summary>
        /// Verifica si existe conexion a internet
        /// </summary>
        /// <returns></returns>
        public static bool CheckForInternetConnection()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["onLine"].ToString());
        }
    }
}
