using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace TestRMSQP.Controllers
{
    public class LoginController : ApiController
    {
        public IHttpActionResult Post([FromBody] dynamic request)
        {

            if (request == null)
                return Ok(new { error = true, errorMessage = "json data invalid", returnData = "" });

            try
            {
                //Convertimos json entrada en un diccionario de claves y valores string
                var requestJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.ToString());

                //Comprobamos existan elementos y valores obligatorios
                Int32 appId = 0, custId = 0;
                string login,  loginName, password;

                if (!requestJson.ContainsKey("appId")) return Ok(new { error = true, errorMessage = "Falta parámetro obligatorio appId.", returnData = "" });
                if (!Int32.TryParse(requestJson["appId"].ToString(), out appId)) return Ok(new { error = true, errorMessage = "Valor parámetro appId no es válido", returnData = "" });

                if (!requestJson.ContainsKey("custId")) return Ok(new { error = true, errorMessage = "Falta parámetro obligatorio custId.", returnData = "" });
                if (!Int32.TryParse(requestJson["custId"].ToString(), out custId)) return Ok(new { error = true, errorMessage = "Valor parámetro custId no es válido", returnData = "" });

                if (!requestJson.ContainsKey("login")) return Ok(new { error = true, errorMessage = "Falta parámetro obligatorio login.", returnData = "" });
                login= requestJson["login"].ToString();
                if (string.IsNullOrEmpty(login)) return Ok(new { error = true, errorMessage = "Falta valor para parámetro obligatorio login.", returnData = "" });

                if (requestJson.ContainsKey("loginName")) loginName = requestJson["loginName"].ToString();
                else loginName = "";

                if (!requestJson.ContainsKey("password")) return Ok(new { error = true, errorMessage = "Falta parámetro obligatorio password.", returnData = "" });
                password = requestJson["password"].ToString();
                if (string.IsNullOrEmpty(password)) return Ok(new { error = true, errorMessage = "Falta valor para parámetro obligatorio password.", returnData = "" });

                //Guardamos aplicación
                string message = "";
                new TestRMSQP.Models.LoginManager().SetLogin(appId, custId, login, loginName, password, ref message);
                if (string.IsNullOrEmpty(message))
                    return Ok(new { error = "false", errorMessage = "", returnData = "" });
                else
                    return Ok(new { error = "true", errorMessage = message, returnData = "" });
            }
            catch (Exception ex)
            {
                return Ok(new { error = "true", errorMessage = ex.Message, returnData = "" });
            }
        }

        public IHttpActionResult Delete([FromBody] Int32 id)
        {

            if (id < 1)
                return Ok(new { error = true, errorMessage = "Identificador incorrecto", returnData = "" });

            //OJO// Temporal para comprobar primera llamada
            return Ok(new { error = true, errorMessage = "En desarrollo", returnData = "" });
        }
    }
}
