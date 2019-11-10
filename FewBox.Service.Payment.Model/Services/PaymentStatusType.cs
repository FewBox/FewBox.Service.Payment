namespace FewBox.Service.Payment.Model.Service
{
    public enum PaymentStatusType
    {
        Unknown,
        Canceled_Reversal,
        Completed,
        Declined,
        Expaired,
        Failed,
        In_Progress,
        Partially_Refunded,
        Pending,
        Processed,
        Refunded,
        Reversed,
        Voided
    }
}