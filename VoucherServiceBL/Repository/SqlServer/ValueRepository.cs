using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository.SqlServer
{
    public class ValueRepository : BaseRepository, IValueRepository
    {
       public ValueRepository(IConfiguration config) : base(config)
        {
        }

        /// <summary>
        /// Create Value Voucher repository handler
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<int> CreateValueVoucherAsync(Value value)
        {        
                using (var conn = Connection)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    //Parameters Declaration to be passed into Stored procdure "usp_CreateVoucher"..
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@HashedCode", value.Code);
                    parameters.Add("@MerchantId", value.MerchantId);
                    parameters.Add("@ValueAmount", value.ValueAmount);
                    parameters.Add("@ExpiryDate", value.ExpiryDate);

                    return await conn.ExecuteAsync("usp_CreateValueVoucher", parameters, commandType: CommandType.StoredProcedure);
                }
        }

        public Task<int> CreateValueVoucherAsync(IList<Value> vouchersList)
        {

            ValueStreamingSqlRecord record = new ValueStreamingSqlRecord(vouchersList);

            //foreach (var t in vouchersList)
            //{
            //    Console.WriteLine($"<<<<<value>>> {t}");
            //}

            try
            {
                var connection = Connection;

                if (connection.State == ConnectionState.Closed) connection.Open();

                string storedProcedure = "dbo.usp_CreateValueVoucher";

                var command = new SqlCommand(storedProcedure, connection as SqlConnection);
                command.CommandType = CommandType.StoredProcedure;

                var param = new SqlParameter();
                param.ParameterName = "@tblValue";
                param.TypeName = "dbo.ValueVoucherType";
                param.SqlDbType = SqlDbType.Structured;
                param.Value = record;

                command.Parameters.Add(param);
                command.CommandTimeout = 120;
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


        /// <summary>
        /// Read All Value Vouchers filtered by a MerchantId From Table handler
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Value>> GetAllValueVouchersAsync(string merchantId)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_GetAllValueVouchersFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MerchantId", merchantId);
                return await conn.QueryAsync<Value>("usp_GetAllValueVouchersFilterByMerchantId",parameters, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Read All details of a Value Voucher filtered by a MerchantId From Table handler
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns></returns>
        public async Task<Value> GetValueVoucherAsync(Voucher voucher)
        {
            using (var conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                //Parameters Declaration to be passed into Stored procdure "usp_GetVoucherByCodeFilterByMerchantId"..
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", voucher.Code);
                parameters.Add("@VoucherType", voucher.VoucherType);
                parameters.Add("@MerchantId", voucher.MerchantId);
                return await conn.QuerySingleAsync<Value>("usp_GetVoucherByCodeFilterByMerchantId", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
