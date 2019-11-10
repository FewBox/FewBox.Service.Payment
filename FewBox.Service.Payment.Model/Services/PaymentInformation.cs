using System;

namespace FewBox.Service.Payment.Model.Service
{
    public class PaymentInformation
    {
        public PaymentType PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatusType PaymentStatusType { get; set; }
        public string PendingReason { get; set; }
    }
}