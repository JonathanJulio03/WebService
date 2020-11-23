using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models.Commons
{
    public class Helper
    {
        public static string Envoltorio(string contenido, bool success, bool kill_sesion = false)
        {
            if (success)
                return "{ 'success': true, 'kill_sesion': '" + kill_sesion + "','response':" + contenido + "}";
            else
                return "{ 'success': false, 'kill_sesion': '" + kill_sesion + "','response':'" + contenido + "'}";
        }
    }
}