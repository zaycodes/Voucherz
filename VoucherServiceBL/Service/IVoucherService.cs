using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;

namespace VoucherServiceBL.Service
{
    public interface IVoucherService
    {
        Task<int?> CreateVoucher(VoucherRequest voucherRequest);
        Task<Voucher> GetVoucherByCode(string code);
        Task<IEnumerable<Voucher>> GetAllVouchers(string merchantId); 
        Task DeleteVoucher(string code);
        Task<long?> ActivateOrDeactivateVoucher(string code);
        Task<Voucher> UpdateGiftVoucherAmount(string code, long amount);
        Task<Voucher> UpdateGiftVoucherBalance(string code, long amount);
        Task UpdateRedemptionCount(string code);
        Task<long?> UpdateVoucherExpiryDate(string code, DateTime newDate);

        Task<IEnumerable<Gift>> GetAllGiftVouchers(string merchantId);
        Task<Gift> GetGiftVoucher(string code);

        Task<Value> GetValueVoucher(string code);
        Task<IEnumerable<Value>> GetAllValueVouchers(string merchantId);

        Task<IEnumerable<Discount>> GetAllDiscountVouchers(string merchantId);
        Task<Discount> GetDiscountVoucher(string code);       
    }
}