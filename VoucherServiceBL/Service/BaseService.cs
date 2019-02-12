using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VoucherServiceBL.Util;
using VoucherServiceBL.Domain;
using VoucherServiceBL.Exceptions;
using VoucherServiceBL.Model;
using VoucherServiceBL.Repository;
using VoucherServiceBL.Repository.SqlServer;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using VoucherServiceBL.Events;
using Serilog;
using Hangfire;
using System.Reflection;

namespace VoucherServiceBL.Service
{
    public class BaseService : IVoucherService
    {

        private IGiftVoucherService _giftVoucherService;
        private IDiscountVoucherService _discountVoucherService;
        private IValueVoucherService _valueVoucherService;
        private IVoucherRepository _baseRepository;
        private ILogger<BaseService> _logger;

        //inject the services
        public BaseService(IGiftVoucherService giftService, IDiscountVoucherService discountService,
                           IValueVoucherService valueService, IVoucherRepository voucherRepository, ILogger<BaseService> logger)
        {
            this._giftVoucherService = giftService;
            this._discountVoucherService = discountService;
            this._valueVoucherService = valueService;
            this._baseRepository = voucherRepository;
            this._logger = logger;
        }
        

        public async Task<int?> CreateVoucher(VoucherRequest voucherRequest)
        {
            var numOfVouchersCreated = 0;

            //let each voucher service handle its own creation
            try
            {
                voucherRequest.CreationDate = DateTime.Now;
                voucherRequest.Metadata = Guid.NewGuid().ToString();


                if (voucherRequest.VoucherType.ToUpper() == "GIFT")
                {
                    numOfVouchersCreated += await _giftVoucherService.CreateGiftVoucher(voucherRequest);
                }

                else if (voucherRequest.VoucherType.ToUpper() == "DISCOUNT")
                {
                    numOfVouchersCreated += await _discountVoucherService.CreateDiscountVoucher(voucherRequest);
                }

                else
                {
                    numOfVouchersCreated += await _valueVoucherService.CreateValueVoucher(voucherRequest);

                }

                //TODO: Log the event (VoucherCreated) 
                var voucherGeneratedEvent = new VoucherGeneratedEvent()
                {
                    EventId = Guid.NewGuid(),
                    EventTime = DateTime.Now,
                    MerchantId = voucherRequest.MerchantId,
                    NumberGenerated = numOfVouchersCreated,
                    Message = "New Vouchers created"
                };

                _logger.LogInformation("Created {Number}: vouchers for {Merchant} :{@Event}",
                        numOfVouchersCreated, voucherRequest.MerchantId, voucherGeneratedEvent);

                return numOfVouchersCreated;
            }

            catch (VoucherCreateException ex) //something happened handle it
                                              //if some error occurred and not all voucher could be created log the error
            {
                //Log the error
                var creationError = new VoucherGenerationFailedEvent()
                {
                    EventId = Guid.NewGuid(),
                    EventTime = DateTime.Now,
                    MerchantId = voucherRequest.MerchantId,
                    FailureReason = ex.Message,
                    Message = "Failed to generate voucher",
                    VoucherType = voucherRequest.VoucherType,
                    NumberToGenerate = voucherRequest.NumbersOfVoucherToCreate
                };
                _logger.LogError("Could not generate voucher: {@creationError}", creationError);
                _logger.LogDebug(ex, "An error occured while creating vouchers for {Merchant}", voucherRequest.MerchantId);
                return null;
            }
        }


        public async Task<Voucher> GetVoucherByCode(string code)
        {
            
            Voucher voucher = await _baseRepository.GetVoucherByCodeAsync(code);
            try
            {

                if (voucher != null)
                {

                    return voucher;
                }
            }
            catch (TargetException ex)
            {
                return null;
            }
          
            return null;
        }

        /// <summary>
        /// Returns all vouchers created by a merchant regardless of the their type
        /// </summary>
        /// <param name="merchantId">the id of the merchant that created the vouchers</param>
        /// <returns>a list of vouchers</returns>
        public async Task<IEnumerable<Voucher>> GetAllVouchers(string merchantId)
        {
            var vouchers = await _baseRepository.GetAllVouchersFilterByMerchantIdAsync(merchantId);
            foreach (var voucher in vouchers)
            {
                string decryptedCode = CodeGenerator.Decrypt(voucher.Code);
                voucher.Code = decryptedCode;
            }
            return vouchers;
        }

        public Task DeleteVoucher(string code)
        {
            try
            {
                string encryptedCode = CodeGenerator.Encrypt(code);
                var deleteTask =_baseRepository.DeleteVoucherByCodeAsync(encryptedCode);
                var deleteEvent = new VoucherDeletedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now,
                        Message = "Deleted voucher", VoucherCode = code                    
                };

                _logger.LogInformation("Deleted Voucher: {@DeleteEvent}", deleteEvent);
                return deleteTask;                
            }

