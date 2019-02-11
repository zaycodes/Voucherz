using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoucherServiceBL.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;
using MongoDB.Driver;
using Serilog;
using Microsoft.Extensions.Logging;

namespace VoucherServiceBL.Service
{
    public class ValueVoucherService : IValueVoucherService
    {
        private IValueRepository repository;

        public IValueRepository ValueRepository => this.repository;
        public ILogger<ValueVoucherService> _logger;

        public ValueVoucherService(IValueRepository repository, ILogger<ValueVoucherService> logger)
        {
            this.repository = repository;
            this._logger = logger;
        }

        public Task<int> CreateValueVoucher(VoucherRequest valueRequest)
        {
            // var numOfVouchersCreated = 0;

            var vouchersList = new List<Value>(valueRequest.NumbersOfVoucherToCreate);

            //create the gift object from the Vouher
            foreach (var num in Enumerable.Range(1, valueRequest.NumbersOfVoucherToCreate))

            {
                Value valueVoucher = new Value()
                {
                    Code = CodeGenerator.HashedCode(valueRequest),
                    CreationDate = valueRequest.CreationDate,
                    ExpiryDate = valueRequest.ExpiryDate,
                    VoucherStatus = "Active",
                    VoucherType = valueRequest.VoucherType,
                    Description = valueRequest.Description,
                    ValueAmount = valueRequest.ValueAmount,
                    MerchantId = valueRequest.MerchantId,
                    Metadata = valueRequest.Metadata,
                };
                vouchersList.Add(valueVoucher);
            }
            
            return ValueRepository.CreateValueVoucherAsync(vouchersList);
        }

        public Task<IEnumerable<Value>> GetAllValueVouchers(string merchantId)
        {
            return ValueRepository.GetAllValueVouchersAsync(merchantId);
        }

        public Task<Value> GetValueVoucher(Voucher voucher)
        {
            return ValueRepository.GetValueVoucherAsync(voucher);
        }

    }
}
