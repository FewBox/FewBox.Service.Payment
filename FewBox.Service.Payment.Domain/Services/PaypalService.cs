using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;

namespace FewBox.Service.Payment.Domain.Services
{
    abstract class PaypalService : IPaypalService
    {
        protected PaypalConfig PaypalConfig { get; set; }
        protected PaypalService(PaypalConfig paypalConfig)
        {
            this.PaypalConfig = paypalConfig;
        }

        public IPaypalService Approver { get; set; }

        public abstract void HandleIPN(PaypalContext paypalContext);
    }
}