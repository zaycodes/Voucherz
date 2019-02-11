using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository.SqlServer
{
    public class DiscountRepository : BaseRepository,IDiscountRepository
    {
        public DiscountRepository(IConfiguration config) : base(config)
        {
        }

        /// <summary>
        /// Create Discount Voucher repository handler
        /// </summary>
        /// <param name="discount"></param>
        /// <returns></returns>
        public async Task<int> CreateDiscountVoucherAsync(Discount discount)
        {        
                using (var conn = Connection)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    //Parameters Declaration to be passed into Stored procdure "usp_CreateDiscountVoucher"..
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@HashedCode", discount.Code);
                    parameters.Add("@MerchantId", discount.MerchantId);
                    parameters.Add("@DiscountAmount", discount.DiscountAmount);
                    parameters.Add("@DiscountPercentage", discount.DiscountPercentage);
                    parameters.Add("@DiscountUnit", discount.DiscountUnit);
                    parameters.Add("@ExpiryDate", discount.ExpiryDate);

                   return await conn.ExecuteAsync("usp_CreateDiscountVoucher", parameters, commandType: CommandType.StoredProcedure);
                }
        }

        public async Task<int> CreateDiscountVoucherAsync(IList<Discount> vouchersList)
        {

            DiscountStreamingSqlRecord record = new DiscountStreamingSqlRecord(vouchersList);

            //foreach (var t in vouchersList)
            //{
            //    Console.WriteLine($"<<<<<discount>>> {t}");
            //}

            try
            {
                var connection = Connection;

                if (connection.State == ConnectionState.Closed) connection.Open();

                string storedProcedure = "dbo.usp_CreateDiscountVoucher";

                var command = new SqlCommand(storedProcedure, connection as SqlConnection);
                command.CommandType = CommandType.StoredProcedure;

                var param = new SqlParameter();
                param.ParameterName = "@tblDiscount";
                param.TypeName = "dbo.DiscountVoucherType";
                param.SqlDbType = SqlDbType.Structured;
                param.Value = record;

                command.Parameters.Add(param);
                command.CommandTimeout = 120;
                return await command.ExecuteNonQueryAsync();

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


        /// <summary>
        /// Read Discount Voucher From Table handler
        /// </summary>
        /// <param name="discount"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Discount>> GetAllDiscountVouchersFilterByMerchantIdAsync(
                                                                string merchantId)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_CreateDiscountVoucher"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MerchantId", merchantId);
                return await conn.QueryAsync<Discount>("usp_GetAllDiscountVouchersFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }
 
        public async Task<Discount> GetDiscountVoucherAsync(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);
                return await conn.QuerySingleAsync<Discount>("usp_GetVoucherByCodeFilterByMerchantId",parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public Task<int> UpdateRedemptionCount(Discount discount)
        {
            throw new NotImplementedException();
        }
    }

}
