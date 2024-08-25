using SponsorAPI.Models;
namespace SponsorAPI.DAO
{
    public interface ISponsorDao
    {
        Task<List<SponsorsWithPaymentDetailsView>> GetSponsorsWithPaymentDetails(); 

        Task<List<MatchesWithTotalPaymentsView>> GetMatchesWithTotalPayments();
        Task<List<SponsorsByMatchCountAsPerYearView>> GetSponsorsByMatchCountAsPerYear(int year);
        Task<int> AddPayment(Payment payment);

    }
}
