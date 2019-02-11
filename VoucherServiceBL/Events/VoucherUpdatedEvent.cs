using System;

namespace VoucherServiceBL.Events
{
    public class VoucherUpdatedEvent : BaseVoucherEvent
    {
        public PropertyUpdated PropertyUpdated { get; set; }
        
    }

    public class PropertyUpdated
    {
        public string PropertyName { get; set; }
        public object PreviousValue { get; set; }
        public object NewValue { get; set; }
    }
}