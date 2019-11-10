using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;

namespace FewBox.Service.Payment.Domain.Services
{
    class RefundPaypalService : PaypalService
    {
        public RefundPaypalService(PaypalConfig paypalConfig) : base(paypalConfig)
        {
        }
        public override void HandleIPN(PaypalContext paypalContext)
        {
            
        }
    }
}