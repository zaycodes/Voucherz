
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository.SqlServer
{
    public class ValueStreamingSqlRecord : IEnumerable<SqlDataRecord>
    {
        private IEnumerable<Value> _value;

        public ValueStreamingSqlRecord(IEnumerable<Value> value) => this._value = value;

        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            SqlMetaData[] columnStructure = new SqlMetaData[10];
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
            columnStructure[9] = new SqlMetaData("ValueAmount", SqlDbType.BigInt);

            var columnId = 1L;

            foreach (var value in _value)
            {
                var record = new SqlDataRecord(columnStructure);
                record.SetInt64(0, columnId++);
                record.SetString(1, value.Code);
                record.SetString(2, value.VoucherType);
                record.SetDateTime(3, value.CreationDate);
                record.SetDateTime(4, value.ExpiryDate);
                record.SetString(5, value.VoucherStatus);
                record.SetString(6, value.MerchantId);
                record.SetString(7, value.Metadata);
                record.SetString(8, value.Description);
                record.SetInt64(9, value.ValueAmount);
                yield return record;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}