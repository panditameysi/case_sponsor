using System.Numerics;

namespace SponsorAPI.Models
{
    public class MatchesWithTotalPaymentsView
    {
        public int MatchID { get; set; }
        public string MatchName { get; set; }
        public DateOnly MatchDate { get; set; }
        public string Location { get; set; }
        public double TotalPayments { get; set; }
    }
}
