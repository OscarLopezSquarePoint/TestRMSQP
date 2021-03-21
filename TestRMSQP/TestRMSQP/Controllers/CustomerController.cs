using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace TestRMSQP.Controllers
{
    public class CustomerController : ApiController
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
                if (!requestJson.ContainsKey("custName")) return Ok(new { error = true, errorMessage = "Falta parámetro obligatorio custName.", returnData = "" });
                string custName = requestJson["custName"].ToString();
                if (string.IsNullOrEmpty(custName)) return Ok(new { error = true, errorMessage = "Falta valor para parámetro obligatorio custName.", returnData = "" });
                if (!requestJson.ContainsKey("cif")) return Ok(new { error = true, errorMessage = "Falta parámetro obligatorio cif.", returnData = "" });
                string cif = requestJson["cif"].ToString();
                if (string.IsNullOrEmpty(cif)) return Ok(new { error = true, errorMessage = "Falta valor para parámetro obligatorio cif.", returnData = "" });

                //Guardamos aplicación
                string message = "";
                Int32 appId = new TestRMSQP.Models.CustomerManager().SetCustomer(custName, cif, ref message);
                if (appId > 0)
                    return Ok(new { error = "false", errorMessage = "", returnData = "{appId=" + appId.ToString() + "}" });
                else
                    return Ok(new { error = "true", errorMessage = message, returnData = "" });
            }
            catch (Exception ex)
            {
                return Ok(new { error = "true", errorMessage = ex.Message, returnData = "" });
            }

        }
    }
}
