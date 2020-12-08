using System.Collections.Generic;
using FewBox.SDK.Auth;
using FewBox.SDK.Core;
using FewBox.SDK.Mail;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;
using Microsoft.Extensions.Logging;

namespace FewBox.Service.Payment.Domain.Services
{
    class RefundPaypalService : PaypalService
    {
        public RefundPaypalService(PaypalConfig paypalConfig, IMailService mailService, IPlanService planService, ILogger logger)
        : base(paypalConfig, mailService, planService, logger)
        {
        }
        public override void HandleIPN(PaypalContext paypalContext)
        {
            if (paypalContext.PaymentInformation.PaymentStatusType == PaymentStatusType.Refunded)
            {
                var customer = new PlanCustomer
                {
                    Email = paypalContext.BuyerInformation.PayerEmail
                };
                int count;
                if (!int.TryParse(paypalContext.BasicInformation.ItemNumber, out count))
                {
                    count = 0;
                }
                var product = new PlanProduct
                {
                    Name = paypalContext.BasicInformation.ItemName,
                    Count = count
                };
                this.PlanService.QuitProPlan(customer, product);
                this.MailService.SendOpsNotification("Refund from Pro!", $"Welcome to join us - from FewBox {paypalContext.BasicInformation.ItemName} team.", new List<string> { paypalContext.BuyerInformation.PayerEmail });
                this.Logger.LogDebug($"Refund from {customer.Email}");
            }
            else
            {
                this.Approver.HandleIPN(paypalContext);
            }
        }
    }
}