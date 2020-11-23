using ClassLibrary;
using ClassLibrary.Commons;
using Newtonsoft.Json;
using System;
using System.Data;
using WebService.Models.Commons;

namespace WebService.Models.Dao
{
    public class GetCrudDao
    {

        public static string GetDataCRUD(string User, string Sucursal, string Terminal, string datajson)
        {
            try
            {
                dynamic jsond = JsonConvert.DeserializeObject(datajson);
                string vquery = "CALL TNS_WF_" + jsond.sp + "('" + jsond.json + "'," + Sucursal + ",'" + User + "','" + Terminal + "'')";

                JsonDataResult json = DBWs.TNS_QTConsulta(vquery);
                GlobalVariables.Mensaje = "";
                if (json.SUCCESS)
                {
                    DataTable tabla = (DataTable)JsonConvert.DeserializeObject(json.CONTENIDO.ToString(), typeof(DataTable));
                    try
                    {
                        int success = Convert.ToInt32(tabla.Rows[0]["OSUCCESS"].ToString());
                        if (success.Equals(0))
                        {
                            GlobalVariables.Mensaje = tabla.Rows[0]["OMENSAJE"].ToString();
                            return null;
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    return json.CONTENIDO.ToString();
                }
                else
                {
                    GlobalVariables.Mensaje = json.CONTENIDO.ToString();
                    return null;
                }

            }
            catch (Exception ex)
            {
                return Helper.Envoltorio(HelperWs.getMessageException(ex), false);
            }
        }
    }
}