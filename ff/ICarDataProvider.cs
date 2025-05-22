using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Model;

namespace W31UL9_HSZF_2024252.Persistence.MsSql.Interfaces
{
    public interface ICarDataProvider
    {
        public IQueryable<Car> GetAll();  
        public void Insert(Car item);
        public void Update(Car item);
        public void Delete(int id);
        public Car Read(int id);
    }
}
