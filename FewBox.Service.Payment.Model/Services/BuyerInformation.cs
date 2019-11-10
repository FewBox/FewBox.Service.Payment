namespace FewBox.Service.Payment.Model.Service
{
    public class BuyerInformation
    {
        public AddressStatusType AddressStatusType { get; set; }
        public PayerStatusType PayerStatusType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PayerEmail { get; set; }
        public string PayerId { get; set; }
        public string AddressName { get; set; }
        public string AddressCountry { get; set; }
        public string AddressZip { get; set; }
        public string AddressState { get; set; }
        public string AddressCity { get; set; }
        public string AddressStreet { get; set; }
    }
}