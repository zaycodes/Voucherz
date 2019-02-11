using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace VoucherServiceBL.Domain
{
    [Table("DiscountVoucher")]
    public class Discount:Voucher,ISingleVoucher
    {
        [BsonElement("discount_amount")]
        public long DiscountAmount { get; set; }

        [BsonElement("discount_unit")]
        public long DiscountUnit { get; set; }

        [BsonElement("discount_percentage")]
        public float DiscountPercentage { get; set; }

        [BsonElement("redemption_count")]
        public long RedemptionCount { get; set; }
    }
}
