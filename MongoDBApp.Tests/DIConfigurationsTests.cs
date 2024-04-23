using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MongoDBApp.Infrastructure;
using MongoDB.Driver;
using MongoDBApp.Infrastructure.Helpers;
using MongoDBApp.Infrastructure.Interfaces;

namespace MongoDBApp.Tests
{
    [TestClass]
    public class DIConfigurationsTests
    {
        [TestMethod]
        public void RegisterDatabaseDependencies_Adds_MongoClient_AsSingleton()
        {
            var services = new ServiceCollection();
            var configurationMock = new Mock<IConfigurationRoot>();
            configurationMock.Setup(config => config.GetConnectionString("MyDB")).Returns("mongodb://localhost:27017");

            // Act
            services.RegisterDatabaseDependencies(configurationMock.Object);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var mongoClient1 = serviceProvider.GetService<IMongoClient>();
            var mongoClient2 = serviceProvider.GetService<IMongoClient>();

            Assert.IsNotNull(mongoClient1);
            Assert.IsNotNull(mongoClient2);
            Assert.AreSame(mongoClient1, mongoClient2); // MongoClient should be registered as Singleton
        }

        [TestMethod]
        public void RegisterDatabaseDependencies_Adds_MongoDatabase_AsScoped()
        {
            // Arrange
            var services = new ServiceCollection();
            var mongoClientMock = new Mock<IMongoClient>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(provider => provider.GetService(typeof(IMongoClient))).Returns(mongoClientMock.Object);
            // Act
            services.AddScoped<IMongoDatabase>(serviceProvider =>
            {
                var client = (IMongoClient)serviceProvider.GetService(typeof(IMongoClient));
                return client.GetDatabase("MyDB");
            });

            // Assert
            var serviceProvider = services.BuildServiceProvider();

            Assert.IsNotNull(serviceProvider);
        }

        [TestMethod]
        public void Test_DI_Registration()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddScoped<INumberHelper, NumberHelper>();

            // Act
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var dataService = serviceProvider.GetService<INumberHelper>();
            Assert.IsNotNull(dataService);
        }

        [TestMethod]
        public void Test_Singleton_Service_Registration()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<INumberHelper, NumberHelper>(); // Registering SingletonService

            // Act
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var singletonService1 = serviceProvider.GetService<INumberHelper>();
            var singletonService2 = serviceProvider.GetService<INumberHelper>();

            Assert.IsNotNull(singletonService1);
            Assert.AreSame(singletonService1, singletonService2);
        }

        [TestMethod]
        public void Test_Scoped_Service_Registration()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddScoped<INumberHelper, NumberHelper>(); // Registering ScopedService

            // Act
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedService1 = scope.ServiceProvider.GetService<INumberHelper>();
                var scopedService2 = scope.ServiceProvider.GetService<INumberHelper>();

                Assert.IsNotNull(scopedService1);
                Assert.AreSame(scopedService1, scopedService2);
            }
        }

        [TestMethod]
        public void Test_Transient_Service_Registration()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<INumberHelper, NumberHelper>(); // Registering TransientService

            // Act
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var transientService1 = serviceProvider.GetService<INumberHelper>();
            var transientService2 = serviceProvider.GetService<INumberHelper>();

            Assert.IsNotNull(transientService1);
            Assert.IsNotNull(transientService2);
            Assert.AreNotSame(transientService1, transientService2);
        }
    }
}