using Moq;
using NUnit.Framework;
using W31UL9_HSZF_2024252.Application;
using W31UL9_HSZF_2024252.Application.Services;
using W31UL9_HSZF_2024252.Model;
using W31UL9_HSZF_2024252.Persistence.MsSql.Interfaces;

namespace W31UL9_HSZF_2024252.Test
{
    [TestFixture]
    public class StartTripTests
    {
        [Test]
        public void CanStartTrip_CustomerBalanceBelow40_ReturnsFalse()
        {
            // Arrange
            var mockCustomerRepo = new Mock<ICustomerDataProvider>();
            var mockTripRepo = new Mock<ITripDataProvider>();
            var mockCarRepo = new Mock<ICarDataProvider>();

            mockCustomerRepo.Setup(repo => repo.Read(It.IsAny<int>()))
                .Returns(new Customer { Id = 1, Name = "Test Customer", Balance = 30m });

            var startTrip = new StartTrip(mockCustomerRepo.Object, mockTripRepo.Object, mockCarRepo.Object);

            bool canStartEventFired = false;
            startTrip.canStart += () => canStartEventFired = true;

            // Act
            bool result = startTrip.CanStartTrip(1);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(canStartEventFired, Is.True, "The canStart event should have been triggered");
        }

        [Test]
        public void Start_ValidCustomerAndCar_CreatesNewTrip()
        {
            // Arrange
            var mockCustomerRepo = new Mock<ICustomerDataProvider>();
            var mockTripRepo = new Mock<ITripDataProvider>();
            var mockCarRepo = new Mock<ICarDataProvider>();

            mockCustomerRepo.Setup(repo => repo.Read(It.IsAny<int>()))
                .Returns(new Customer { Id = 1, Name = "Test Customer", Balance = 50m });

            mockTripRepo.Setup(repo => repo.GetAll())
                .Returns(new List<Trip>().AsQueryable());

            var startTrip = new StartTrip(mockCustomerRepo.Object, mockTripRepo.Object, mockCarRepo.Object);

            // Act
            Trip result = startTrip.Start(2, 1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CarId, Is.EqualTo(2));
            Assert.That(result.CustomerId, Is.EqualTo(1));
            Assert.That(result.Distance, Is.EqualTo(0));
            Assert.That(result.Cost, Is.EqualTo(0));

            mockTripRepo.Verify(repo => repo.Insert(It.IsAny<Trip>()), Times.Once);
        }

        [Test]
        public void EndTrip_CalculatesCostCorrectly_And_UpdatesEntities()
        {
            // Arrange
            var mockCustomerRepo = new Mock<ICustomerDataProvider>();
            var mockTripRepo = new Mock<ITripDataProvider>();
            var mockCarRepo = new Mock<ICarDataProvider>();

            Trip testTrip = new Trip { Id = 1, CarId = 2, CustomerId = 3, Distance = 0, Cost = 0 };
            Customer testCustomer = new Customer { Id = 3, Name = "Test Customer", Balance = 100m };
            Car testCar = new Car
            {
                Id = 2,
                Model = "Test Car",
                TotalDistance = 1000m,
                DistanceSinceLastMaintenance = 50m
            };

            mockTripRepo.Setup(repo => repo.Read(1))
                .Returns(testTrip);

            mockCustomerRepo.Setup(repo => repo.Read(3))
                .Returns(testCustomer);

            mockCarRepo.Setup(repo => repo.Read(2))
                .Returns(testCar);

            var startTrip = new StartTrip(mockCustomerRepo.Object, mockTripRepo.Object, mockCarRepo.Object);

            // Act
            decimal cost = startTrip.EndTrip(100m);

            // Assert
            // Cost calculation: baseFee (0.5) + distance (100) * ratePerKm (0.35) = 35.5
            Assert.That(cost, Is.EqualTo(35.5m));
            Assert.That(testTrip.Distance, Is.EqualTo(100m));
            Assert.That(testTrip.Cost, Is.EqualTo(35.5m));
            Assert.That(testCustomer.Balance, Is.EqualTo(64.5m)); // 100 - 35.5 = 64.5
            Assert.That(testCar.TotalDistance, Is.EqualTo(1100m)); // 1000 + 100 = 1100
            Assert.That(testCar.DistanceSinceLastMaintenance, Is.EqualTo(150m)); // 50 + 100 = 150

            mockTripRepo.Verify(repo => repo.Update(It.IsAny<Trip>()), Times.Once);
            mockCustomerRepo.Verify(repo => repo.Update(It.IsAny<Customer>()), Times.Once);
            mockCarRepo.Verify(repo => repo.Update(It.IsAny<Car>()), Times.Once);
        }

