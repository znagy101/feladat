using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Persistence.MsSql.Interfaces;

namespace W31UL9_HSZF_2024252.Application
{
    public class Queries
    {

        
        public Queries(ICustomerDataProvider customerRepo, ITripDataProvider tripRepo, ICarDataProvider carRepo)
        {
            this.customerRepo = customerRepo;
            this.tripRepo = tripRepo;
            this.carRepo = carRepo;
        }
    }
}
