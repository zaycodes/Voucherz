using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Util;

namespace VoucherServiceBL.Repository.Mongo
{
    public class MongoGiftRepository : BaseMongoRepository, IGiftRepository
    {
        private ILogger<Gift> _logger;
        public MongoGiftRepository(MongoClient client, IConfiguration config, ILogger<Gift> logger) : base(client, config)
        {
            _logger = logger;
            // BsonClassMap.RegisterClassMap<Value>();

        }
        public async Task<int> CreateGiftVoucherAsync(Gift gift)
        {
            BackgroundJob.Enqueue(() => MyAsyncMethod(gift));
            return 1;
        }

        public async Task MyAsyncMethod(Gift gift)
        {
            await _vouchers.InsertOneAsync(gift);
        }

        public async Task<int> CreateGiftVoucherAsync(IList<Gift> vouchersList)
        {
            BackgroundJob.Enqueue(() => MyAsyncMethod(vouchersList));
            return vouchersList.Count;
        }

        public async Task MyAsyncMethod(IList<Gift> vouchersList)
        {
            await _vouchers.InsertManyAsync(vouchersList);
        }

        public async Task<IEnumerable<Gift>> GetAllGiftVouchersAsync(string merchantId)
        {
            var filter = Builders<Voucher>.Filter.Where(g => g.MerchantId == merchantId && 
                                            g.VoucherType.ToUpper() == VoucherType.GIFT.ToString());
            var cursor = await _vouchers.FindAsync<Gift>(filter);

            var gifts = await cursor.ToListAsync();
            return gifts;
        }

        public async Task<Gift> GetGiftVoucherAsync(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter.Where(v =>
                                           v.Code == voucher.Code && v.MerchantId == voucher.MerchantId &&
                                           v.VoucherType == voucher.VoucherType);
            var voucherCursor = await _vouchers.FindAsync<Gift>(filter);
            return await voucherCursor.FirstOrDefaultAsync();
        }

        public async Task<int?> UpdateGiftVoucherAmountAsync(Gift gift)
        {

            var filter = Builders<Voucher>.Filter.Eq("code", gift.Code);
            var updateDef = Builders<Voucher>.Update.Set("gift_balance", gift.GiftBalance)
                                                 .Set("gift_amount", gift.GiftAmount);       ;

            var cursor = await _vouchers.UpdateOneAsync(filter, updateDef);

            return (int) cursor.ModifiedCount;
        }

        public async Task<int?> UpdateGiftVoucherBalanceAsync(Gift gift)
        {
           

            var filter = Builders<Voucher>.Filter.Eq("code", gift.Code);
            var updateDef = Builders<Voucher>.Update.Set("gift_balance", gift.GiftBalance);

            var cursor = await _vouchers.UpdateOneAsync(filter, updateDef);

            return (int)cursor.ModifiedCount; 
        }
    }
}