using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace VoucherService
{
    public static class MongoSupport
    {
        public static void AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new 
            MongoClient(configuration.GetSection("ConnectionStrings")["MongoConnString"]));
        }
    }
}