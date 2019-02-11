using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Service;

namespace VoucherServiceBL.Repository.SqlServer
{
    public class BaseRepository:IVoucherRepository
    {
        private static IConfiguration _config;

        public BaseRepository(IConfiguration config) => _config = config;

        public static IDbConnection Connection
        {
            get { return new SqlConnection(_config.GetConnectionString("MainConnString")); }
        }
         
        
        public async Task<IEnumerable<Voucher>> GetAllVouchersFilterByMerchantIdAsync(string merchantId)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_GetAllVouchersFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MerchantId", merchantId);
                return await conn.QueryAsync<Voucher>("usp_GetAllVouchersFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByCodeAsync(string code)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_GetAllVouchersFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", code);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByCode",parameters,commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByCodeFilterByMerchantIdAsync(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_CreateDiscountVoucher"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                // parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);
                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByCodeFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByCreationDateAsync(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
        //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByCreationDate"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CreationDate", voucher.CreationDate);
                parameters.Add("@VoucherType", voucher.VoucherType);
                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByCreationDate",parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByCreationDateFilterByMerchantIdAsync(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByCreationDate"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CreationDate", voucher.CreationDate);
                parameters.Add("@MerchantId", voucher.MerchantId);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByCreationDateFilterByMerchantId",parameters,commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByExpiryDateAsync(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByExpiryDate"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ExpiryDate", voucher.CreationDate);
                parameters.Add("@VoucherType", voucher.VoucherType);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByExpiryDate", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByExpiryDateFilterByMerchantIdAsync(Voucher voucher)
        {

            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByExpiryDateFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ExpiryDate", voucher.ExpiryDate);
                parameters.Add("@MerchantId", voucher.MerchantId);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByExpiryDateFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByIdAsync(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherById"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@VoucherId", voucher.Id);
                parameters.Add("@VoucherType", voucher.VoucherType);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByIdFilterByMerchantIdAsync(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByIdFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@VoucherId", voucher.Id);
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByIdFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public async Task<Voucher> GetVoucherByMerchantIdAsync(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Voucher> GetVoucherByStatusAsync(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                conn.Open();
                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByStatus"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@VoucherStatus", voucher.VoucherStatus);

                return await conn.QuerySingleAsync<Voucher>("usp_GetVoucherByStatus", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<long> UpdateVoucherExpiryDateByCodeAsync(Voucher voucher)
        {
            
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_UpdateVoucherExpiryDateByCode"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@ExpiryDate", voucher.ExpiryDate);

                return await conn.ExecuteAsync("usp_UpdateVoucherExpiryDateByCode", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<long> UpdateVoucherStatusByCodeAsync(Voucher voucher)
        {
            //var rowAffected = 0;
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_UpdateVoucherStatusByCode"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@VoucherStatus", voucher.VoucherStatus);

                return await conn.ExecuteAsync("usp_UpdateVoucherStatusByCode", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteVoucherByCodeAsync(string code)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_DeleteVoucherByCode"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", code);

                await conn.ExecuteAsync("usp_DeleteVoucherByCode", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<long> DeleteVoucherByIdAsync(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_DeleteVoucherById"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@VoucherId", voucher.Id);

                return await conn.ExecuteAsync("usp_DeleteVoucherByCode", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
