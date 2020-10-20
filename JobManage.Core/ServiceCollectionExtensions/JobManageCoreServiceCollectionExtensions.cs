using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace JobManage.Core
{
    public static class JobManageCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            services.AddSingleton<IMongoDbContext, MongoDbContext>(factory =>
            {
                var context = new MongoDbContext();
                context.InitializeDatabase(database);
                return context;
            });

            services.AddTransient(typeof(MongoDbRepository<>));
            services.AddTransient<JobRepository>();
            services.AddTransient<JobRunLogRepository>();

            return services;
        }
    }
}

