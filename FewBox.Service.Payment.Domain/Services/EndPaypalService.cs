using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;
using Microsoft.Extensions.Logging;

namespace FewBox.Service.Payment.Domain.Services
{
    class EndPaypalService : PaypalService
    {
        private ILogger Logger { get; set; }
        public EndPaypalService(PaypalConfig paypalConfig, ILogger logger) : base(paypalConfig)
        {
            this.Logger = logger;
        }
        public override void HandleIPN(PaypalContext paypalContext)
        {
            this.Logger.LogError("Handle IPN error!");
        }
    }
}