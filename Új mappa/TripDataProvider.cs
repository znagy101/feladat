using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Model;
using W31UL9_HSZF_2024252.Persistence.MsSql.Interfaces;
using Microsoft.Extensions.Logging;

namespace W31UL9_HSZF_2024252.Persistence.MsSql.DataProviders
{
    public class TripDataProvider : ITripDataProvider
    {
        private AppDbContext appdbc;
        

    public TripDataProvider(AppDbContext appDbContext)
        {
            appdbc = appDbContext;
        }

        public void Delete(int id)
        {

            Trip trip = appdbc.Trips.FirstOrDefault(x => x.Id == id);
            if (trip != null)
            {
                appdbc.Remove(trip);
                appdbc.SaveChanges();
            }


        }

        public IQueryable<Trip> GetAll()
        {
            return appdbc.Trips;
        }

        public void Insert(Trip item)
        {
            appdbc.Add(item);
            appdbc.SaveChanges();
        }

        public void Update(Trip item)
        {
            Trip oldTrip = Read(item.Id);
            oldTrip.Id = item.Id;
            oldTrip.CarId = item.CarId;
            oldTrip.CustomerId = item.CustomerId;   
            oldTrip.Distance = item.Distance;
            oldTrip.Cost = item.Cost;
            appdbc.SaveChanges();
        }

        public Trip Read(int id)
        {
            return appdbc.Trips.FirstOrDefault(x => x.Id == id);
        }
    }
}

