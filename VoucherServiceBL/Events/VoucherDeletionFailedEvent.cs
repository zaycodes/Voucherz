using System;

namespace VoucherServiceBL.Events
{
    public class VoucherDeletionFailedEvent : BaseVoucherEvent
    {
        public string FailureReason { get; set; }
    }
}