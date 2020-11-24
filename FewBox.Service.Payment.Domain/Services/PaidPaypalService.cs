using System.Collections.Generic;
using FewBox.SDK.Mail;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;

namespace FewBox.Service.Payment.Domain.Services
{
    class PaidPaypalService : PaypalService
    {
        private IMailService MailService { get; set; }
        public PaidPaypalService(PaypalConfig paypalConfig, IMailService mailService) : base(paypalConfig)
        {
            this.MailService = mailService;
        }
        public override void HandleIPN(PaypalContext paypalContext)
        {
            if (paypalContext.PaymentInformation.PaymentStatusType == PaymentStatusType.Completed)
            {
                //RSA rsa = RSA.Create();
                this.MailService.SendOpsNotification("License", "XXX", new List<string> { paypalContext.BuyerInformation.PayerEmail });
            }
            else
            {
                this.Approver.HandleIPN(paypalContext);
            }
        }
    }
}