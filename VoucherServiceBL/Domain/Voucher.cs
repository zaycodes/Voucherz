using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;
using VoucherServiceBL.Util;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VoucherServiceBL.Domain
{
    [Table("Voucher")]
    public class Voucher
    {
        //    [BsonId]
        [BsonIgnoreIfDefault]
        [JsonIgnore]
        public ObjectId _id { get; set; }

        [BsonIgnore]
        public long Id { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("voucher_type")]
        public string VoucherType { get; set; }
       
        //[JsonConverter(typeof(MicrosecondEpochConverter))]

        [BsonElement("expiry_date")]
        public DateTime ExpiryDate { get; set; }
        
        //[JsonConverter(typeof(MicrosecondEpochConverter))]

        [BsonElement("creation_date")]
        public DateTime CreationDate { get; set; }

        [BsonElement("merchant_id")]
        public string MerchantId { get; set; }

        [BsonElement("voucher_status")]
        public string VoucherStatus { get; set; }

        [BsonElement("Metadata")]
        public string Metadata { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

    }
}