            catch (Exception ex) 
            {
                var deleteFailedEvent = new VoucherDeletionFailedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now,
                        Message = "Could not perform delete on voucher", VoucherCode = code,
                        FailureReason = ex.Message 
                };

                _logger.LogError("Deletion Failed on voucher: {@DeleteFailedEvent}", deleteFailedEvent);
                
                _logger.LogError(ex, "Could not perform delete operation on voucher with {Code}", code);
                return null;
            }
        }

        public async Task<long?> ActivateOrDeactivateVoucher(string code)
        {
            try
            {
                //get the voucher that is to be updated
                string encryptedCode = CodeGenerator.Encrypt(code);
                var voucher = await GetVoucherByCode(encryptedCode);
                voucher.VoucherStatus = voucher.VoucherStatus== "ACTIVE" ? "INACTIVE" : "ACTIVE";


                long recordsAffected = await _baseRepository.UpdateVoucherStatusByCodeAsync(voucher);

                //log the event
                if (voucher.VoucherStatus == "ACTIVE")
                {
                    var updatedEvent = new VoucherDeactivatedEvent() {
                            EventId = Guid.NewGuid(), EventTime = DateTime.Now, MerchantId = voucher.MerchantId,
                            Message = "Voucher was deactivated", VoucherCode = voucher.Code, 
                            VoucherType = voucher.VoucherType
                    };
                    _logger.LogInformation("Deactivated a voucher: {@Event}", updatedEvent);    
                }

                if (voucher.VoucherStatus == "INACTIVE")
                {
                    var updatedEvent = new VoucherReactivatedEvent() {
                            EventId = Guid.NewGuid(), EventTime = DateTime.Now, MerchantId = voucher.MerchantId,
                            Message = "Voucher was Activated", VoucherCode = voucher.Code, 
                            VoucherType = voucher.VoucherType
                    };
                    _logger.LogInformation("Activated a voucher: {@Event}", updatedEvent);    
                };

                return recordsAffected;
            }

            catch (Exception ex)
            {
                //log the event
                
                var updatedFailedEvent = new VoucherUpdateFailedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now,
                        Message = "Could not perform update on voucher", VoucherCode = code, 
                        FailureReason = ex.Message 
                };

                _logger.LogInformation("Updated a voucher: {@Event}", updatedFailedEvent);    

                
                _logger.LogError(ex, "Could not perform activate or deactivate operation on voucher with {Code}", code);
            }
                return null;
        }

        public async Task<Voucher> UpdateGiftVoucherAmount(string code, long amount)
        {
            try
            {

                string encryptedCode = CodeGenerator.Encrypt(code);
                var voucher = await GetVoucherByCode(encryptedCode);
                Gift giftVoucher = await _giftVoucherService.GetGiftVoucher(voucher); //returning a gift voucher
                
                var previousAmount = giftVoucher.GiftAmount;

                giftVoucher.GiftAmount += amount;
                giftVoucher.GiftBalance += amount;
    

                //log the event
                var updatedEvent = new VoucherUpdatedEvent()
                {
                    EventId = Guid.NewGuid(),
                    EventTime = DateTime.Now,
                    MerchantId = voucher.MerchantId,
                    Message = "Update performed on voucher",
                    VoucherCode = voucher.Code,
                    VoucherType = voucher.VoucherType,
                    PropertyUpdated = new PropertyUpdated()
                    {
                        PropertyName = "GiftAmount",
                        PreviousValue = previousAmount,
                        NewValue = giftVoucher.GiftAmount
                    }
                };

                _logger.LogInformation("Updated a voucher: {@UpdateEvent}", updatedEvent);
                await _giftVoucherService.UpdateGiftVoucher(giftVoucher); //persist the change 
                string decryptedCode = CodeGenerator.Decrypt(voucher.Code);
                voucher.Code = decryptedCode;

                return voucher;
            }

            catch (VoucherUpdateException ex)
            {
                var updatedFailedEvent = new VoucherUpdateFailedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now, VoucherType = VoucherType.GIFT.ToString(),
                        Message = "Update operation failed for voucher", VoucherCode = code, FailureReason = ex.Message,
                        PropertyToUpdate = new PropertyUpdated() {PropertyName = "GiftBalance", NewValue = amount}
                };

                _logger.LogError("Error Updating a gift: {@UpdateFailedEvent}", updatedFailedEvent);

                _logger.LogDebug(ex, "Could not perform update operation on voucher with {Code}", code);
                return null;
            }
        }

        public async Task<Voucher> UpdateGiftVoucherBalance(string code, long amount)
        {
            try
            {
            string encryptedCode = CodeGenerator.Encrypt(code);
            var voucher = await GetVoucherByCode(encryptedCode);
            Gift giftVoucher = await _giftVoucherService.GetGiftVoucher(voucher); //returning a gift voucher
            giftVoucher.GiftBalance = amount; // do the update
            await _giftVoucherService.UpdateGiftVoucherBalance(giftVoucher); //persist the change   
                                                                                 

                return voucher;
            }
        
            catch (VoucherUpdateException ex)
            {
                _logger.LogError(ex, "Could not perform update operation on voucher with {Code}", code);
                return null;
            }
        }

        public async Task<long?> UpdateVoucherExpiryDate(string code, DateTime newDate)
        {
            try
            {
                //get the voucher that is to be updated
                string encryptedCode = CodeGenerator.Encrypt(code);
                var voucher = await GetVoucherByCode(encryptedCode);
                var oldDate = voucher.ExpiryDate;
                voucher.ExpiryDate = newDate;
                var recordsAffected = await _baseRepository.UpdateVoucherExpiryDateByCodeAsync(voucher);
                
                var updatedEvent = new VoucherUpdatedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now, MerchantId = voucher.MerchantId,
                        Message = "Update performed on voucher", VoucherCode = voucher.Code, 
                        VoucherType = voucher.VoucherType, PropertyUpdated = new PropertyUpdated() {
                            PropertyName = "ExpiryDate", PreviousValue = oldDate, NewValue = voucher.ExpiryDate
                        }
                };

                _logger.LogInformation("Voucher expiry date updated: {@ExpiryUpdateEvent}", updatedEvent);
                return recordsAffected;
            }
            catch (VoucherUpdateException ex)
            {
                var updateFailedEvent = new VoucherUpdateFailedEvent() {
                        EventId = Guid.NewGuid(), EventTime = DateTime.Now,
                        Message = "Update performed on voucher", VoucherCode = code, 
                        FailureReason = ex.Message, PropertyToUpdate = new PropertyUpdated() {
                            PropertyName = "ExpiryDate", NewValue = newDate
                        }
                };
                _logger.LogError("Failed to update Voucher: {@ExpiryUpdateFailedEvent}", updateFailedEvent);
                _logger.LogDebug(ex, "Could not perform update operation on voucher with {Code}", code);
                return null;
            }
        }

        public async Task<IEnumerable<Gift>> GetAllGiftVouchers(string merchantId)
        {
            try
            {
                var vouchers = await _giftVoucherService.GetAllGiftVouchers(merchantId);
                foreach (var voucher in vouchers)
                {
                    string decryptedCode = CodeGenerator.Decrypt(voucher.Code);
                    voucher.Code = decryptedCode;
                }
                return vouchers;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Could not perform retrieve operation on vouchers of {Merchant}", merchantId);
                return null;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Could not perform retrieve operation on vouchers of {Merchant}", merchantId);
                return null;
            }
        }

        public async Task<Gift> GetGiftVoucher(string code)
        {
            try
            { 
                    string encryptedCode = CodeGenerator.Encrypt(code);
                    var voucher = await GetVoucherByCode(encryptedCode);
                    Gift voucherResponse = await _giftVoucherService.GetGiftVoucher(voucher);
                    string decryptedCode = CodeGenerator.Decrypt(voucherResponse.Code);
                    voucherResponse.Code = decryptedCode;
                    return voucherResponse;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Invalid Voucher");
                return null;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Invalid Voucher");
                return null;
            }
        }

        public async Task<Value> GetValueVoucher(string code)
        {
    
            string encryptedCode = CodeGenerator.Encrypt(code);
            var voucher = await GetVoucherByCode(encryptedCode);
            Value voucherResponse = await _valueVoucherService.GetValueVoucher(voucher);
            string decryptedCode = CodeGenerator.Decrypt(voucherResponse.Code);
            voucherResponse.Code = decryptedCode;
            return voucherResponse;
        }

        public async Task<IEnumerable<Value>> GetAllValueVouchers(string merchantId)
        {
       
            var vouchers = await _valueVoucherService.GetAllValueVouchers(merchantId);
            foreach (var voucher in vouchers)
            {
                string decryptedCode = CodeGenerator.Decrypt(voucher.Code);
                voucher.Code = decryptedCode;
            }
            return vouchers;
        }


        public async Task<IEnumerable<Discount>> GetAllDiscountVouchers(string merchantId)
        {
            
            var vouchers = await _discountVoucherService.GetAllDiscountVouchersFilterByMerchantId(merchantId);
            foreach (var voucher in vouchers)
            {
                string decryptedCode = CodeGenerator.Decrypt(voucher.Code);
                voucher.Code = decryptedCode;
            }
            return vouchers;
        }

        public async Task<Discount> GetDiscountVoucher(string code)
        {
            
            string encryptedCode = CodeGenerator.Encrypt(code);
            var voucher = await GetVoucherByCode(encryptedCode);
            Discount voucherResponse = await _discountVoucherService.GetDiscountVoucher(voucher);
            string decryptedCode = CodeGenerator.Decrypt(voucherResponse.Code);
            voucherResponse.Code = decryptedCode;
            return voucherResponse;
        }

        public async Task UpdateRedemptionCount(string code)
        {
            
            var discount = await GetDiscountVoucher(code);
            await _discountVoucherService.UpdateRedemptionCount(discount);
        }
    }


}
