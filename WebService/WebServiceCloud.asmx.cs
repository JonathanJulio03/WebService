using ClassLibrary;
using ClassLibrary.Commons;
using System;
using System.Web.Script.Serialization;
using System.Web.Services;
using WebService.Models.Commons;
using WebService.Models.Dao;

namespace WebService
{
    /// <summary>
    /// Descripción breve de WebServiceCloud
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceCloud : System.Web.Services.WebService
    {
        public JavaScriptSerializer serializer;
        public WebServiceCloud() {
            serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
        }
        [WebMethod]
        public string GetCRUD(string Token, string User, string Cliente, string Sucursal, string Terminal, string datajson)
        {
            try
            {
                if (DBWs.TNS_ValidarTokenDeSesion(Token, Cliente))
                {
                    string vdj = GetCrudDao.GetDataCRUD(User, Sucursal, Terminal, datajson);
                    if (vdj != null)
                        return Helper.Envoltorio(vdj.ToString(), true);
                    else
                        return Helper.Envoltorio(GlobalVariables.Mensaje, false);
                }
                return Helper.Envoltorio(GlobalVariables.Mensaje, false, true);
            }
            catch (Exception e)
            {
                return Helper.Envoltorio(HelperWs.getMessageException(e), false);
            }
        }
    }
}