        [Test]
        public void CheckForMaintenance_OverThreshold_PerformsMaintenance()
        {
            // Arrange
            var mockCustomerRepo = new Mock<ICustomerDataProvider>();
            var mockTripRepo = new Mock<ITripDataProvider>();
            var mockCarRepo = new Mock<ICarDataProvider>();

            Trip testTrip = new Trip { Id = 1, CarId = 2, CustomerId = 3, Distance = 0, Cost = 0 };
            Customer testCustomer = new Customer { Id = 3, Name = "Test Customer", Balance = 100m };
            Car testCar = new Car
            {
                Id = 2,
                Model = "Test Car",
                TotalDistance = 1000m,
                DistanceSinceLastMaintenance = 190m // Close to maintenance threshold
            };

            mockTripRepo.Setup(repo => repo.Read(1))
                .Returns(testTrip);

            mockCustomerRepo.Setup(repo => repo.Read(3))
                .Returns(testCustomer);

            mockCarRepo.Setup(repo => repo.Read(2))
                .Returns(testCar);

            var startTrip = new StartTrip(mockCustomerRepo.Object, mockTripRepo.Object, mockCarRepo.Object);

            bool maintenanceEventFired = false;
            startTrip.maintance200km += () => maintenanceEventFired = true;

            // Act
            startTrip.EndTrip(20m); // This should push over the 200km threshold

            // Assert
            Assert.That(maintenanceEventFired, Is.True, "The maintenance event should have been triggered");
            Assert.That(testCar.DistanceSinceLastMaintenance, Is.EqualTo(0m), "The car should have been serviced");
        }
    }

    [TestFixture]
    public class QueriesTests
    {
        [Test]
        public void GetMostDrivenCar_ReturnsCarWithHighestTotalDistance()
        {
            // Arrange
            var mockCustomerRepo = new Mock<ICustomerDataProvider>();
            var mockTripRepo = new Mock<ITripDataProvider>();
            var mockCarRepo = new Mock<ICarDataProvider>();

            var cars = new List<Car>
            {
                new Car { Id = 1, Model = "Toyota", TotalDistance = 1000m },
                new Car { Id = 2, Model = "Honda", TotalDistance = 3000m },
                new Car { Id = 3, Model = "Ford", TotalDistance = 2000m }
            }.AsQueryable();

            mockCarRepo.Setup(repo => repo.GetAll())
                .Returns(cars);

            var queries = new Queries(mockCustomerRepo.Object, mockTripRepo.Object, mockCarRepo.Object);

            // Act
            var result = queries.GetMostDrivenCar();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(2));
            Assert.That(result.Model, Is.EqualTo("Honda"));
            Assert.That(result.TotalDistance, Is.EqualTo(3000m));
        }

        [Test]
        public void GetAverageCarDistance_CalculatesAverageCorrectly()
        {
            // Arrange
            var mockCustomerRepo = new Mock<ICustomerDataProvider>();
            var mockTripRepo = new Mock<ITripDataProvider>();
            var mockCarRepo = new Mock<ICarDataProvider>();

            var cars = new List<Car>
            {
                new Car { Id = 1, Model = "Toyota", TotalDistance = 1000m },
                new Car { Id = 2, Model = "Honda", TotalDistance = 3000m },
                new Car { Id = 3, Model = "Ford", TotalDistance = 2000m }
            }.AsQueryable();

            mockCarRepo.Setup(repo => repo.GetAll())
                .Returns(cars);

            var queries = new Queries(mockCustomerRepo.Object, mockTripRepo.Object, mockCarRepo.Object);

            // Act
            var result = queries.GetAverageCarDistance();

            // Assert
            // Average should be (1000 + 3000 + 2000) / 3 = 2000
            Assert.That(result, Is.EqualTo(2000m));
        }

