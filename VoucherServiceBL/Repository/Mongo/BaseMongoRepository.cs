using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository.Mongo
{
    public abstract class BaseMongoRepository
    {
        protected IMongoDatabase _database;
        protected IMongoCollection<Voucher> _vouchers;
        public BaseMongoRepository(MongoClient client, IConfiguration configuration)
        {   
            _database = client.GetDatabase(configuration.GetSection("MongoDatabase").Value);
            _vouchers = _database.GetCollection<Voucher>(nameof(_vouchers));
        }
    }
}
