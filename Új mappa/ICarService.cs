using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Model;

namespace W31UL9_HSZF_2024252.Application.Interfaces
{
    public interface ICarService
    {
        public IQueryable<Car> GetAll();
        public void Insert(Car item);
        public void Update(Car item);
        public void Delete(int id);
        
    }
}
