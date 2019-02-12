using System.Collections.Generic;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Service
{
    public interface IAdminService
    {
        Task<IList<Voucher>> GetAllVouchers(); //retrieves all vouchers in the db
        Task<IList<Gift>> GetAllGiftVouchers(); //retrieves all gift vouchers in the db
        Task<IList<Discount>> GetAllDiscountVouchers(); //retrieves all discount vouchers in the db
        Task<IList<Value>> GetAllValueVouchers(); //retrieves all value vouchers in the db

        //retrieves a list containing the sum of all vouchers of type 'type' 
        //created each month of the year
        Task<IList<long>> GetTotalVouchersPerMonth(string type = null); 


    }
}