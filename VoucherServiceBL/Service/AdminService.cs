using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Repository;
using VoucherServiceBL.Util;

namespace VoucherServiceBL.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepo _adminRepo;

        public AdminService(IAdminRepo adminRepo)
        {
            this._adminRepo = adminRepo;
        }
        public Task<IList<Discount>> GetAllDiscountVouchers()
        {
            return _adminRepo.GetAllDiscountVouchers();
        }

        public Task<IList<Gift>> GetAllGiftVouchers()
        {
            return _adminRepo.GetAllGiftVouchers();
        }

        public Task<IList<Value>> GetAllValueVouchers()
        {
            return _adminRepo.GetAllValueVouchers();
        }

        public Task<IList<Voucher>> GetAllVouchers()
        {
            return _adminRepo.GetAllVouchers();
        }

        public async Task<IList<long>> GetTotalVouchersPerMonth(string type = "VOUCHER")
        {
            var totals = new List<long>(12);
            switch (type.ToUpper())
            {
                case "GIFT": {
                    for(int num = 1; num <= 12; num++)
                    {
                        var monthCount = await _adminRepo.GetTotalVoucherPerMonth(VoucherType.GIFT.ToString(), num);
                        totals.Add(monthCount);
                    }
                    return totals;
                }
                case "DISCOUNT": {
                    for(int num = 1; num <= 12; num++)
                    {
                        var monthCount = await _adminRepo.GetTotalVoucherPerMonth(VoucherType.DISCOUNT.ToString(), num);
                        totals.Add(monthCount);
                    }
                    return totals;
                }
                case "VALUE": {
                    for(int num = 1; num <= 12; num++)
                    {
                        var monthCount = await _adminRepo.GetTotalVoucherPerMonth(VoucherType.VALUE.ToString(), num);
                        totals.Add(monthCount);
                    }
                    return totals;
                }
                case "VOUCHER": {
                    for(int num = 1; num <= 12; num++)
                    {
                        var monthCount = await _adminRepo.GetTotalVoucherPerMonth(month:num);
                        totals.Add(monthCount);
                    }
                    return totals;                
                }
                
                default: return null;
            }
        }
    }
}