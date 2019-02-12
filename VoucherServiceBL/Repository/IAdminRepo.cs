using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository
{
    public interface IAdminRepo
    {
        Task<IList<Discount>> GetAllDiscountVouchers();
        Task<IList<Gift>> GetAllGiftVouchers();
        Task<IList<Value>> GetAllValueVouchers();
        Task<IList<Voucher>> GetAllVouchers();
        Task<long> GetTotalVoucherPerMonth(string type = "VOUCHER", 
                                            int month = 1, int? year = null);
    }
}