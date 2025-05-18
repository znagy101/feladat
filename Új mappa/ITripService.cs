using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Model;

namespace W31UL9_HSZF_2024252.Application.Interfaces
{
    public interface ITripService
    {
        public IQueryable<Trip> GetAll();
        public void Insert(Trip item);
        public void Update(Trip item);
        public void Delete(int id);
    }
}
