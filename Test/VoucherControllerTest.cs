using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoucherService.Controllers;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Service;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }


        [Test]
        public async Task GetVoucher_WhenCalledWithCorrectCode_ShouldReturnCorrectVoucher()
        {

            // Arrange
            var testVoucher = GetVoucher();
            var mockService = new Mock<IVoucherService>();
            mockService.Setup(service => service.GetVoucherByCode("25142dw")).ReturnsAsync(testVoucher);
            var controller = new VoucherController(mockService.Object);

            // Act
            ActionResult<Voucher> result = await controller.GetVoucher("25142dw");

            // Assert
            Assert.IsNotNull(result);
        }


        //GetVoucher_WhenCalledWithWrongCode_ShouldNotReturnNull() test confirms that the controller 
        //returns an ActionResult but not a nonexistent list of ideas for a nonexistent voucher:
        
        //The Result is a NotFoundResult.

        [Test]
        public async Task GetVoucher_ReturnsNotFoundResultForNonexistentVoucher()
        {

            // Arrange
            var mockRepo = new Mock<IVoucherService>();
            var controller = new VoucherController(mockRepo.Object);
            var nonExistentVoucher = "sr4579ndsh";

            // Act
            ActionResult<Voucher> actionResult = await controller.GetVoucher(nonExistentVoucher);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(actionResult.Result);
        }


        ////DeleteVoucher_ShouldChangeVoucherStatusToDeleted() checks the return type, in this case OkObjectResult,
        //// of the DeleteVoucher() Controller method
        //[Test]
        //public async Task DeleteVoucher_ShouldChangeVoucherStatusToDeleted()
        //{
        //    // Arrange
        //    var testVoucher = GetVoucher();
        //    var mockService = new Mock<IVoucherService>();
        //    mockService.Setup(service => service.DeleteVoucher("25142dw"));
        //    var controller = new VoucherController(mockService.Object);

        //    // Act
        //    Task<ActionResult> actionResult = controller.DeleteVoucher("25142dw");

        //    // Assert
        //    Assert.IsNotNull(actionResult);
        //    Assert.IsInstanceOf<OkObjectResult>(actionResult);

        //}
        private IEnumerable<Value> GetVouchers()
        {
            var TestValueVouchers = new List<Value>();
            TestValueVouchers.Add(new Value { Code = "253hjD142dw", Description = null, CreationDate = new DateTime(2019, 09, 15), ExpiryDate = new DateTime(2019, 12, 25), MerchantId = "2222", ValueAmount = 2000, VoucherStatus = "Active", VoucherType = "Value", Metadata = null });
            TestValueVouchers.Add(new Value { Code = "zrd352j", Description = null, CreationDate = new DateTime(2019, 01, 20), ExpiryDate = new DateTime(2019, 01, 29), MerchantId = "2222", ValueAmount = 1000, VoucherStatus = "Active", VoucherType = "Value", Metadata = null });
            TestValueVouchers.Add(new Value { Code = "36632fjms", Description = null, CreationDate = new DateTime(2019, 10, 28), ExpiryDate = new DateTime(2019, 10, 30), MerchantId = "2223", ValueAmount = 500, VoucherStatus = "Active", VoucherType = "Value", Metadata = null });
            TestValueVouchers.Add(new Value { Code = "ytw5353ss", Description = null, CreationDate = new DateTime(2019, 12, 12), ExpiryDate = new DateTime(2019, 12, 14), MerchantId = "2224", ValueAmount = 1500, VoucherStatus = "Active", VoucherType = "Value", Metadata = null });
            return TestValueVouchers;
        }

        private Voucher GetVoucher()
        {
            var voucher = new Voucher { Id = 0, Code = "25142dw", Description = null, CreationDate = new DateTime(2019, 09, 15), ExpiryDate = new DateTime(2019, 12, 25), MerchantId = "2222", VoucherStatus = "Active", VoucherType = "Value", Metadata = null };
            return voucher;
        }

        private Voucher GetVoucherByCode(string code)
        {
            var vouchers = GetVouchers();
            var voucher = from v in vouchers where v.Code == code select v;
            return voucher.FirstOrDefault<Value>();
        }

        //private IEnumerable<Value> GetVouchersByMerchantId(String merchantId)
        //{
        //    var values = GetVouchers();
        //    var value = from v in values where v.MerchantId == merchantId select v;
        //    return value.AsEnumerable<Value>();
        //}

    }
}