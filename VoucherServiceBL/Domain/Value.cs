using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace VoucherServiceBL.Domain
{
    [Table("ValueVoucher")]
    [BsonDiscriminator("Value")]
    public class Value:Voucher
    {
        [BsonElement("value_amount")]
        public long ValueAmount { get; set; }
    }
}
