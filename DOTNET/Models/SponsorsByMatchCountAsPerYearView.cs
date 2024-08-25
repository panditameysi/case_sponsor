using System.Numerics;

namespace SponsorAPI.Models
{
    public class SponsorsByMatchCountAsPerYearView
    {
        public int SponsorID { get; set; }
        public string SponsorName { get; set; }
        public int MatchCount { get; set; }
    }
}
