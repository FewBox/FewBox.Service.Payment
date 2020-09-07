using System;
using System.Collections.Specialized;

namespace FewBox.Service.Payment.Model.Service
{
    public class PaypalContext
    {
        private NameValueCollection QureyParams { get; set; }
        public PaymentInformation PaymentInformation { get; set; }
        public BuyerInformation BuyerInformation { get; set; }
        public BasicInformation BasicInformation { get; set; }
        public CurrencyAndCurrrencyExchange CurrencyAndCurrrencyExchange { get; set; }
        public TransactionFields TransactionFields { get; set; }
        public EBayAuction EBayAuction { get; set; }
        public RefundsOrReversals RefundsOrReversals { get; set; }
        public AdvancedAndCustomInformation AdvancedAndCustomInformation { get; set; }

        public PaypalContext(NameValueCollection qureyParams)
        {
            this.QureyParams = qureyParams;
            this.PaymentInformation = new PaymentInformation
            {
                PaymentType = this.ConvertToEnum<PaymentType>(this.QureyParams["payment_type"]),
                PaymentDate = this.ConvertToDate(this.QureyParams["payment_date"]),
                PaymentStatusType = this.ConvertToEnum<PaymentStatusType>(this.QureyParams["payment_status"]),
                PendingReason = this.QureyParams["pending_reason"]
            };
            this.BuyerInformation = new BuyerInformation
            {
                AddressStatusType = this.ConvertToEnum<AddressStatusType>(this.QureyParams["address_status"]),
                PayerStatusType = this.ConvertToEnum<PayerStatusType>(this.QureyParams["payer_status"]),
                FirstName = this.QureyParams["first_name"],
                LastName = this.QureyParams["last_name"],
                PayerEmail = this.QureyParams["payer_email"],
                PayerId = this.QureyParams["payer_id"],
                AddressName = this.QureyParams["address_name"],
                AddressCountry = this.QureyParams["address_country"],
                AddressZip = this.QureyParams["address_zip"],
                AddressState = this.QureyParams["address_state"],
                AddressCity = this.QureyParams["address_city"],
                AddressStreet = this.QureyParams["address_city"]
            };
            this.BasicInformation = new BasicInformation
            {
                Business = this.QureyParams["address_city"],
                ReceiverEmail = this.QureyParams["receiver_email"],
                ReceiverId = this.QureyParams["receiver_id"],
                ResidenceCountry = this.QureyParams["residence_country"],
                ItemName = this.QureyParams["item_name"],
                ItemName1 = this.QureyParams["item_name1"],
                ItemNumber = this.QureyParams["item_number"],
                ItemNumber1 = this.QureyParams["item_number1"],
                Quantity = this.ConvertToInteger(this.QureyParams["quantity"]),
                Shipping = this.QureyParams["shipping"],
                Tax = this.QureyParams["tax"]
            };
            this.CurrencyAndCurrrencyExchange = new CurrencyAndCurrrencyExchange
            {
                MCCurrency = this.QureyParams["mc_currency"],
                MCFee = this.ConvertToDecimal(this.QureyParams["mc_fee"]),
                MCGross = this.ConvertToDecimal(this.QureyParams["mc_gross"]),
                MCGross1 = this.ConvertToDecimal(this.QureyParams["mc_gross_1"]),
                MCHandling = this.QureyParams["mc_handling"],
                MCHandling1 = this.QureyParams["mc_handling1"],
                MCShipping = this.QureyParams["mc_shipping"],
                MCShipping1 = this.QureyParams["mc_shipping1"]
            };
            this.TransactionFields = new TransactionFields
            {
                TxnType = this.ConvertToEnum<TxnType>(this.QureyParams["txn_type"]),
                TxnId = this.QureyParams["txn_id"],
                NotifyVersion = this.QureyParams["notify_version"],
                ParentTxnId = this.QureyParams["parent_txn_id"],
                ReasonCode = this.QureyParams["reason_code"],
                ReceiptID = this.QureyParams["receipt_ID"]
            };
            this.EBayAuction = new EBayAuction
            {
                AuctionBuyerId = this.QureyParams["auction_buyer_id"],
                AuctionClosingDate = this.ConvertToDate(this.QureyParams["auction_closing_date"]),
                ForAuction = this.QureyParams["for_auction"]
            };
            this.RefundsOrReversals = new RefundsOrReversals
            {
                ReasonCode = this.QureyParams["reason_code"],
                ReceiptId = this.QureyParams["receipt_id"]
            };
            this.AdvancedAndCustomInformation = new AdvancedAndCustomInformation
            {
                Custom = this.QureyParams["custom"],
                Invoice = this.QureyParams["invoice"]
            };
        }

        private int ConvertToInteger(string value)
        {
            int result;
            if (int.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        private decimal ConvertToDecimal(string value)
        {
            decimal result;
            if (decimal.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        private DateTime ConvertToDate(string value)
        {
            DateTime result;
            if (DateTime.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        private T ConvertToEnum<T>(string value) where T : struct
        {
            T result;
            if (Enum.TryParse<T>(value.Replace("-", "_"), out result))
            {
                return result;
            }
            else
            {
                return default(T);
            }
        }
    }
}