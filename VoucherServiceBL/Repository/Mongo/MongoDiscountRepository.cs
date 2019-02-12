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
    public class MongoDiscountRepository :BaseMongoRepository, IDiscountRepository
    {

        private ILogger<Discount> _logger;
        public MongoDiscountRepository(MongoClient client, IConfiguration config, ILogger<Discount> logger) : base(client, config)
        {
            _logger = logger;
            // BsonClassMap.RegisterClassMap<Value>();

        }
        public async Task<int> CreateDiscountVoucherAsync(Discount discount)
        {
            BackgroundJob.Enqueue(() => MyAsyncMethod(discount));
            return 1;
        }

        public async Task MyAsyncMethod(Discount discount)
        {
           await _vouchers.InsertOneAsync(discount);
        }

        public async Task<int> CreateDiscountVoucherAsync(IList<Discount> vouchersList)
        {
            BackgroundJob.Enqueue(() => MyAsyncMethod(vouchersList)); 
            return vouchersList.Count;
        }

        public async Task MyAsyncMethod(IList<Discount> vouchersList)
        {
            await _vouchers.InsertManyAsync(vouchersList);
        }

        public async Task<IEnumerable<Discount>> GetAllDiscountVouchersFilterByMerchantIdAsync(string merchantId)
        {
            var filter = Builders<Voucher>.Filter.Where(g => g.MerchantId == merchantId &&
                                            g.VoucherType.ToUpper() == VoucherType.DISCOUNT.ToString());
            var cursor = await _vouchers.FindAsync<Discount>(filter);

            var discounts = await cursor.ToListAsync();
            return discounts;
        }

        public async Task<Discount> GetDiscountVoucherAsync(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter.Where(v =>
                               v.Code == voucher.Code && v.MerchantId == voucher.MerchantId &&
                               v.VoucherType == voucher.VoucherType);

            var voucherCursor = await _vouchers.FindAsync<Discount>(filter);
            var res = await voucherCursor.FirstOrDefaultAsync();
            return res;
        }
        
        public async Task<int> UpdateRedemptionCount(Discount discount)
        {
            string encryptedCode = CodeGenerator.Encrypt(discount.Code);
            var filter = Builders<Voucher>.Filter.Eq("code", encryptedCode);
            discount.RedemptionCount = discount.RedemptionCount + 1;
            var updateDef = Builders<Voucher>.Update.Set("redemption_count", discount.RedemptionCount);

            var cursor = await _vouchers.UpdateOneAsync(filter, updateDef);

            return (int)cursor.ModifiedCount;
        }
    }
}
