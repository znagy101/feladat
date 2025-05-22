using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Application.Interfaces;
using W31UL9_HSZF_2024252.Model;
using W31UL9_HSZF_2024252.Persistence.MsSql.DataProviders;
using W31UL9_HSZF_2024252.Persistence.MsSql.Interfaces;

namespace W31UL9_HSZF_2024252.Application.Services
{
    public class TripService : ITripService
    {
        ITripDataProvider repo;

        public TripService(ITripDataProvider irepo)
        {
            repo = irepo;
        }

        public void Delete(int id)
        {
            
            var trip = repo.Read(id);
            if (trip == null)
            {
                throw new EntityNotFoundException("Trip", id);
            }

            
            repo.Delete(id);
        }

        public IQueryable<Trip> GetAll()
        {
            return repo.GetAll();
        }

        public void Insert(Trip item)
        {
            repo.Insert(item);
        }

        public void Update(Trip item)
        {
            repo.Update(item);
        }
    }
}

