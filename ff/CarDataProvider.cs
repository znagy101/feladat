using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Model;
using W31UL9_HSZF_2024252.Persistence.MsSql.Interfaces;

namespace W31UL9_HSZF_2024252.Persistence.MsSql.DataProviders
{
    public class CarDataProvider : ICarDataProvider
    {
        private AppDbContext appdbc;

        public CarDataProvider(AppDbContext appDbContext)
        {
            appdbc = appDbContext;
        }

        public void Delete(int id)
        {
            Car car = appdbc.Cars.FirstOrDefault(x => x.Id == id);
            if (car != null)
            {
                appdbc.Remove(car);
                appdbc.SaveChanges();
            }

        }

        public IQueryable<Car> GetAll()
        {
            return appdbc.Cars;
        }

        public void Insert(Car item)
        {
            appdbc.Add(item);
            appdbc.SaveChanges();
        }

        public void Update(Car item)
        {
            Car oldCar = Read(item.Id);
            oldCar.Id = item.Id;
            oldCar.Model = item.Model;
            oldCar.TotalDistance = item.TotalDistance;
            oldCar.DistanceSinceLastMaintenance = item.DistanceSinceLastMaintenance;
            appdbc.SaveChanges();
        }

        public Car Read(int id) 
        {
            return appdbc.Cars.FirstOrDefault(x => x.Id == id);
        }
    }
}

