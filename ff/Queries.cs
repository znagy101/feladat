using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Model;
using W31UL9_HSZF_2024252.Persistence.MsSql.Interfaces;

namespace W31UL9_HSZF_2024252.Application
{
    public class Queries
    {
        ICustomerDataProvider customerRepo;
        ITripDataProvider tripRepo;
        ICarDataProvider carRepo;


        public Queries(ICustomerDataProvider customerRepo, ITripDataProvider tripRepo, ICarDataProvider carRepo)
        {
            this.customerRepo = customerRepo;
            this.tripRepo = tripRepo;
            this.carRepo = carRepo;
        }

        public Car GetMostDrivenCar()
        {
            return carRepo.GetAll()
                .OrderByDescending(c => c.TotalDistance)
                .FirstOrDefault();
        }

        public decimal GetAverageCarDistance()
        {
            var cars = carRepo.GetAll().ToList();

            if (cars.Count == 0)
                return 0;

            return cars.Average(c => c.TotalDistance);
        }

        public List<(Customer Customer, decimal TotalSpent)> GetTop10HighestPayingCustomers()
        {
            var trips = tripRepo.GetAll().ToList();
            var customers = customerRepo.GetAll().ToList();

            var customerSpending = customers
                .Select(customer => {
                    decimal totalSpent = trips
                        .Where(trip => trip.CustomerId == customer.Id)
                        .Sum(trip => trip.Cost);

                    return (Customer: customer, TotalSpent: totalSpent);
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(10)
                .ToList();

            return customerSpending;
        }
    }
}
