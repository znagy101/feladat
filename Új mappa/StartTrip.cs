using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W31UL9_HSZF_2024252.Model;
using W31UL9_HSZF_2024252.Persistence.MsSql.Interfaces;

namespace W31UL9_HSZF_2024252.Application
{
    public class StartTrip
    {
        public delegate void CanStart();
        public CanStart canStart;
        public delegate void MaintanceEvents ();
        public MaintanceEvents maintance20;
        public MaintanceEvents maintance200km;

        const decimal ratePerKm = 0.35m;
        const decimal baseFee = 0.5m;
        ICustomerDataProvider customerRepo;
        ITripDataProvider tripRepo;
        ICarDataProvider carRepo;

        int lastTripId;
        
        

        public StartTrip(ICustomerDataProvider customerRepo, ITripDataProvider tripRepo, ICarDataProvider carRepo)
        {
            this.customerRepo = customerRepo;
            this.tripRepo = tripRepo;
            this.carRepo = carRepo;

            var allTrips = tripRepo.GetAll();
            this.lastTripId = allTrips.Any() ? allTrips.Max(t => t.Id) : 0;
        }

        public bool CanStartTrip(int customerId)
        {
            var customer = customerRepo.Read(customerId);
            if (customer == null || customer.Balance < 40.0m)
            {
                canStart?.Invoke();
                return false;
            }

            return true;
        }

        public Trip Start(int carId, int customerId)
        {
            if (!CanStartTrip(customerId))
                return null;


            lastTripId++;
            var trip = new Trip
            {
                Id=lastTripId,
                CarId = carId,
                CustomerId = customerId,
                Distance = 0,
                Cost =0
            };

            tripRepo.Insert(trip);
            return trip;
        }

        public decimal EndTrip( decimal distance)
        { 
            //update trip
            var trip = tripRepo.Read(lastTripId);
            if (trip == null) 
                return 0;

            trip.Distance = distance;
            trip.Cost = CalculateCost(distance);
            tripRepo.Update(trip);

            //update customer
            var customer = customerRepo.Read(trip.CustomerId);
            customer.Balance -= trip.Cost;
            customerRepo.Update(customer);

            //update car
            var car = carRepo.Read(trip.CarId);
            car.TotalDistance += distance;
            car.DistanceSinceLastMaintenance += distance;
            carRepo.Update(car);

            CheckForMaintenance(car);

            return trip.Cost;

        }

        private void CheckForMaintenance(Car car)
        { 
            bool needsMaintenance = false;

            if (new Random().Next(100) < 20)
            { 
                needsMaintenance = true;
                maintance20?.Invoke();
            }

            if (car.DistanceSinceLastMaintenance >= 200)
            {
                needsMaintenance = true;
                //[ESEMENY] - 200km+ szerviz
                maintance200km?.Invoke();
            }

            if (needsMaintenance)
            {
                PerformanceMaintance(car.Id);
            }
        }

        private void PerformanceMaintance(int carId)
        { 
            var car = carRepo.Read(carId);
            if (car != null)
            { 
                car.DistanceSinceLastMaintenance = 0;
                carRepo.Update(car);
            }
        }

        private decimal CalculateCost(decimal distance)
        {
            return baseFee + (distance * ratePerKm);
        }


    }
}
