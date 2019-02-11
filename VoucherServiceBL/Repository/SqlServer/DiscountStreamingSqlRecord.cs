
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository.SqlServer
{
    public class DiscountStreamingSqlRecord : IEnumerable<SqlDataRecord>
    {
        private IEnumerable<Discount> _discount;

        public DiscountStreamingSqlRecord(IEnumerable<Discount> discount) => this._discount = discount;

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            SqlMetaData[] columnStructure = new SqlMetaData[12];
            columnStructure[0] = new SqlMetaData("VoucherId", SqlDbType.BigInt,
                        useServerDefault: false,
                        isUniqueKey: true,
                        columnSortOrder: SortOrder.Ascending, sortOrdinal: 0);
            columnStructure[1] = new SqlMetaData("Code", SqlDbType.NVarChar, maxLength: 100);
            columnStructure[2] = new SqlMetaData("VoucherType", SqlDbType.NVarChar, maxLength: 50);
            columnStructure[3] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructure[4] = new SqlMetaData("ExpiryDate", SqlDbType.DateTime);
            columnStructure[5] = new SqlMetaData("VoucherStatus", SqlDbType.NVarChar, maxLength: 10);
            columnStructure[6] = new SqlMetaData("MerchantId", SqlDbType.NVarChar, maxLength: 100);
            columnStructure[7] = new SqlMetaData("Metadata", SqlDbType.NVarChar, maxLength: 100);
            columnStructure[8] = new SqlMetaData("Description", SqlDbType.NVarChar, maxLength: 100);
            columnStructure[9] = new SqlMetaData("DiscountAmount", SqlDbType.BigInt);
            columnStructure[10] = new SqlMetaData("DiscountUnit", SqlDbType.BigInt);
            columnStructure[11] = new SqlMetaData("DiscountPercentage", SqlDbType.Float);

            var columnId = 1L;

            foreach (var discount in _discount)
            {
                var record = new SqlDataRecord(columnStructure);
                record.SetInt64(0, columnId++);
                record.SetString(1, discount.Code);
                record.SetString(2, discount.VoucherType);
                record.SetDateTime(3, discount.CreationDate);
                record.SetDateTime(4, discount.ExpiryDate);
                record.SetString(5, discount.VoucherStatus);
                record.SetString(6, discount.MerchantId);
                record.SetString(7, discount.Metadata);
                record.SetString(8, discount.Description);
                record.SetInt64(9, discount.DiscountAmount);
                record.SetInt64(10, discount.DiscountUnit);
                record.SetSqlDouble(11, discount.DiscountPercentage);
                yield return record;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}