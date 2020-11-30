using FewBox.SDK.Auth;
using FewBox.SDK.Mail;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;
using Microsoft.Extensions.Logging;

namespace FewBox.Service.Payment.Domain.Services
{
    public class PaypalServiceBuilder
    {
        private PaypalConfig PaypalConfig { get; set; }
        private IMailService MailService { get; set; }
        private IPlanService PlanService { get; set; }
        private ILogger Logger { get; set; }
        public PaypalServiceBuilder(PaypalConfig paypalConfig, IMailService mailService, IPlanService planService, ILogger<PaypalServiceBuilder> logger)
        {
            this.PaypalConfig = paypalConfig;
            this.MailService = mailService;
            this.PlanService = planService;
            this.Logger = logger;
        }

        public IPaypalService Build()
        {
            IPaypalService paidPaypalService = new PaidPaypalService(this.PaypalConfig, this.MailService, this.PlanService);
            IPaypalService refundPaypalService = new RefundPaypalService(this.PaypalConfig, this.MailService, this.PlanService);
            IPaypalService endPaypalService = new EndPaypalService(this.PaypalConfig, this.MailService, this.PlanService, this.Logger);
            paidPaypalService.Approver = refundPaypalService;
            refundPaypalService.Approver = endPaypalService;
            return paidPaypalService;
        }
    }
}