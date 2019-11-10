using FewBox.Core.Web.Notification;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;

namespace FewBox.Service.Payment.Domain.Services
{
    public class PaypalServiceBuilder
    {
        private PaypalConfig PaypalConfig { get; set; }
        private INotificationHandler NotificationHandler { get; set; }
        public PaypalServiceBuilder(PaypalConfig paypalConfig, INotificationHandler notificationHandler)
        {
            this.PaypalConfig = paypalConfig;
            this.NotificationHandler = notificationHandler;
        }

        public IPaypalService Build()
        {
            IPaypalService paidPaypalService = new PaidPaypalService(this.PaypalConfig, this.NotificationHandler);
            IPaypalService refundPaypalService = new RefundPaypalService(this.PaypalConfig);
            paidPaypalService.Approver = refundPaypalService;
            return paidPaypalService;
        }
    }
}