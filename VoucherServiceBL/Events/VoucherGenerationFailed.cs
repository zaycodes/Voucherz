using System;

namespace VoucherServiceBL.Events
{
    public class VoucherGenerationFailedEvent : BaseVoucherEvent
    {
        public string MerchantId { get; set; }
        public string VoucherType { get; set; }
        public long NumberToGenerate { get; set; }
        public string FailureReason { get; set; }
    }
}