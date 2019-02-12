using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Model;
using VoucherServiceBL.Service;
using VoucherServiceBL.Util;

namespace VoucherService.Controllers
{
    [Authorize("Admin")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class _adminVoucherController : ControllerBase
    {
        private IVoucherService baseVoucherService;
        private readonly IAdminService _adminService;

        public _adminVoucherController(IVoucherService baseService, IAdminService adminService)
        {
            this.baseVoucherService = baseService;
            this._adminService = adminService;
        }

        #region MerchantId Delimited

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllVouchers([FromQuery] string merchantId)
        {
            var vouchers = await baseVoucherService.GetAllVouchers(merchantId);
            if (vouchers.Count() == 0) return NotFound(new { message = $"no voucher found for merchantId: {merchantId}" });

            return new OkObjectResult(vouchers);
        }

        [HttpGet("discount/{code}")]
        public async Task<ActionResult<Discount>> GetDiscountVoucher([FromRoute] string code)
        {
            var discount = await baseVoucherService.GetDiscountVoucher(code);
            if (discount == null) return NotFound();
            return discount;
        }

        [HttpGet("discount/all")]
        public async Task<ActionResult<IEnumerable<Discount>>> GetAllDiscountVouchers([FromQuery] string merchantId)
        {
            var discounts = await baseVoucherService.GetAllDiscountVouchers(merchantId);
            return new OkObjectResult(discounts);
        }

        [HttpGet("gift/{code}")]
        public async Task<ActionResult<Gift>> GetGiftVoucher([FromRoute] string code)
        {
            var gift = await baseVoucherService.GetGiftVoucher(code);
            if (gift == null) return NotFound();
            return gift;
        }

        [HttpGet("gift/all")]
        public async Task<ActionResult<IEnumerable<Gift>>> GetAllGiftVouchers([FromQuery] string merchantId)
        {
            var gifts = await baseVoucherService.GetAllGiftVouchers(merchantId);
            return new OkObjectResult(gifts);
        }


        [HttpGet("value/{code}")]
        public async Task<ActionResult<Value>> GetValueVoucher([FromRoute] string code)
        {
            var value = await baseVoucherService.GetValueVoucher(code);
            if (value == null) return NotFound();
            return value;
        }

        [HttpGet("value/all")]
        public async Task<ActionResult<IEnumerable<Value>>> GetAllValueVouchers([FromQuery] string merchantId)
        {
            var value = await baseVoucherService.GetAllValueVouchers(merchantId);
            return new OkObjectResult(value);
        }

        [HttpDelete("{code}")]
        public async Task<ActionResult> DeleteVoucher([FromRoute] string code)
        {
            await baseVoucherService.DeleteVoucher(code);
            return Ok("Voucher Deleted");
        }
        #endregion

        #region Non Merchant 
        [HttpGet("{code}")]
        public async Task<ActionResult<Voucher>> GetVoucher([FromRoute] string code)
        {
            string encryptedCode = CodeGenerator.Encrypt(code);
            var voucher = await baseVoucherService.GetVoucherByCode(encryptedCode);
            string decryptedCode = CodeGenerator.Decrypt(voucher.Code);
            voucher.Code = decryptedCode;
            if (voucher == null) return NotFound(new { Message = "voucher not found" });
            return Ok(voucher);
        }
        

        [HttpGet("{vouchers/all}")]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllVouchers()
        {
            var voucherCollections = await _adminService.GetAllVouchers();
            if (voucherCollections.Count == 0 ) return NotFound(new {Message = "No voucher in collection"});
            return Ok(voucherCollections);
        }

        [HttpGet("{vouchers/gift/all}")]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllGiftVouchers()
        {
            var giftCollections = await _adminService.GetAllGiftVouchers();
            if (giftCollections.Count == 0 ) return NotFound(new {Message = "No voucher in collection"});
            return Ok(giftCollections);
        }
        
        [HttpGet("{vouchers/values/all}")]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllValueVouchers()
        {
            var valueCollections = await _adminService.GetAllVouchers();
            if (valueCollections.Count == 0 ) return NotFound(new {Message = "No voucher in collection"});
            return Ok(valueCollections);
        }

        [HttpGet("{vouchers/count}")]
        public async Task<ActionResult<IEnumerable<long>>> TotalVoucherCount()
        {
            var totalList = await _adminService.GetTotalVouchersPerMonth();
            if (totalList.Count == 0) return NotFound(new {Message = "no vouchers to count "});
            return Ok(totalList);

        }

        [HttpGet("{vouchers/gift/count}")]
        public async Task<ActionResult<IEnumerable<long>>> TotalGiftVoucherCount()
        {
            var totalList = await _adminService.GetTotalVouchersPerMonth(VoucherType.GIFT.ToString());
            if (totalList.Count == 0) return NotFound(new {Message = "no vouchers to count "});
            return Ok(totalList);
        }

        [HttpGet("{vouchers/discount/count}")]
        public async Task<ActionResult<IEnumerable<long>>> TotalValueVoucherCount()
        {
            var totalList = await _adminService.GetTotalVouchersPerMonth(VoucherType.VALUE.ToString());
            if (totalList.Count == 0) return NotFound(new {Message = "no vouchers to count "});
            return Ok(totalList);

        }        

        [HttpGet("{vouchers/value/count}")]
        public async Task<ActionResult<IEnumerable<long>>> TotalDiscountVouchersCount()
        {
            var totalList = await _adminService.GetTotalVouchersPerMonth(type:VoucherType.DISCOUNT.ToString());
            if (totalList.Count == 0) return NotFound(new {Message = "no vouchers to count "});
            return Ok(totalList);

        }
        #endregion
    }
}