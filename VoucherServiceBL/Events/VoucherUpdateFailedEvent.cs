using System;

namespace VoucherServiceBL.Events
{
    public class VoucherUpdateFailedEvent : BaseVoucherEvent
    {
        public PropertyUpdated PropertyToUpdate { get; set; }
        public string FailureReason { get; set; }
        
    }
}