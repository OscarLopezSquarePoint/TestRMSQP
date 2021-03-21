using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TestRMSQP.Models
{
    public class ApplicationManager
    {
        custLoginRepoModelContainer DBContext = new custLoginRepoModelContainer();
        
        public Int32 SetApplication (string name, ref string errorMessage)
        {
            Int32 id = 0;
            
            try
            {
                //Si no se ha indicado nombre aplicación 
                if (string.IsNullOrEmpty(name))
                {
                    errorMessage = "Debe indicar el nombre de aplicación.";
                    return id;
                }

                //Si existe aplicación con el nombre indicado devolvemos error
                if (DBContext.Application.Where(x => x.appName == name).Count() > 0)
                {
                    errorMessage = "Aplicación " + name + " ya está registrada.";
                    return id;
                }

                //Solicitamos UID

                //Creamos aplicación con el nombre indicado y un UID termporal
                using (var transaccion = DBContext.Database.BeginTransaction())
                {
                    Application app = new Application();
                    app.appName = name;
                    app.appUid = "t_"+name; //Asignamos UID falso que posteriormente modificaremos
                                                //No podemos asignar UID si no sabemos que id se le va a asignar
                    DBContext.Application.Add(app);

                    DBContext.SaveChanges();
                    id = app.appId;
                    transaccion.Commit();
                }

                //Solicitamos UID al proveedor externo con el identificador de aplicación recién creado
                string uid = new ExternalServices().GetApplicationUid(id, ref errorMessage);

                //Si no hemos obtenido un uid borramos la aplicación recien creada y devolvemos error
                if (string.IsNullOrEmpty(uid))
                {
                    using (var transaccion = DBContext.Database.BeginTransaction())
                    {
                        Application app = DBContext.Application.Find(id);
                        DBContext.Application.Remove(app);
                        DBContext.SaveChanges();
                        transaccion.Commit();
                    }
                    errorMessage = "No se ha podido obtener UID. " + errorMessage;
                    return 0;
                }

                //Actualizamos aplicación con el uid obtenido
                using (var transaccion = DBContext.Database.BeginTransaction())
                {
                    Application app = DBContext.Application.Find(id);
                    app.appUid = uid;
                    DBContext.SaveChanges();
                    transaccion.Commit();
                }
                return id;

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return 0;
            }
        }
    }
}