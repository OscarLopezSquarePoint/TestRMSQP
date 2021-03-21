using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TestRMSQP.Tools
{
    public static class Passwords
    {
        #region Encriptacion
        //Encripta clave para poder almacenarla o compararla con una almacenada previamente
        public static string EncryptPassword (byte[] password)
        {
            string pwdEncripted;
            //Usamos MD5
            MD5 myHash = new MD5CryptoServiceProvider();
            //Obtenemos hash
            myHash.ComputeHash(password);
            //Convertimos a texto base 64 para poder guardar como string
            pwdEncripted = (Convert.ToBase64String(myHash.Hash));
            return pwdEncripted;
        }

        //Sobrecarga para pasar clave como string
        public static string EncryptPassword(string password)
        {
            return EncryptPassword(Encoding.ASCII.GetBytes(password));
        }
        #endregion

        #region validacion_claves
        //Función que determina si una contraseña es correcta
        public static Boolean CorrectPassword (string password, ref string errorMessage)
        {
            //PENDIENTE! definir una reglas seria de contraseñas.
            //De momento solo controlamos que tenga una lóngitud mínima
            //La responsabilidad de comprobar si ya existe otra igual es del modelo

            //Control londitud mínima
            int minLength = 4;
            if (string.IsNullOrEmpty(password)) { errorMessage = "Lóngitud mínima de " + minLength.ToString(); return false; }
            if (password.Length < minLength ) { errorMessage = "Lóngitud mínima de " + minLength.ToString(); return false; }

            return true;
        }
        #endregion
    }
}