using System;

namespace FewBox.Service.Payment.Model.Service
{
    public interface IPaymentLogService
    {
        void Write(PaymentInfo paymentInfo);
    }
}