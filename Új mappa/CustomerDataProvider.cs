using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Model;
using W31UL9_HSZF_2024252.Persistence.MsSql.Interfaces;

namespace W31UL9_HSZF_2024252.Persistence.MsSql.DataProviders
{
    public class CustomerDataProvider : ICustomerDataProvider
    {
        private AppDbContext appdbc;

        public CustomerDataProvider(AppDbContext appDbContext)
        {
            appdbc = appDbContext;
        }

        public void Delete(int id)
        {
            Customer customer = appdbc.Customers.FirstOrDefault(x => x.Id == id);
            if (customer != null)
            {
                appdbc.Remove(customer);
                appdbc.SaveChanges();
            }
        }

        public IQueryable<Customer> GetAll()
        {
            return appdbc.Customers;
        }

        public void Insert(Customer item)
        {
            appdbc.Add(item);
            appdbc.SaveChanges();
        }

        public void Update(Customer item)
        {
            Customer oldCustomer = Read(item.Id);
            oldCustomer.Id = item.Id;
            oldCustomer.Name = item.Name;
            oldCustomer.Balance = item.Balance;
            appdbc.SaveChanges();
        }

        public Customer Read(int id)
        {
            return appdbc.Customers.FirstOrDefault(x => x.Id == id);
        }
    }
}

