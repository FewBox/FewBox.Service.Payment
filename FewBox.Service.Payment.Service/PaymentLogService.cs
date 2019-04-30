using System;
using FewBox.Service.Payment.Model.Service;

namespace FewBox.Service.Payment.Service
{
    public class PaymentLogService : IPaymentLogService
    {
        public void Write(PaymentInfo paymentInfo)
        {
            // Todo: Should use FewBox.Service.Log service.
            Console.WriteLine(paymentInfo.Body);
        }
    }
}
