using System;

namespace VoucherServiceBL.Events
{
    public abstract class BaseVoucherEvent
    {
        public string VoucherCode { get; set; }
        public Guid EventId { get; set; }
        public string Message { get; set; }
        public DateTime EventTime { get; set; }
        public string MerchantId { get; set; }
        public string VoucherType { get; set; }        
    }
}