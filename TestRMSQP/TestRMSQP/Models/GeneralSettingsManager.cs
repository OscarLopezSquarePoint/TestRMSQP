using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestRMSQP.Models
{
    public class GeneralSettingsManager
    {

        custLoginRepoModelContainer DBContext = new custLoginRepoModelContainer();

        public Int32 SetGeneralSettings(string registryProvider, ref string errorMessage)
        {
            Int32 id = 0;
            try
            {
                using (var transacction = DBContext.Database.BeginTransaction())
                {
                    GeneralSettings GS = getGeneralSettings();
                    if (GS == null)
                    {
                        GS = new GeneralSettings();
                        GS.regProvider = registryProvider;
                        DBContext.GeneralSettings.Add(GS);
                        DBContext.SaveChanges();
                        id = GS.Id;
                        transacction.Commit();
                    }
                    else
                    {
                        GS.regProvider = registryProvider;
                        DBContext.SaveChanges();
                        transacction.Commit();
                    }

                }
                return id;
            }catch (Exception ex)
            {
                errorMessage = ex.Message;
                return 0;
            }

        }

        //Obtener parámetros generales.
        public GeneralSettings getGeneralSettings()
        {
            //Solo debería haber un registro en esta tabla. Seleccionamos el último por si acaso
            Int32 id = DBContext.GeneralSettings.Max(x => x.Id);
            GeneralSettings GS = DBContext.GeneralSettings.Find(id);
            return GS;
        }

        public string getRegistryProvider()
        {
            GeneralSettings GS = getGeneralSettings();
            return GS.regProvider;
        }
    }

}