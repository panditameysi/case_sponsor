using System.Numerics;

namespace SponsorAPI.Models
{
    public class SponsorsWithPaymentDetailsView
    {
        public int SponsorID { get; set; }
        public string SponsorName { get; set; }
        public string IndustryType { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
        public double TotalPayments { get; set; }
        public int NumberOfPayments { get; set; }
        public DateOnly LatestPaymentDate { get; set; }
    }
}
