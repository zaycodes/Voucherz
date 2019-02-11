using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Util;

namespace VoucherServiceBL.Repository.Mongo
{
    public class MongoVoucherRepository :BaseMongoRepository, IVoucherRepository
    {
        public MongoVoucherRepository(MongoClient client, IConfiguration config):base(client, config)
        {}
        
        public Task DeleteVoucherByCodeAsync(string code)
        {
            return _vouchers.DeleteOneAsync(v => v.Code == code);
        }

        public async Task<IEnumerable<Voucher>> GetAllVouchersFilterByMerchantIdAsync(string merchantId)
        {
            var filter = Builders<Voucher>.Filter.Eq("merchant_id", merchantId);
            IFindFluent<Voucher, Voucher> cursor = ConstructProjection(filter);
            return await cursor.ToListAsync();
        }

        private IFindFluent<Voucher, Voucher> ConstructProjection(FilterDefinition<Voucher> filter)
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

        public async Task<Voucher> GetVoucherByCodeAsync(string code)
        {
            var filter = Builders<Voucher>.Filter.Eq("code", code);
            IFindFluent<Voucher, Voucher> cursor = ConstructProjection(filter);
            var res = await cursor.SingleOrDefaultAsync<Voucher>();
            return res;
        }
    
        public async Task<Voucher> GetVoucherByCodeFilterByMerchantIdAsync(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter
                    .Where(v => v.Code == voucher.Code && v.MerchantId == voucher.MerchantId);
            IFindFluent<Voucher, Voucher> cursor = ConstructProjection(filter);            

            return await cursor.SingleOrDefaultAsync();
        }

        public async Task<Voucher> GetVoucherByCreationDateAsync(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter.Eq("creation_date",voucher.CreationDate);
            IFindFluent<Voucher, Voucher> cursor = ConstructProjection(filter);            
            return await cursor.FirstAsync();
        }

        public async Task<Voucher> GetVoucherByCreationDateFilterByMerchantIdAsync(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter
                        .Where(v => v.CreationDate == voucher.CreationDate && 
                               v.MerchantId == voucher.MerchantId);
            IFindFluent<Voucher, Voucher> cursor = ConstructProjection(filter);
            return await cursor.FirstAsync();
        }

        public async Task<Voucher> GetVoucherByExpiryDateAsync(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter.Eq("expiry_date", voucher.ExpiryDate);
            IFindFluent<Voucher, Voucher> cursor = ConstructProjection(filter);            
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<Voucher> GetVoucherByExpiryDateFilterByMerchantIdAsync(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter
                            .Where(v => v.ExpiryDate == voucher.ExpiryDate && 
                                   v.MerchantId == voucher.MerchantId);
            IFindFluent<Voucher, Voucher> cursor = ConstructProjection(filter);
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<Voucher> GetVoucherByMerchantIdAsync(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter.Eq("merchant_id", voucher.MerchantId);
            IFindFluent<Voucher, Voucher> cursor = ConstructProjection(filter);
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<Voucher> GetVoucherByStatusAsync(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter.Eq("merchant_id", voucher.MerchantId);
            IFindFluent<Voucher, Voucher> cursor = ConstructProjection(filter);
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<long> UpdateVoucherExpiryDateByCodeAsync(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter.Eq("code", voucher.Code);
            var updateDef = Builders<Voucher>.Update.Set(v => v.ExpiryDate, voucher.ExpiryDate);
            var updateResult =  await _vouchers.UpdateOneAsync( filter, updateDef, 
                    new UpdateOptions() {IsUpsert = false});

            return updateResult.ModifiedCount;  //TODO: might throw handle error        
        }

        public async Task<long> UpdateVoucherStatusByCodeAsync(Voucher voucher)
        {
            var filter = Builders<Voucher>.Filter.Eq("code", voucher.Code);

            var updateDef = Builders<Voucher>.Update.Set(v => v.VoucherStatus, 
                voucher.VoucherStatus);
            var updateResult = await _vouchers.UpdateOneAsync(filter, updateDef);


          return updateResult.ModifiedCount;  //TODO: might throw handle error 
        }
    }
}