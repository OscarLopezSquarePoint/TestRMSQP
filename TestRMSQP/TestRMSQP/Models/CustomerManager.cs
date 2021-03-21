using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestRMSQP.Models
{
    public class CustomerManager
    {
        custLoginRepoModelContainer DBContext = new custLoginRepoModelContainer();

        public Int32 SetCustomer (string customerName, string customerCIF, ref string errorMessage)
        {
            try
            {
                //Comprobar valores obligatorios
                if (string.IsNullOrEmpty(customerName))
                {
                    errorMessage = "El nombre del cliente es obligatorio";
                    return 0;
                }
                if (string.IsNullOrEmpty(customerCIF))
                {
                    errorMessage = "El CIF del cliente es obligatorio";
                    return 0;
                }
                //No puede haber 2 clientes con el mismo CIF
                if (DBContext.Customer.Where(x => x.custCif == customerCIF).Count() > 0)
                {
                    errorMessage = "Ya existe cliente con CIF "+customerCIF;
                    return 0;
                }
                //Registramos nuevo cliente
                using (var transaction = DBContext.Database.BeginTransaction())
                {
                    Customer cust = new Customer();
                    cust.custName = customerName;
                    cust.custCif = customerCIF;
                    DBContext.Customer.Add(cust);
                    DBContext.SaveChanges();
                    Int32 id = cust.custId;
                    transaction.Commit();
                    return id;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return 0;
            }
        }
    }
}