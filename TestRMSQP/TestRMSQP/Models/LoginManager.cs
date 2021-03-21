using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestRMSQP.Models
{
    public class LoginManager
    {
        custLoginRepoModelContainer DBContext = new custLoginRepoModelContainer();

        public void SetLogin (Int32 applicationId, Int32 customerId, string login, string loginName, string password, ref string errorMessage)
        {
            try
            {
                //Comprobación valor login
                if (string.IsNullOrEmpty(login))
                {
                    errorMessage = "Login es un valor obligatorio";
                    return;
                }
                //Comprobación clave correcta
                if (!Tools.Passwords.CorrectPassword(password, ref errorMessage))
                {
                    errorMessage = "Clave incorrecta. " + errorMessage;
                    return;
                }
                //Comprobación clave no se ha utilizado anteriormente
                string encryptedPwd = Tools.Passwords.EncryptPassword(password);
                if (DBContext.Login.Where(x => x.Password == encryptedPwd).Count() > 0)
                {
                    errorMessage = "Clave incorrecta. Utilizada anteriormente.";
                    return;
                }
                //Comprobación existe aplicación
                if (DBContext.Application.Find(applicationId) == null)
                {
                    errorMessage = "No existe aplicación con identificador " + applicationId.ToString();
                    return;
                }
                //Comprobación existe cliente 
                if (DBContext.Customer.Find(customerId) == null)
                {
                    errorMessage = "No existe cliente con identificador " + customerId.ToString();
                    return;
                }
                //Comprobación no existe login
                if (DBContext.Login.Where(x => x.login1 == login).Count() > 0)
                {
                    errorMessage = "Login " + login + " ya existe.";
                    return;
                }
                // Registramos nuevo login
                using (var transaction = DBContext.Database.BeginTransaction())
                {
                    Login logi = new Login();
                    logi.login1 = login;
                    logi.loginName = loginName;
                    logi.Password = encryptedPwd;
                    logi.appId = applicationId;
                    logi.custId = customerId;
                    DBContext.Login.Add(logi);
                    DBContext.SaveChanges();
                    transaction.Commit();
                    return;
                }
            } catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public void DeleteLogin (string login, ref string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(login))
                {
                    errorMessage = "Login es un valor requerido";
                    return;
                }
                var logins = DBContext.Login.Where(x => x.login1 == login);
                if (logins.Count() > 0)
                { 
                    using (var transaction = DBContext.Database.BeginTransaction())
                    {
                        var logi = logins.First();
                        DBContext.Login.Remove(logi);
                        DBContext.SaveChanges();
                        transaction.Commit();
                        return;
                    }
                }
                else
                {
                    errorMessage = "No existe login " + login;
                    return;
                }
                
            } catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}