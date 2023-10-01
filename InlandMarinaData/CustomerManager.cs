using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlandMarinaData
{
    public class CustomerManager
    {
        public static Customer Authenticate(string username, string password)
        {
            InlandMarinaContext db = new InlandMarinaContext();

            var customer = db.Customers.SingleOrDefault(cust => cust.Username == username
                                                        && cust.Password == password);
            return customer; //this will either be null or an object
        }
    }
}
