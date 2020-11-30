using FewBox.SDK.Auth;
using FewBox.SDK.Mail;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;

namespace FewBox.Service.Payment.Domain.Services
{
    abstract class PaypalService : IPaypalService
    {
        protected PaypalConfig PaypalConfig { get; set; }
        protected IMailService MailService { get; set; }
        protected IPlanService PlanService { get; set; }
        protected PaypalService(PaypalConfig paypalConfig, IMailService mailService, IPlanService planService)
        {
            this.PaypalConfig = paypalConfig;
            this.MailService = mailService;
            this.PlanService = planService;
        }

        public IPaypalService Approver { get; set; }

        public abstract void HandleIPN(PaypalContext paypalContext);
    }
}