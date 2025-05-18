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

            InitDatabase(serviceProvider);

            bool exit = false;
            while (!exit)
            {
                exit = ShowMenu(serviceProvider);
            }

            Console.WriteLine("Program vége. Nyomjon meg egy billentyűt a kilépéshez...");
            Console.ReadKey();
        }

        static bool ShowMenu(IServiceProvider serviceProvider)
        {
            Console.Clear();
            Console.WriteLine("=== CAR SHARING ALKALMAZÁS ===");
            Console.WriteLine("1. Adatbázis műveletek (Insert/Update/Delete)");
            Console.WriteLine("2. Adatbázis tartalmának listázása");
            Console.WriteLine("3. Utazás indítása");
            Console.WriteLine("4. Riportok megtekintése");
            Console.WriteLine("0. Kilépés");
            Console.Write("\nVálasszon egy opciót: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DatabaseOperations(serviceProvider);
                    break;
                case "2":
                    Listing(serviceProvider);
                    WaitForKeyPress();
                    break;
                case "3":
                    StartTripProgram(serviceProvider);
                    WaitForKeyPress();
                    break;
                case "4":
                    Reports(serviceProvider);
                    WaitForKeyPress();
                    break;
                case "0":
                    return true; // Kilépés a programból
                default:
                    Console.WriteLine("Érvénytelen választás!");
                    WaitForKeyPress();
                    break;
            }

            return false; // Folytatás
        }

        static void WaitForKeyPress()
        {
            Console.WriteLine("\nNyomjon meg egy billentyűt a folytatáshoz...");
            Console.ReadKey();
        }

        static void DatabaseOperations(IServiceProvider serviceProvider)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== ADATBÁZIS MŰVELETEK ===");
                Console.WriteLine("1. Autó kezelése (Insert/Update/Delete)");
                Console.WriteLine("2. Ügyfél kezelése (Insert/Update/Delete)");
                Console.WriteLine("3. Utazás kezelése (Insert/Update/Delete)");
                Console.WriteLine("0. Vissza a főmenübe");
                Console.Write("\nVálasszon egy opciót: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageCars(serviceProvider);
                        break;
                    case "2":
                        ManageCustomers(serviceProvider);
                        break;
                    case "3":
                        ManageTrips(serviceProvider);
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás!");
                        WaitForKeyPress();
                        break;
                }
            }
        }

        static void ManageCars(IServiceProvider serviceProvider)
        {
            CarService carService = serviceProvider.GetService<CarService>();

            Console.Clear();
            Console.WriteLine("=== AUTÓK KEZELÉSE ===");
            Console.WriteLine("1. Új autó hozzáadása");
            Console.WriteLine("2. Autó módosítása");
            Console.WriteLine("3. Autó törlése");
            Console.WriteLine("0. Vissza");
            Console.Write("\nVálasszon egy opciót: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // Új autó hozzáadása
                    Car newCar = new Car();
                    Console.Write("ID: ");
                    newCar.Id = int.Parse(Console.ReadLine());
                    Console.Write("Modell: ");
                    newCar.Model = Console.ReadLine();
                    Console.Write("Teljes távolság: ");
                    newCar.TotalDistance = decimal.Parse(Console.ReadLine());
                    Console.Write("Távolság az utolsó karbantartás óta: ");
                    newCar.DistanceSinceLastMaintenance = decimal.Parse(Console.ReadLine());

                    carService.Insert(newCar);
                    Console.WriteLine("Autó sikeresen hozzáadva!");
                    break;

                case "2":
                    // Autó módosítása
                    Console.Write("Adja meg a módosítandó autó ID-jét: ");
                    int carId = int.Parse(Console.ReadLine());

                    ICarDataProvider carRepo = serviceProvider.GetService<ICarDataProvider>();
                    Car carToUpdate = carRepo.Read(carId);

                    if (carToUpdate != null)
                    {
                        Console.Write($"Modell ({carToUpdate.Model}): ");
                        string newModel = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newModel))
                            carToUpdate.Model = newModel;

                        Console.Write($"Teljes távolság ({carToUpdate.TotalDistance}): ");
                        string totalDistStr = Console.ReadLine();
                        if (!string.IsNullOrEmpty(totalDistStr))
                            carToUpdate.TotalDistance = decimal.Parse(totalDistStr);

                        Console.Write($"Távolság az utolsó karbantartás óta ({carToUpdate.DistanceSinceLastMaintenance}): ");
                        string distSinceMaintenanceStr = Console.ReadLine();
                        if (!string.IsNullOrEmpty(distSinceMaintenanceStr))
                            carToUpdate.DistanceSinceLastMaintenance = decimal.Parse(distSinceMaintenanceStr);

                        carService.Update(carToUpdate);
                        Console.WriteLine("Autó sikeresen frissítve!");
                    }
                    else
                    {
                        Console.WriteLine("Nem található ilyen ID-jű autó!");
                    }
                    break;

                case "3":
                    // Autó törlése
                    Console.Write("Adja meg a törlendő autó ID-jét: ");
                    int carIdToDelete = int.Parse(Console.ReadLine());

                    carService.Delete(carIdToDelete);
                    Console.WriteLine("Autó sikeresen törölve!");
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Érvénytelen választás!");
                    break;
            }

            WaitForKeyPress();
        }

        static void ManageCustomers(IServiceProvider serviceProvider)
        {
            CustomerService customerService = serviceProvider.GetService<CustomerService>();

            Console.Clear();
            Console.WriteLine("=== ÜGYFELEK KEZELÉSE ===");
            Console.WriteLine("1. Új ügyfél hozzáadása");
            Console.WriteLine("2. Ügyfél módosítása");
            Console.WriteLine("3. Ügyfél törlése");
            Console.WriteLine("0. Vissza");
            Console.Write("\nVálasszon egy opciót: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // Új ügyfél hozzáadása
                    Customer newCustomer = new Customer();
                    Console.Write("ID: ");
                    newCustomer.Id = int.Parse(Console.ReadLine());
                    Console.Write("Név: ");
                    newCustomer.Name = Console.ReadLine();
                    Console.Write("Egyenleg: ");
                    newCustomer.Balance = decimal.Parse(Console.ReadLine());

                    customerService.Insert(newCustomer);
                    Console.WriteLine("Ügyfél sikeresen hozzáadva!");
                    break;

                case "2":
                    // Ügyfél módosítása
                    Console.Write("Adja meg a módosítandó ügyfél ID-jét: ");
                    int customerId = int.Parse(Console.ReadLine());

                    ICustomerDataProvider customerRepo = serviceProvider.GetService<ICustomerDataProvider>();
                    Customer customerToUpdate = customerRepo.Read(customerId);

                    if (customerToUpdate != null)
                    {
                        Console.Write($"Név ({customerToUpdate.Name}): ");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newName))
                            customerToUpdate.Name = newName;

                        Console.Write($"Egyenleg ({customerToUpdate.Balance}): ");
                        string balanceStr = Console.ReadLine();
                        if (!string.IsNullOrEmpty(balanceStr))
                            customerToUpdate.Balance = decimal.Parse(balanceStr);

                        customerService.Update(customerToUpdate);
                        Console.WriteLine("Ügyfél sikeresen frissítve!");
                    }
                    else
                    {
                        Console.WriteLine("Nem található ilyen ID-jű ügyfél!");
                    }
                    break;

                case "3":
                    // Ügyfél törlése
                    Console.Write("Adja meg a törlendő ügyfél ID-jét: ");
                    int customerIdToDelete = int.Parse(Console.ReadLine());

                    customerService.Delete(customerIdToDelete);
                    Console.WriteLine("Ügyfél sikeresen törölve!");
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Érvénytelen választás!");
                    break;
            }

            WaitForKeyPress();
        }

        static void ManageTrips(IServiceProvider serviceProvider)
        {
            TripService tripService = serviceProvider.GetService<TripService>();

            Console.Clear();
            Console.WriteLine("=== UTAZÁSOK KEZELÉSE ===");
            Console.WriteLine("1. Új utazás hozzáadása");
            Console.WriteLine("2. Utazás módosítása");
            Console.WriteLine("3. Utazás törlése");
            Console.WriteLine("0. Vissza");
            Console.Write("\nVálasszon egy opciót: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // Új utazás hozzáadása
                    Trip newTrip = new Trip();
                    Console.Write("ID: ");
                    newTrip.Id = int.Parse(Console.ReadLine());
                    Console.Write("Autó ID: ");
                    newTrip.CarId = int.Parse(Console.ReadLine());
                    Console.Write("Ügyfél ID: ");
                    newTrip.CustomerId = int.Parse(Console.ReadLine());
                    Console.Write("Távolság: ");
                    newTrip.Distance = decimal.Parse(Console.ReadLine());
                    Console.Write("Költség: ");
                    newTrip.Cost = decimal.Parse(Console.ReadLine());

                    tripService.Insert(newTrip);
                    Console.WriteLine("Utazás sikeresen hozzáadva!");
                    break;

                case "2":
                    // Utazás módosítása
                    Console.Write("Adja meg a módosítandó utazás ID-jét: ");
                    int tripId = int.Parse(Console.ReadLine());

                    ITripDataProvider tripRepo = serviceProvider.GetService<ITripDataProvider>();
                    Trip tripToUpdate = tripRepo.Read(tripId);

                    if (tripToUpdate != null)
                    {
                        Console.Write($"Autó ID ({tripToUpdate.CarId}): ");
                        string carIdStr = Console.ReadLine();
                        if (!string.IsNullOrEmpty(carIdStr))
                            tripToUpdate.CarId = int.Parse(carIdStr);

                        Console.Write($"Ügyfél ID ({tripToUpdate.CustomerId}): ");
                        string customerIdStr = Console.ReadLine();
                        if (!string.IsNullOrEmpty(customerIdStr))
                            tripToUpdate.CustomerId = int.Parse(customerIdStr);

                        Console.Write($"Távolság ({tripToUpdate.Distance}): ");
                        string distanceStr = Console.ReadLine();
                        if (!string.IsNullOrEmpty(distanceStr))
                            tripToUpdate.Distance = decimal.Parse(distanceStr);

                        Console.Write($"Költség ({tripToUpdate.Cost}): ");
                        string costStr = Console.ReadLine();
                        if (!string.IsNullOrEmpty(costStr))
                            tripToUpdate.Cost = decimal.Parse(costStr);

                        tripService.Update(tripToUpdate);
                        Console.WriteLine("Utazás sikeresen frissítve!");
                    }
                    else
                    {
                        Console.WriteLine("Nem található ilyen ID-jű utazás!");
                    }
                    break;

                case "3":
                    // Utazás törlése
                    Console.Write("Adja meg a törlendő utazás ID-jét: ");
                    int tripIdToDelete = int.Parse(Console.ReadLine());

                    tripService.Delete(tripIdToDelete);
                    Console.WriteLine("Utazás sikeresen törölve!");
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Érvénytelen választás!");
                    break;
            }

            WaitForKeyPress();
            ;
        }

                    /*InitDatabase(serviceProvider);

                    Listing(serviceProvider);

                    StartTripProgram(serviceProvider);

                    Listing(serviceProvider);

                    Reports(serviceProvider);*/



                    
        

        public static void StartTripProgram(IServiceProvider serviceProvider)
        { 
            StartTrip startTrip = serviceProvider.GetService<StartTrip>();

            startTrip.canStart += () => Console.WriteLine("Az utazas nem kezdheto meg, az egyenlege nem eri el a 40 eurot");
            startTrip.maintance20 += () => Console.WriteLine("Random karbantartas");
            startTrip.maintance200km += () => Console.WriteLine("Rendszeres karbantartas, elerte a 200km-t");

            Console.WriteLine("A bérlés megkezdéséhez, adjon meg néhány adatot!");
            Console.Write("Car Id: ");
            int carId = int.Parse(Console.ReadLine());
            Console.Write("Customer Id: ");
            int customerId = int.Parse(Console.ReadLine());


            startTrip.CanStartTrip(customerId);
            startTrip.Start(carId, customerId);
            Console.Write("Adja meg, hany km-t veztett: ");
            int distance = int.Parse(Console.ReadLine());

            var cost = startTrip.EndTrip(distance);
            Console.WriteLine($"Az on koltsege: {cost}");
        
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
            Console.WriteLine($"Legtobbet futott jarmu: {mostDrivenCar.Model} (ID: {mostDrivenCar.Id}) - {mostDrivenCar.TotalDistance} km");

            var averageCarDistance = queries.GetAverageCarDistance();
            Console.WriteLine($"Autok atlagos futása: {averageCarDistance} km");

            var top10HighestPayingCustomer = queries.GetTop10HighestPayingCustomers();
            Console.WriteLine("Top10 legtobbet fizeto Ugyfel");
            int rank = 1;
            foreach (var item in top10HighestPayingCustomer)
            {
                Console.WriteLine($"{rank}. {item.Customer.Name} (ID: {item.Customer.Id}) - Osszes koltes: {item.TotalSpent} EUR");
                rank++;
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