        [Test]
        public void GetTop10HighestPayingCustomers_ReturnsSortedCustomers()
        {
            // Arrange
            var mockCustomerRepo = new Mock<ICustomerDataProvider>();
            var mockTripRepo = new Mock<ITripDataProvider>();
            var mockCarRepo = new Mock<ICarDataProvider>();

            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "John", Balance = 100m },
                new Customer { Id = 2, Name = "Jane", Balance = 200m },
                new Customer { Id = 3, Name = "Alice", Balance = 300m }
            }.AsQueryable();

            var trips = new List<Trip>
            {
                new Trip { Id = 1, CustomerId = 1, Cost = 50m },
                new Trip { Id = 2, CustomerId = 1, Cost = 30m },
                new Trip { Id = 3, CustomerId = 2, Cost = 40m },
                new Trip { Id = 4, CustomerId = 3, Cost = 100m }
            }.AsQueryable();

            mockCustomerRepo.Setup(repo => repo.GetAll())
                .Returns(customers);

            mockTripRepo.Setup(repo => repo.GetAll())
                .Returns(trips);

            var queries = new Queries(mockCustomerRepo.Object, mockTripRepo.Object, mockCarRepo.Object);

            // Act
            var result = queries.GetTop10HighestPayingCustomers();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));

            // Customer 3 should be first with total spent = 100
            Assert.That(result[0].Customer.Id, Is.EqualTo(3));
            Assert.That(result[0].TotalSpent, Is.EqualTo(100m));

            // Customer 1 should be second with total spent = 80
            Assert.That(result[1].Customer.Id, Is.EqualTo(1));
            Assert.That(result[1].TotalSpent, Is.EqualTo(80m));

            // Customer 2 should be third with total spent = 40
            Assert.That(result[2].Customer.Id, Is.EqualTo(2));
            Assert.That(result[2].TotalSpent, Is.EqualTo(40m));
        }
    }

    [TestFixture]
    public class ServiceTests
    {
        [Test]
        public void CarService_GetAll_ReturnsAllCars()
        {
            // Arrange
            var mockCarRepo = new Mock<ICarDataProvider>();

            var cars = new List<Car>
            {
                new Car { Id = 1, Model = "Toyota" },
                new Car { Id = 2, Model = "Honda" },
                new Car { Id = 3, Model = "Ford" }
            }.AsQueryable();

            mockCarRepo.Setup(repo => repo.GetAll())
                .Returns(cars);

            var carService = new CarService(mockCarRepo.Object);

            // Act
            var result = carService.GetAll();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result, Has.Some.Matches<Car>(c => c.Model == "Toyota"));
            Assert.That(result, Has.Some.Matches<Car>(c => c.Model == "Honda"));
            Assert.That(result, Has.Some.Matches<Car>(c => c.Model == "Ford"));
        }

        [Test]
        public void TripService_Update_CallsRepositoryUpdate()
        {
            // Arrange
            var mockTripRepo = new Mock<ITripDataProvider>();
            var tripService = new TripService(mockTripRepo.Object);
            var trip = new Trip { Id = 1, CarId = 1, CustomerId = 1, Distance = 100m, Cost = 35.5m };

            // Act
            tripService.Update(trip);

            // Assert
            mockTripRepo.Verify(repo => repo.Update(trip), Times.Once);
        }

        [Test]
        public void CustomerService_Delete_CallsRepositoryDelete()
        {
            // Arrange
            var mockCustomerRepo = new Mock<ICustomerDataProvider>();
            var customerService = new CustomerService(mockCustomerRepo.Object);

            // Act
            customerService.Delete(1);

            // Assert
            mockCustomerRepo.Verify(repo => repo.Delete(1), Times.Once);
        }
    }
}