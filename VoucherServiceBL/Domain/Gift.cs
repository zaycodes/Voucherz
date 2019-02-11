using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace VoucherServiceBL.Domain
{
    [Table("GiftVoucher")]
    public class Gift:Voucher
    {
        [BsonElement("gift_amount")]
        public long GiftAmount { get; set; }

        [BsonElement("gift_balance")]
        public long GiftBalance { get; set; }
        
        [BsonIgnore]
        public long VoucherId { get; set; }
    }
}
