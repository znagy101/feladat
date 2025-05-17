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
    public class CarService : ICarService
    {
        ICarDataProvider repo;

        public CarService(ICarDataProvider irepo)
        {
            repo = irepo;
        }

        public void Delete(int id)
        {
            //kivetel
            repo.Delete(id);
        }

        public IQueryable<Car> GetAll()
        {
            return repo.GetAll();
        }

        public void Insert(Car item)
        {
            repo.Insert(item);
        }

        public void Update(Car item)
        {
            repo.Update(item);
        }
    }
}

