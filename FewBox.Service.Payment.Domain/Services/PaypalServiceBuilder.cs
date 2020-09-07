using FewBox.Core.Web.Notification;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;
using Microsoft.Extensions.Logging;

namespace FewBox.Service.Payment.Domain.Services
{
    public class PaypalServiceBuilder
    {
        private PaypalConfig PaypalConfig { get; set; }
        private INotificationHandler NotificationHandler { get; set; }
        private ILogger Logger { get; set; }
        public PaypalServiceBuilder(PaypalConfig paypalConfig, INotificationHandler notificationHandler, ILogger<PaypalServiceBuilder> logger)
        {
            this.PaypalConfig = paypalConfig;
            this.NotificationHandler = notificationHandler;
            this.Logger = logger;
        }

        public IPaypalService Build()
        {
            IPaypalService paidPaypalService = new PaidPaypalService(this.PaypalConfig, this.NotificationHandler);
            IPaypalService refundPaypalService = new RefundPaypalService(this.PaypalConfig);
            IPaypalService endPaypalService = new EndPaypalService(this.PaypalConfig, this.Logger);
            paidPaypalService.Approver = refundPaypalService;
            refundPaypalService.Approver = endPaypalService;
            return paidPaypalService;
        }
    }
}