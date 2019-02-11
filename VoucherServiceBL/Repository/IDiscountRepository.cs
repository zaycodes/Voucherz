using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository
{
    public interface IDiscountRepository
    {
        Task<int> CreateDiscountVoucherAsync(Discount discount);
        Task<Discount> GetDiscountVoucherAsync(Voucher voucher);
        Task<IEnumerable<Discount>> GetAllDiscountVouchersFilterByMerchantIdAsync(string merchantId);
        Task<int> CreateDiscountVoucherAsync(IList<Discount> vouchersList);
        Task<int> UpdateRedemptionCount(Discount discount);

    }

}
