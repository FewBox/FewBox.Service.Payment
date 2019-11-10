namespace FewBox.Service.Payment.Model.Service
{
    public interface IPaypalService
    {
        IPaypalService Approver { get; set; }
        void HandleIPN(PaypalContext paypalContext);
    }
}