using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TestRMSQP.Controllers
{
    public class ApplicationController : ApiController
    {
        public IHttpActionResult Post([FromBody] dynamic request)
        {

            if (request == null)
                return Ok(new { error = "true", errorMessage = "json data invalid", returnData = "" });

            try
            {
                //Convertimos json entrada en un diccionario de claves y valores string
                var requestJson = JsonConvert.DeserializeObject<Dictionary<string,string>>(request.ToString());

                //Comprobamos existan elementos y valores obligatorios
                if (!requestJson.ContainsKey("appName")) return Ok(new { error = true, errorMessage = "Falta parámetro obligatorio appName.", returnData = "" });
                string appName = requestJson["appName"].ToString();
                if (string.IsNullOrEmpty(appName)) return Ok(new { error = true, errorMessage = "Falta valor para parámetro obligatorio appName.", returnData = "" });

                //Guardamos aplicación
                string message = "";
                Int32 appId = new TestRMSQP.Models.ApplicationManager().SetApplication(appName,ref message);
                if (appId > 0)
                    return Ok(new { error = "false", errorMessage = "",returnData = "{appId="+appId.ToString()+"}" });
                else
                    return Ok(new { error = "true", errorMessage = message, returnData = ""});
            }
            catch (Exception ex)
            {
                return Ok(new { error = "true", errorMessage = ex.Message, returnData = "" });
            }
        }
    }
}
