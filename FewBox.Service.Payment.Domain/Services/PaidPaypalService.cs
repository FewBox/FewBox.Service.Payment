using System.Security.Cryptography;
using FewBox.Core.Web.Notification;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;

namespace FewBox.Service.Payment.Domain.Services
{
    class PaidPaypalService : PaypalService
    {
        private INotificationHandler NotificationHandler { get; set; }
        public PaidPaypalService(PaypalConfig paypalConfig, INotificationHandler notificationHandler) : base(paypalConfig)
        {
            this.NotificationHandler = notificationHandler;
        }
        public override void HandleIPN(PaypalContext paypalContext)
        {
            if(paypalContext.PaymentInformation.PaymentStatusType==PaymentStatusType.Completed)
            {
            RSA rsa = RSA.Create();
            this.NotificationHandler.Handle("License", "");
            }
            else
            {
                this.Approver.HandleIPN(paypalContext);
            }
        }
    }
}