using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
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
            InitDatabase(serviceProvider);

            Console.WriteLine(  "qweqweqwe");

            

            //foreach (var car in cars.CarList)
            //{
            //    Console.WriteLine($"{car.Id}: {car.Model}, {car.TotalDistance} km összesen");
            //}



            ;
        }

        private static IServiceProvider RegisterServices()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                                .AddTransient<AppDbContext>()
                                .AddTransient<ICarDataProvider, CarDataProvider>()
                                .AddTransient<ICustomerDataProvider, CustomerDataProvider>()
                                .AddTransient<ITripDataProvider, TripDataProvider>()
                                .AddTransient<CarService>()
                                .AddTransient<CustomerService>()
                                .AddTransient<TripService>()
                                .BuildServiceProvider();
            return serviceProvider;
        }

        public static void InitDatabase(IServiceProvider serviceProvider)
        {
            CarService carService = serviceProvider.GetService<CarService>();
            CustomerService customerService = serviceProvider.GetService<CustomerService>();
           // TripService tripService = serviceProvider.GetService<TripService>();

            var cars = LoadFromXmlFile<Cars>("car.xml");

            foreach (var item in cars.CarList)
            {
                carService.Insert(item);
            }

            var carsss = carService.GetAll();

            foreach (var item in carsss)
            {
                Console.WriteLine( "item");
            }



        }



        public static T LoadFromXmlFile<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using FileStream fs = new FileStream(path, FileMode.Open);
            return (T)serializer.Deserialize(fs)!;
        }




}
} 
