using FewBox.SDK.Auth;
using FewBox.SDK.Mail;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;
using Microsoft.Extensions.Logging;

namespace FewBox.Service.Payment.Domain.Services
{
    class EndPaypalService : PaypalService
    {
        public EndPaypalService(PaypalConfig paypalConfig, IMailService mailService, IPlanService planService, ILogger logger)
        : base(paypalConfig, mailService, planService, logger)
        {
            this.Logger = logger;
        }
        public override void HandleIPN(PaypalContext paypalContext)
        {
            this.Logger.LogError("Handle IPN error!");
        }
    }
}