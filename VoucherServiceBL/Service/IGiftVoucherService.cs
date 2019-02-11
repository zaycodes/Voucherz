
using System.Collections.Generic;
using VoucherServiceBL.Repository;
using VoucherServiceBL.Domain;
using System.Numerics;
using VoucherServiceBL.Model;
using System.Threading.Tasks;

namespace VoucherServiceBL.Service
{
    /// <summary>
    /// A interface that handles the management of a gift voucher
    /// </summary>
    public interface IGiftVoucherService
    {
        IGiftRepository GiftRepository {get;}
        Task<int> CreateGiftVoucher(VoucherRequest giftRequest);

        Task<Gift> GetGiftVoucher(Voucher voucher);

        Task<IEnumerable<Gift>> GetAllGiftVouchers(string merchantId);

        Task<int?> UpdateGiftVoucher(Gift giftVoucher);
        
        Task<int?> UpdateGiftVoucherBalance(Gift giftVoucher);
    }
}