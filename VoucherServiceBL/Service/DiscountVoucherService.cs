using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherServiceBL.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;

namespace VoucherServiceBL.Service
{
    public class DiscountVoucherService : IDiscountVoucherService
    {
        private IDiscountRepository repository;

        public IDiscountRepository DiscountRepository => this.repository;

        public DiscountVoucherService(IDiscountRepository repository)
        {
            this.repository = repository;
        }


        public Task<int> CreateDiscountVoucher(VoucherRequest discountRequest)
        {
            // var numOfVouchersCreated = 0;

            var vouchersList = new List<Discount>(discountRequest.NumbersOfVoucherToCreate);

            //create the gift object from the Vouher
            foreach (var num in Enumerable.Range(1, discountRequest.NumbersOfVoucherToCreate))


            {
                Discount discountVoucher = new Discount()
                {
                    Code = CodeGenerator.HashedCode(discountRequest),
                    CreationDate = discountRequest.CreationDate,
                    ExpiryDate = discountRequest.ExpiryDate,
                    VoucherStatus = "Active",
                    VoucherType = discountRequest.VoucherType,
                    Description = discountRequest.Description,
                    DiscountAmount = discountRequest.DiscountAmount,
                    DiscountUnit = discountRequest.DiscountUnit,
                    DiscountPercentage = discountRequest.DiscountPercentage,
                    RedemptionCount = 0L,
                    MerchantId = discountRequest.MerchantId,
                    Metadata = discountRequest.Metadata
                };
                vouchersList.Add(discountVoucher);
            }
                //persist the object to the db    
                return DiscountRepository.CreateDiscountVoucherAsync(vouchersList);
        }

        public Task<Discount> GetDiscountVoucher(Voucher voucher)
        {
            return DiscountRepository.GetDiscountVoucherAsync(voucher);
        }

        public Task<IEnumerable<Discount>> GetAllDiscountVouchersFilterByMerchantId(string merchantId)
        {
            return DiscountRepository.GetAllDiscountVouchersFilterByMerchantIdAsync(merchantId);
        }

        public Task UpdateRedemptionCount(Discount discount)
        {
            return DiscountRepository.UpdateRedemptionCount(discount);
        }
    }
}
