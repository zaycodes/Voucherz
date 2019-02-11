
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using VoucherServiceBL.Domain;

namespace VoucherServiceBL.Repository.SqlServer
{
    public class GiftStreamingSqlRecord : IEnumerable<SqlDataRecord>
    {
        private IEnumerable<Gift> _gifts;

        public GiftStreamingSqlRecord(IEnumerable<Gift> gifts) =>  this._gifts = gifts;
        
        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            SqlMetaData[] columnStructure = new SqlMetaData[11];
            columnStructure[0] = new SqlMetaData("VoucherId",SqlDbType.BigInt, 
                        useServerDefault: false,  
                        isUniqueKey: true, 
                        columnSortOrder:SortOrder.Ascending, sortOrdinal: 0);
            columnStructure[1] = new SqlMetaData("Code", SqlDbType.NVarChar, maxLength: 100);
            columnStructure[2] = new SqlMetaData("VoucherType", SqlDbType.NVarChar, maxLength: 50);
            columnStructure[3] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructure[4] = new SqlMetaData("ExpiryDate", SqlDbType.DateTime);
            columnStructure[5] = new SqlMetaData("VoucherStatus", SqlDbType.NVarChar, maxLength: 10);
            columnStructure[6] = new SqlMetaData("MerchantId", SqlDbType.NVarChar, maxLength: 100);
            columnStructure[7] = new SqlMetaData("Metadata", SqlDbType.NVarChar, maxLength: 100);
            columnStructure[8] = new SqlMetaData("Description", SqlDbType.NVarChar, maxLength: 100);
            columnStructure[9] = new SqlMetaData("GiftAmount", SqlDbType.BigInt);
            columnStructure[10] = new SqlMetaData("GiftBalance", SqlDbType.BigInt);

            var columnId = 1L;

            foreach (var gift in _gifts)
            {
                var record =  new SqlDataRecord(columnStructure);
                record.SetInt64(0, columnId++);
                record.SetString(1, gift.Code);
                record.SetString(2, gift.VoucherType);
                record.SetDateTime(3, gift.CreationDate);
                record.SetDateTime(4, gift.ExpiryDate);
                record.SetString(5, gift.VoucherStatus);
                record.SetString(6, gift.MerchantId);
                record.SetString(7, gift.Metadata);
                record.SetString(8, gift.Description);
                record.SetInt64(9, gift.GiftAmount);
                record.SetInt64(10, gift.GiftBalance);
                yield return record;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}