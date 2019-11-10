using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;

namespace FewBox.Service.Payment.Domain.Services
{
    class PaidPaypalService : PaypalService
    {
        public PaidPaypalService(PaypalConfig paypalConfig) : base(paypalConfig)
        {
        }
        public override void HandleIPN(PaypalContext paypalContext)
        {
            
        }
    }
}