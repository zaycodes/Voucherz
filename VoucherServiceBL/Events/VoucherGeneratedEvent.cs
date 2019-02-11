using System;

namespace VoucherServiceBL.Events
{
    public class VoucherGeneratedEvent : BaseVoucherEvent
    {
        public long NumberGenerated { get; set; }
    }
}