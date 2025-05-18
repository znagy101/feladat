using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Model;

namespace W31UL9_HSZF_2024252.Application.Interfaces
{
    public interface ICustomerService
    {
        public IQueryable<Customer> GetAll();
        public void Insert(Customer item);
        public void Update(Customer item);
        public void Delete(int id);
    }
}
