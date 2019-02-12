using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Util;

namespace VoucherServiceBL.Repository.Mongo
{
    public class MongoAdminRepository :BaseMongoRepository, IAdminRepo
    {
        public MongoAdminRepository(MongoClient client, IConfiguration configuration) : base(client, configuration)
        {}

        public async Task<IList<Discount>> GetAllDiscountVouchers()
        {
            var filter = Builders<Voucher>.Filter.Empty;
            var cursor = await _vouchers.FindAsync<Discount>(filter);

            var discounts = await cursor.ToListAsync();
            return discounts;

        }

        private  IFindFluent<Voucher, Voucher> ConstructProjection(FilterDefinition<Voucher> filter)
        {
            var projection = Builders<Voucher>.Projection
                                    .Include("_id")
                                    .Include("code")
                                    .Include("voucher_type")
                                    .Include("expiry_date")
                                    .Include("creation_date")
                                    .Include("merchant_id")
                                    .Include("voucher_status")
                                    .Include("Metadata")
                                    .Include("description");
            var cursor = _vouchers.Find<Voucher>(filter)
                                        .Project<Voucher>(projection);
            return cursor;
        }

        public async Task<IList<Gift>> GetAllGiftVouchers()
        {
             var filter = Builders<Voucher>.Filter.Empty;
            var cursor = await _vouchers.FindAsync<Gift>(filter);

            var gifts = await cursor.ToListAsync();
            return gifts;
        }

        public async Task<IList<Value>> GetAllValueVouchers()
        {
            var filter = Builders<Voucher>.Filter.Empty;
            var cursor = await _vouchers.FindAsync<Value>(filter);

            var values = await cursor.ToListAsync();
            return values;
        }

        public async Task<IList<Voucher>> GetAllVouchers()
        {
            var filter = Builders<Voucher>.Filter.Empty;
            IFindFluent<Voucher, Voucher> cursor = ConstructProjection(filter);
            return await cursor.ToListAsync();
        }

        public async Task<long> GetTotalVoucherPerMonth(string type = "VOUCHER", int month = 1, int? year = null)
        {
            //count the num of vouchers of a particular type in a particular month
            var filter = Builders<Voucher>.Filter.Where(v => 
                    v.VoucherType == (type ?? "VOUCHER") &&
                    v.CreationDate.Month == month &&
                    v.CreationDate.Year == (year ?? DateTime.Now.Year)
            );
            return await _vouchers.CountDocumentsAsync(filter);
        }
    }
}