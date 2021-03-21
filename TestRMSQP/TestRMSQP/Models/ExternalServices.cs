using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace TestRMSQP.Models
{
    public class ExternalServices
    {
        public string GetApplicationUid(Int32 id, ref string errorMessage)
        {
            //Obtener proveedor externo actual
            string provider = new GeneralSettingsManager().getRegistryProvider();
            //OJO. Pendiente obtener proveedor
            switch (provider.ToUpper())
            {
             case "DEFAULT":
                return GetApplicationUid_Default(id, ref errorMessage);
                break;
             default:
                errorMessage = "Proveedor de servicios externo desconocido.";
                return "";
                break;
            }
        }

        public string GetApplicationUid_Default(Int32 id, ref string errorMessage)
        {
            string login = "RMS"; //Nadie ha explicitado cual usar.
            string url1 = "http://sqpincipreapi.azurewebsites.net/api/TestRM/GetToken?login=" + login;
            string url2 = "http://sqpincipreapi.azurewebsites.net/api/TestRM/GetAplicationUid/"+id.ToString();
            string token = string.Empty;
            string uid = string.Empty;
            string responseString = string.Empty;

            try
            {
                //Llamamos a servicio para obtener token
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url1);
                request.Method = "GET";
                //OJO// request.Proxy = HttpWebRequest.DefaultWebProxy; //OJO. ¿Esto hace falta? // 
                using (WebResponse response = request.GetResponse())
                {
                    
                    //Recibir el stream devuelto por el servidor
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        //Abrir el Stream utilizando el StreamReader
                        StreamReader reader = new StreamReader(dataStream);
                        //Leer el contenido del StreamReader
                        responseString = reader.ReadToEnd();
                    }
                }
                Dictionary<string, string> resultJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
                if (resultJson.ContainsKey("token"))
                {
                    token = resultJson["token"];
                    if (string.IsNullOrEmpty(token))
                    {
                        errorMessage = "Elemento token recibido no tiene valor.";
                        return uid;
                    }
                }
                else
                {
                    errorMessage = "La respuesta no contiene un elemento token";
                    return uid;
                }

                //Llamamos a servicio para obtener UID
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(url2);
                request2.Method = "POST";
                request2.ContentLength = 0; //No estamos enviando ningún dato
                request2.Headers["Authorization"] = "Bearer " + token;
                //OJO// request2.Proxy = HttpWebRequest.DefaultWebProxy; //OJO. ¿Esto hace falta? // 
                using (WebResponse response = request2.GetResponse())
                {
                    //Recibir el stream devuelto por el servidor
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        //Abrir el Stream utilizando el StreamReader
                        StreamReader reader = new StreamReader(dataStream);
                        //Leer el contenido del StreamReader
                        responseString = reader.ReadToEnd();
                    }
                }
                resultJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
                if (resultJson.ContainsKey("uid"))
                {
                    uid = resultJson["uid"];
                    if (string.IsNullOrEmpty(uid)) errorMessage = "elemento uid recibido no tiene valor.";
                    return uid;
                }
                else
                {
                    errorMessage = "La respuesta no contiene un elemento uid";
                    return uid;
                }

            }
            catch (Exception ex) {
                errorMessage = ex.Message;
                return uid;
            }

        }

    }

}