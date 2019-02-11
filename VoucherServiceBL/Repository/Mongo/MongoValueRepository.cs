using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Util;

namespace VoucherServiceBL.Repository.Mongo
{
    public class MongoValueRepository : BaseMongoRepository, IValueRepository
    {
        private ILogger<Value> _logger;
        public MongoValueRepository(MongoClient client, IConfiguration config, ILogger<Value> logger):base(client, config)
            {
                _logger = logger;

            }
        public async Task<int> CreateValueVoucherAsync(Value value)
        {
            BackgroundJob.Enqueue(() => MyAsyncMethod(value));
            return 1;
        }

        public async Task MyAsyncMethod(Value value)
        {
            await _vouchers.InsertOneAsync(value);
        }

        public async  Task<int> CreateValueVoucherAsync(IList<Value> vouchersList)
        {
            BackgroundJob.Enqueue(() => MyAsyncMethod(vouchersList));
            return vouchersList.Count;
        }

        public async Task MyAsyncMethod(IList<Value> vouchersList)
        {
            await _vouchers.InsertManyAsync(vouchersList);
        }

        public async Task<IEnumerable<Value>> GetAllValueVouchersAsync(string merchantId)
        {
            var filter = Builders<Voucher>.Filter.Where(g => g.MerchantId == merchantId &&
                                             g.VoucherType.ToUpper() == VoucherType.VALUE.ToString());
            var cursor = await _vouchers.FindAsync<Value>(filter);

            var values = await cursor.ToListAsync();
            return values;
        }

        public async Task<Value> GetValueVoucherAsync(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter.Where(v =>
                                            v.Code == voucher.Code && v.MerchantId == voucher.MerchantId &&
                                            v.VoucherType == voucher.VoucherType);
            var voucherCursor = await _vouchers.FindAsync<Value>(filter);
            return await voucherCursor.FirstOrDefaultAsync();
        }
    }
}