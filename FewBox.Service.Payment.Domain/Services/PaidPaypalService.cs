using System.Collections.Generic;
using FewBox.SDK.Auth;
using FewBox.SDK.Core;
using FewBox.SDK.Mail;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;
using Microsoft.Extensions.Logging;

namespace FewBox.Service.Payment.Domain.Services
{
    class PaidPaypalService : PaypalService
    {
        public PaidPaypalService(PaypalConfig paypalConfig, IMailService mailService, IPlanService planService, ILogger logger)
        : base(paypalConfig, mailService, planService, logger)
        {
        }
        public override void HandleIPN(PaypalContext paypalContext)
        {
            if (paypalContext.PaymentInformation.PaymentStatusType == PaymentStatusType.Completed)
            {
                //RSA rsa = RSA.Create();
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
                this.PlanService.UpgradeProPlan(customer, product);
                this.MailService.SendOpsNotification("Upgrade from Free Plan to Pro Plan!", $"Welcome to join us - from FewBox {paypalContext.BasicInformation.ItemName} team.", new List<string> { paypalContext.BuyerInformation.PayerEmail });
                this.Logger.LogDebug($"Paid from {customer.Email}");
            }
            else
            {
                this.Approver.HandleIPN(paypalContext);
            }
        }
    }
}