using System.Collections.Generic;
using System.Threading.Tasks;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;

namespace VoucherServiceBL.Service
{
    /// <summary>
    /// A interface that handles the management of a value voucher
    /// </summary>


    public interface IValueVoucherService
    {
        IValueRepository ValueRepository { get; }
        Task<int> CreateValueVoucher(VoucherRequest value);

        Task<IEnumerable<Value>> GetAllValueVouchers(string merchantId);

        Task<Value> GetValueVoucher(Voucher voucher);
    }
}