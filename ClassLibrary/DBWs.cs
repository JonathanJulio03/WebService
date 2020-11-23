using ClassLibrary.Commons;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace ClassLibrary
{
    public class DBWs
    {

        /// <summary>
        /// Retorna un datatable, recibe el sql a ejecutar
        /// </summary>
        /// <param name="pSelect"></param>
        /// <returns></returns>
        public static JsonDataResult TNS_QTConsulta(string Sql, string ConexionString = "")
        {
            JsonDataResult JsonResult = new JsonDataResult() { CONTENIDO = null };
            ConexionString = string.IsNullOrEmpty(ConexionString) ? GlobalVariables.ConexionMySql : ConexionString;

            MySqlConnection conexion = new MySqlConnection(ConexionString);
            MySqlCommand command = new MySqlCommand(Sql, conexion);

            try
            {
                conexion.Open();
                DataTable vDataTable = new DataTable();
                vDataTable.Load(command.ExecuteReader());

                JsonResult.CONTENIDO = JsonConvert.SerializeObject(vDataTable);
                JsonResult.SUCCESS = true;
            }
            catch (Exception e)
            {
                JsonResult.CONTENIDO = HelperWs.getMessageException(e);
                JsonResult.SUCCESS = false;
            }
            finally
            {
                conexion.Close();
                conexion.Dispose();
            }
            return JsonResult;
        }
        /// <summary>
        /// Retorna un datatable, recibe el sql a ejecutar
        /// </summary>
        /// <param name="pSelect"></param>
        /// <returns></returns>
        public static JsonDataResult TNS_QTConsulta(string Sql, List<object> parametros, string ConexionString = "")
        {
            JsonDataResult JsonResult = new JsonDataResult() { CONTENIDO = null };
            ConexionString = string.IsNullOrEmpty(ConexionString) ? GlobalVariables.ConexionMySql : ConexionString;

            MySqlConnection conexion = new MySqlConnection(ConexionString);
            MySqlCommand command = new MySqlCommand(Sql, conexion);

            try
            {
                int i = 0;
                foreach (object value in parametros)
                {
                    string param = string.Concat("@param", i);
                    command.Parameters.AddWithValue(param, value);
                    i++;
                }
                conexion.Open();
                DataTable vDataTable = new DataTable();
                vDataTable.Load(command.ExecuteReader());
                JsonResult.CONTENIDO = JsonConvert.SerializeObject(vDataTable);
                JsonResult.SUCCESS = true;
            }
            catch (Exception e)
            {
                JsonResult.CONTENIDO = HelperWs.getMessageException(e);
                JsonResult.SUCCESS = false;
            }
            finally
            {
                conexion.Close();
                conexion.Dispose();
            }
            return JsonResult;
        }
        /// <summary>
        /// Valida si aurora invirtio o no las rds de lectura y escritura
        /// </summary>
        private static void TNS_ValidarSwitchRds()
        {
            string sql = "SHOW GLOBAL VARIABLES LIKE 'innodb_read_only'";
            string connection_write = ConfigurationManager.ConnectionStrings["DBClientControl_Web"].ConnectionString;

            JsonDataResult json = TNS_QTConsulta(sql, new List<object>(), connection_write);
            DataTable data_write = (DataTable)JsonConvert.DeserializeObject(json.CONTENIDO.ToString(), typeof(DataTable));

            if (data_write.Rows[0]["Value"].ToString().Equals("ON"))
            {
                bool switchOn = Convert.ToBoolean(ConfigurationManager.AppSettings["switchOn"].ToString());
                if (!switchOn)
                {
                    string connection_read = ConfigurationManager.ConnectionStrings["DBClientControl_Web2"].ConnectionString;
                    sql = string.Format("CALL TNS_CC_SWITCH_RDS({0})", true);
                    TNS_QTConsulta(sql, connection_read);
                    ConfigurationManager.AppSettings.Set("switchOn", "true");
                }
            }
            else
            {
                bool switchOn = Convert.ToBoolean(ConfigurationManager.AppSettings["switchOn"].ToString());
                if (switchOn)
                {
                    sql = string.Format("CALL TNS_CC_SWITCH_RDS({0})", false);
                    TNS_QTConsulta(sql, connection_write);
                    ConfigurationManager.AppSettings.Set("switchOn", "false");
                }
            }
        }
        /// <summary>
        /// determian si el token de sesion existe en la base de datos clientcontrol mysql
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static bool TNS_ValidarTokenDeSesion(string token, string cliente)
        {
            TNS_ValidarSwitchRds();

            /*conexion db clientControl*/
            string conexionclient = HelperWs.FoundRouteClientControlMySql();

            string vsql = "select t.token, t.connectionstring, t.connectiondbimages " +
                          "from tokens t " +
                          "inner join empresa e on t.empresaid = e.empresaid " +
                          "where t.token = @param0 and e.codigo = @param1";
            List<object> parametros = new List<object>() { token, cliente };

            JsonDataResult json = TNS_QTConsulta(vsql, parametros, conexionclient);
            DataTable tabla = (DataTable)JsonConvert.DeserializeObject(json.CONTENIDO.ToString(), typeof(DataTable));

            if (tabla != null)
            {
                if (tabla.Rows.Count > 0)
                {
                    string vtoken = tabla.Rows[0]["TOKEN"].ToString();
                    if (vtoken.Equals(token))
                    {
                        GlobalVariables.ConexionMySql = tabla.Rows[0]["CONNECTIONSTRING"].ToString();
                        GlobalVariables.ConexionDbImages = tabla.Rows[0]["CONNECTIONDBIMAGES"].ToString();
                        return true;
                    }
                    else
                    {
                        GlobalVariables.Mensaje = "El token suministrado es incorrecto.";
                        return false;
                    }
                }
                else
                    GlobalVariables.Mensaje = "Tu sesion ha finalizado.";
            }
            else
                GlobalVariables.Mensaje = GlobalVariables.ErrorDB;
            return false;
        }
    }
}
