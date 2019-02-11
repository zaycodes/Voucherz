using System;

namespace VoucherServiceBL.Events
{
    public class VoucherReactivatonFailedEvent : BaseVoucherEvent
    {
        public string FailureReason { get; set; }
    }
}