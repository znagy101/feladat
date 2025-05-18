using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Application.Interfaces;
using W31UL9_HSZF_2024252.Model;
using W31UL9_HSZF_2024252.Persistence.MsSql.Interfaces;

namespace W31UL9_HSZF_2024252.Application.Services
{
    public class CustomerService : ICustomerService
    {
        ICustomerDataProvider repo;

        public CustomerService(ICustomerDataProvider irepo)
        {
            repo = irepo;
        }

        public void Delete(int id)
        {
            //kivetel
            repo.Delete(id);
        }

        public IQueryable<Customer> GetAll()
        {
            return repo.GetAll();
        }

        public void Insert(Customer item)
        {
            repo.Insert(item);
        }

        public void Update(Customer item)
        {
            repo.Update(item);
        }
    }
}

