# VoucherService
Voucher service API for Voucherz Project


API

This is a Microservice fully consistent with restful API specifications. 

Updates to Docs coming soon
Each namespace provides method equivalents for API calls.

Vouchers API

Create Voucher
Get Voucher
Update Voucher
Delete Voucher
List Vouchers: Gift, Discount, Value
Enable/Disable Voucher
Add Gift Voucher Amount
Export Vouchers

Create Voucher
HTTP POST
{host}/api/v1/

Get Voucher
HTTP GET
{host}/api/v1/(merchantId)

Update Voucher
HTTP PATCH
{host}/api/v1/(code)

Update Gift Voucher Amount
HTTP PATCH
{host}/api/v1/(code)

List Vouchers
HTTP GET
{host}/api/v1/

Add Gift Voucher Balance
https://{url}/api/v1/
