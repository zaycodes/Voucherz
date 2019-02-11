using System;


namespace VoucherServiceBL.Exceptions
{
    public class VoucherUpdateException:System.Exception
    {
        public VoucherUpdateException(string message):base(message)
        {
        
        }
    }

    public class VoucherCreateException: System.Exception
    {
        public VoucherCreateException(string message):base(message)
        {
            
        }
    }
}