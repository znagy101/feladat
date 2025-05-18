using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Model;

namespace W31UL9_HSZF_2024252.Persistence.MsSql.Interfaces
{
    public interface ICustomerDataProvider
    {
        public IQueryable<Customer> GetAll();
        public void Insert(Customer item);
        public void Update(Customer item);
        public void Delete(int id);
        public Customer Read(int id);
    }
}
