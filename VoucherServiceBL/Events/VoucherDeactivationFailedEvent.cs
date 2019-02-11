using System;

namespace VoucherServiceBL.Events
{
    public class VoucherDeactivationFailedEvent : BaseVoucherEvent
    {
        public string FailureReason { get; set; }
    }
}