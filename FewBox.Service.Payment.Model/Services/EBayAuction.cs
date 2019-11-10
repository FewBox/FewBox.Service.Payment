using System;

namespace FewBox.Service.Payment.Model.Service
{
    public class EBayAuction
    {
        public string AuctionBuyerId { get; set; }
        public DateTime AuctionClosingDate { get; set; }
        public string ForAuction { get; set; }
    }
}