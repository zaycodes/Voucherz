
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository
{
    /// <summary>
    /// An interface that interacts with the database concerning a Gift voucher
    /// </summary>
    public interface IGiftRepository
    {
        Task<IEnumerable<Gift>> GetAllGiftVouchersAsync(string merchantId);

        Task<int> CreateGiftVoucherAsync(Gift voucher);

        /// <summary>
        /// Upward review of the amount on a gift voucher
        /// A gift voucher's amount cannot be reduced after
        /// it has been created
        /// </summary>
        /// <param name="amountToAdd">Amount to add to the current balance on the gift voucher</param>
        /// <returns>The gift voucher</returns>
        Task<int?> UpdateGiftVoucherAmountAsync(Gift voucher); 
        Task<Gift> GetGiftVoucherAsync(Voucher voucher);
        Task<int> CreateGiftVoucherAsync(IList<Gift> vouchersList);
        Task<int?> UpdateGiftVoucherBalanceAsync(Gift gift);
    }
}