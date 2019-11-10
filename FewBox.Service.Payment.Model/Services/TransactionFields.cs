namespace FewBox.Service.Payment.Model.Service
{
    public class TransactionFields
    {
        public TxnType TxnType { get; set; }
        public string TxnId { get; set; }
        public string NotifyVersion { get; set; }
        public string ParentTxnId { get; set; }
        public string ReasonCode { get; set; }
        public string ReceiptID { get; set; }
    }
}