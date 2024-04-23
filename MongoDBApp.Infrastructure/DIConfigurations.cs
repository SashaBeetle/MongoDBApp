using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace MongoDBApp.Infrastructure
{
    public static class DIConfigurations
    {
        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("MongoDBConnection");
                return new MongoClient(connectionString);
            });

            services.AddScoped<IMongoDatabase>(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                return client.GetDatabase("MyDB");
            });
        }


    }
}
