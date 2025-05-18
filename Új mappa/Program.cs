using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using W31UL9_HSZF_2024252.Application;
using W31UL9_HSZF_2024252.Application.Services;
using W31UL9_HSZF_2024252.Model;
using W31UL9_HSZF_2024252.Model.EntityesLists;
using W31UL9_HSZF_2024252.Persistence.MsSql;
using W31UL9_HSZF_2024252.Persistence.MsSql.DataProviders;
using W31UL9_HSZF_2024252.Persistence.MsSql.Interfaces;

namespace W31UL9_HSZF_2024252
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var serviceProvider = RegisterServices();

            using (var context = serviceProvider.GetService<AppDbContext>())
            { 
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

               
            Listing(serviceProvider);
            StartTripProgram(serviceProvider);
            InitDatabase(serviceProvider);
            Reports(serviceProvider);








            ;
        }

        public static void StartTripProgram(IServiceProvider serviceProvider)
        { 
            StartTrip startTrip = serviceProvider.GetService<StartTrip>();

            Console.WriteLine("A bérlés megkezdéséhez, adjon meg néhány adatot!");
            Console.Write("Car Id: ");
            int carId = int.Parse(Console.ReadLine());
            Console.Write("Customer Id: ");
            int customerId = int.Parse(Console.ReadLine());


            startTrip.CanStartTrip(customerId);
            startTrip.Start(carId, customerId);
            Console.Write("Adja meg, hany km-t veztett: ");
            int distance = int.Parse(Console.ReadLine());
            startTrip.EndTrip(distance);
        
        }

        public static IServiceProvider RegisterServices()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                                .AddTransient<AppDbContext>()
                                .AddTransient<ICarDataProvider, CarDataProvider>()
                                .AddTransient<ICustomerDataProvider, CustomerDataProvider>()
                                .AddTransient<ITripDataProvider, TripDataProvider>()
                                .AddTransient<CarService>()
                                .AddTransient<CustomerService>()
                                .AddTransient<TripService>()
                                .AddTransient<StartTrip>()
                                .AddTransient<Queries>()
                                .BuildServiceProvider();
            return serviceProvider;
        }

        public static void InitDatabase(IServiceProvider serviceProvider)
        {
            //var dbContext = serviceProvider.GetService<AppContext>();
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            CarService carService = serviceProvider.GetService<CarService>();
            CustomerService customerService = serviceProvider.GetService<CustomerService>();
            TripService tripService = serviceProvider.GetService<TripService>();

            var cars = LoadFromXmlFile<Cars>("car.xml");

            foreach (var item in cars.CarList)
            {
                carService.Insert(item);
            }

            var customers = LoadFromXmlFile<Customers>("customer.xml");

            foreach (var item in customers.CustomerList)
            {
                customerService.Insert(item);
            }

            var trips = LoadFromXmlFile<Trips>("trip.xml");

            foreach (var item in trips.TripList)
            {
                tripService.Insert(item);
            }











            }

        public static T LoadFromXmlFile<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using FileStream fs = new FileStream(path, FileMode.Open);
            return (T)serializer.Deserialize(fs)!;
        }

        public static void Reports(IServiceProvider serviceProvider)
        {
            Queries queries = serviceProvider.GetService<Queries>();

            var mostDrivenCar = queries.GetMostDrivenCar();
            Console.WriteLine( $"Legtobbet futott jarmu: {mostDrivenCar}");

            var averageCarDistance = queries.GetAverageCarDistance();
            Console.WriteLine( $"Autok atlagos futasa: {averageCarDistance}");

            var top10HighestPayingCustomer = queries.GetTop10HighestPayingCustomers();
            Console.WriteLine("Top10 legtobbet fizeto Ugyfel");
            foreach (var item in top10HighestPayingCustomer)
            {
                Console.WriteLine( $"{item}" );
            }

        }

        public static void Listing(IServiceProvider serviceProvider)
        {
            CarService carService = serviceProvider.GetService<CarService>();
            CustomerService customerService = serviceProvider.GetService<CustomerService>();
            TripService tripService = serviceProvider.GetService<TripService>();

            Console.WriteLine("=== CARS ===");
           
            foreach (var car in carService.GetAll().ToList())
            {
                Console.WriteLine($"{car.Id} {car.Model} {car.TotalDistance}km {car.DistanceSinceLastMaintenance}km");
            }

            Console.WriteLine("=== CUSTOMERS ===");
            
            foreach (var customer in customerService.GetAll().ToList())
            {
                Console.WriteLine($"{customer.Id} {customer.Name} {customer.Balance}EUR");
            }

            Console.WriteLine("=== TRIPS ===");
          
            foreach (var trip in tripService.GetAll().ToList())
            {
                Console.WriteLine($"{trip.Id} {trip.CarId} {trip.CustomerId} {trip.Distance}km {trip.Cost}EUR");
            }

        }





}
} 
