using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class VoucherControllerTest
    {
        [TestMethod]
        public void CreateVoucher([FromBody]VoucherRequest voucherReq)
        {
            VoucherController controller = new VoucherController();
            // Act
            ViewResult result = controller.CreateVoucher(voucherReq) as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetVoucher([FromRoute] string code)
        {
            VoucherController controller = new VoucherController();
            // Act
            ViewResult result = controller.GetVoucher(code) as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }



    }