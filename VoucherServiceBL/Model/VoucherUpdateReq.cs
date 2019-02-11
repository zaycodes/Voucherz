using System;
using System.Numerics;

namespace VoucherServiceBL.Model
{
    public class VoucherUpdateReq
    {
        public string Status { get; set; }
        public DateTime ExpiryDate { get; set; }
        public long GiftAmount { get; set; }
        public string Code { get; set; }
    }
}