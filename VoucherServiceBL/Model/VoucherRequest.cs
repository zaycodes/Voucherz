using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace VoucherServiceBL.Model
{
    public class VoucherRequest
    {
        [Required]
        public string VoucherType { get; set; }        
        public long ValueAmount { get; set; }
        public long DiscountAmount { get; set; }
        public int DiscountUnit { get; set; }
        
        [Range(0, 100)]
        public int DiscountPercentage { get; set; }        
        public long GiftAmount { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string CodePattern { get; set; }
        
        public int CodeLength { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression("^Alphanumeric$|^Numeric$|^Alphabet$|alphanumeric$|^numeric$|^alphabet$")]
        public string CharacterSet { get; set; }
        public string Separator { get; set; }
        public DateTime CreationDate { get; set; }
        
        [Required]
        public DateTime ExpiryDate { get; set; }
        public string Description { get; set; }
        public string Metadata;

        [RegularExpression("^[1-9]+[0-9]*$")]        
        public int NumbersOfVoucherToCreate { get; set; }
        
        [Required]
        public string MerchantId { get; set; }
    }
}