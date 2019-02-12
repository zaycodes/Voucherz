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
    //[Authorize]
    [Produces("application/json")]
    [Route("api/v1")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private IVoucherService baseVoucherService;

        //private IValueVoucherService valueVoucherService;
        public VoucherController(
                IVoucherService baseService)
        {
            // this.giftVoucher = giftService;
            // this.discountVoucher = discountService;
            this.baseVoucherService = baseService;
        }


        [HttpPost]
        public async Task<ActionResult<object>> CreateVoucher([FromBody] VoucherRequest voucherReq)
        {
            var createdVoucher =  await baseVoucherService.CreateVoucher(voucherReq);
            var voucherType = voucherReq.VoucherType;

            //TODO:provide better status code to client on internal error
            if (createdVoucher == null)  return new StatusCodeResult(500);//(new {Message = "Could not create the vouchers"});
            switch (voucherType.ToUpper())
            {
                case "GIFT": return CreatedAtAction(nameof(GetAllGiftVouchers), 
                new {VoucherCreated = createdVoucher, Message= $"Created {createdVoucher} Vouchers"});

                case "DISCOUNT": return CreatedAtAction(nameof(GetAllDiscountVouchers), 
                new {VoucherCreated = createdVoucher, Message= $"Created {createdVoucher} Vouchers"});

                case "VALUE": return CreatedAtAction(nameof(GetAllValueVouchers), new {value="value/all"},
                new {VoucherCreated = createdVoucher, Message= $"Created {createdVoucher} Vouchers"});

                default: return BadRequest(new {Message = "Invalid Voucher type"});
            }
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<Voucher>> GetVoucher([FromRoute] string code)
        {
            string encryptedCode = CodeGenerator.Encrypt(code);
            var voucher = await baseVoucherService.GetVoucherByCode(encryptedCode);
            string decryptedCode = CodeGenerator.Decrypt(voucher.Code);
            voucher.Code = decryptedCode;
            if (voucher == null) return NotFound(new {Message = "voucher not found"});
            return  Ok(voucher);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllVouchers([FromQuery] string merchantId)
        {
            var vouchers = await baseVoucherService.GetAllVouchers(merchantId);
            if (vouchers.Count() == 0) return NotFound(new {message = $"no voucher found for merchantId: {merchantId}"});

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
            if (gift == null) return  NotFound();
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



        [HttpPatch("update/{code}")]
        public async Task<ActionResult> UpdateVoucherStatus([FromRoute] string code)
        {
            await baseVoucherService.ActivateOrDeactivateVoucher(code);
            return Ok("updated");
        }

        [HttpPatch("expiry/{code}")]
        public async Task<ActionResult> UpdateVoucherExpiryDate([FromRoute] string code, [FromQuery] DateTime newDate)
        {
             var pathedVoucher = await baseVoucherService.UpdateVoucherExpiryDate(code, newDate);
             if (pathedVoucher == null) return new StatusCodeResult(500);

             return new OkObjectResult(pathedVoucher);
        }


        [HttpPatch("amount/{code}")]
        public async Task<ActionResult> UpdateGiftVoucherAmount([FromRoute] string code, [FromQuery] long amount)
        {

            var patchedGift = await baseVoucherService.UpdateGiftVoucherAmount(code,amount);
            return new OkObjectResult(patchedGift);
        }

        [HttpDelete("{code}")]
        public async Task<ActionResult> DeleteVoucher([FromRoute] string code)
        {
            await baseVoucherService.DeleteVoucher(code);
            return Ok("Voucher Deleted");
        }
    }
}        