using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository
{
    public interface IValueRepository
    {
   
        #region Create Method

        /// <summary>
        /// Create a voucher from a given code
        /// </summary>
        /// <param name="code">the code to create a voucher from</param>
        /// <returns>a single voucher</returns>   
        Task<int> CreateValueVoucherAsync(Value value);
        Task<int> CreateValueVoucherAsync(IList<Value> vouchersList);

        #endregion

        #region Read Method 

         /// <summary>
        /// Returns all value vouchers
        /// filtered by the id of the merchant that created the value voucher///
        /// </summary>
        /// <param name="merchantId">id of the merchant that created the voucher</param>
        /// <returns>a list of value vouchers</returns>
        Task<IEnumerable<Value>> GetAllValueVouchersAsync(string merchantId);

        /// <summary>
        /// Returns all details of a value voucher
        /// </summary>
        /// <param name="vouchertype">id of the merchant that created the voucher</param>
        /// <returns>a  value voucher</returns>
        Task<Value> GetValueVoucherAsync(Voucher voucher);

        #endregion
    }

}