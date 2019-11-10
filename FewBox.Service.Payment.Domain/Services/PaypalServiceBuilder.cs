using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;

namespace FewBox.Service.Payment.Domain.Services
{
    public class PaypalServiceBuilder
    {
        private PaypalConfig PaypalConfig { get; set; }
        public PaypalServiceBuilder(PaypalConfig paypalConfig)
        {
            this.PaypalConfig = paypalConfig;
        }

        public IPaypalService Build()
        {
            IPaypalService paidPaypalService = new PaidPaypalService(this.PaypalConfig);
            IPaypalService refundPaypalService = new RefundPaypalService(this.PaypalConfig);
            paidPaypalService.Approver = refundPaypalService;
            return paidPaypalService;
        }
    }
}