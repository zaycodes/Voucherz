using System;
using System.Collections.Generic;
using System.Numerics;
using VoucherServiceBL.Domain;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace VoucherServiceBL.Repository.SqlServer
{
    public class GiftRepository :BaseRepository, IGiftRepository
    {
        public GiftRepository(IConfiguration configuration):base(configuration) {}
        public async Task<int> CreateGiftVoucherAsync(Gift voucher)
        {
            using (var connection = Connection)
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                string storedProcedure = "usp_CreateGiftVoucher";
                var parameters = new DynamicParameters();
                parameters.Add("@HashedCode", voucher.Code);
                parameters.Add("@ExpiryDate", voucher.ExpiryDate);
                parameters.Add("@MerchantId", voucher.MerchantId);
                parameters.Add("@GiftAmount", voucher.GiftAmount);

                return await connection.ExecuteAsync(storedProcedure, parameters, 
                    commandType: CommandType.StoredProcedure);
            }
        }

    public Task<int> CreateGiftVoucherAsync(IList<Gift> vouchersList)
    {

        GiftStreamingSqlRecord record = new GiftStreamingSqlRecord(vouchersList);

        foreach (var t in vouchersList)
        {
            Console.WriteLine($"<<<<<gfts>>> {t}");
        }

        try
        {
            var connection = Connection;
            
            if (connection.State == ConnectionState.Closed) connection.Open();

            string storedProcedure = "dbo.usp_CreateGiftVoucher";

            var command = new SqlCommand(storedProcedure, connection as SqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            var param = new SqlParameter();
            param.ParameterName = "@tblGift";
            param.TypeName = "dbo.GiftVoucherType";   
            param.SqlDbType = SqlDbType.Structured;             
            param.Value = record;

            command.Parameters.Add(param);
            command.CommandTimeout = 60;
            return command.ExecuteNonQueryAsync();                 
            
        }
        catch (System.Exception)
        {
            throw;
        }
        finally
        {
            Connection.Close();
        }

    }

        public async Task<IEnumerable<Gift>> GetAllGiftVouchersAsync(string merchantId)
        {
            using (var connection = Connection)
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                var storedProcedure = "usp_GetAllGiftVouchersFilterByMerchantId";
                var parameters = new DynamicParameters();
                parameters.Add("@MerchantId", merchantId);

                return await connection.QueryAsync<Gift>(storedProcedure, parameters, commandType:CommandType.StoredProcedure);
            }
        }

        public async Task<Gift> GetGiftVoucherAsync(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_CreateDiscountVoucher"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);
                return await conn.QuerySingleAsync<Gift>("usp_GetVoucherByCodeFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }


        public async Task<int?> UpdateGiftVoucherAmountAsync(Gift voucher)
        {
            using (var connection = Connection)
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                var storedProcedure = "usp_UpdateGiftAmountByCode";
                var parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@GiftAmount", voucher.GiftAmount);
                return await connection.ExecuteAsync(storedProcedure, parameters, commandType:CommandType.StoredProcedure);
            }
        }

        public Task<int?> UpdateGiftVoucherBalanceAsync(Gift gift)
        {
            throw new NotImplementedException();
        }
    }
}