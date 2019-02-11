using System.Collections.Generic;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<Voucher>> GetAllVouchersFilterByMerchantIdAsync(string merchantId);

        Task<Voucher> GetVoucherByCodeAsync(string code);

         Task<Voucher> GetVoucherByCodeFilterByMerchantIdAsync(Voucher voucher);

        Task<Voucher> GetVoucherByCreationDateAsync(Voucher voucher);

        Task<Voucher> GetVoucherByCreationDateFilterByMerchantIdAsync(Voucher voucher);

        Task<Voucher> GetVoucherByExpiryDateAsync(Voucher voucher);

        Task<Voucher> GetVoucherByExpiryDateFilterByMerchantIdAsync(Voucher voucher);

        Task<Voucher> GetVoucherByMerchantIdAsync(Voucher voucher);

        Task<Voucher> GetVoucherByStatusAsync(Voucher voucher);

        Task<long> UpdateVoucherExpiryDateByCodeAsync(Voucher voucher);

        Task<long> UpdateVoucherStatusByCodeAsync(Voucher voucher);

        Task DeleteVoucherByCodeAsync(string code);
        
    }
}
