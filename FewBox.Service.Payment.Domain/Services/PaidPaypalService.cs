using System.Collections.Generic;
using FewBox.SDK.Auth;
using FewBox.SDK.Mail;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;

namespace FewBox.Service.Payment.Domain.Services
{
    class PaidPaypalService : PaypalService
    {
        public PaidPaypalService(PaypalConfig paypalConfig, IMailService mailService, IPlanService planService) 
        : base(paypalConfig, mailService, planService)
        {
        }
        public override void HandleIPN(PaypalContext paypalContext)
        {
            if (paypalContext.PaymentInformation.PaymentStatusType == PaymentStatusType.Completed)
            {
                //RSA rsa = RSA.Create();
                this.PlanService.UpgradeProPlan(paypalContext.BuyerInformation.PayerEmail, paypalContext.BasicInformation.ItemName);
                this.MailService.SendOpsNotification("Upgrade from Free Plan to Pro Plan!", $"Welcome to join us - from FewBox {paypalContext.BasicInformation.ItemName} team.", new List<string> { paypalContext.BuyerInformation.PayerEmail });
            }
            else
            {
                this.Approver.HandleIPN(paypalContext);
            }
        }
    }
}