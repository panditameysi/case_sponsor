using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SponsorAPI.DAO;
using SponsorAPI.Models;

namespace SponsorAPI.Controllers
{
    [Route("sponsor/")]
    [ApiController]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorDao _sponsorDao;

        public SponsorController(ISponsorDao sponsorDao)
        {
            _sponsorDao = sponsorDao;
        }

        [HttpGet("SponsorsWithPaymentDetails", Name = "SponsorsWithPaymentDetails")]
        public async Task<ActionResult<List<SponsorsWithPaymentDetailsView>>> GetMatches()
        {
            var sponsors = await _sponsorDao.GetSponsorsWithPaymentDetails();
            if (sponsors == null)
            {
                return NotFound();
            }
            return Ok(sponsors);
        }

        [HttpGet("MatchesWithTotalPayments", Name = "MatchesWithTotalPayments")]
        public async Task<ActionResult<List<MatchesWithTotalPaymentsView>>> GetMatchesWithTotalPayments()
        {
            var matches = await _sponsorDao.GetMatchesWithTotalPayments();
            if (matches == null)
            {
                return NotFound();
            }
            return Ok(matches);
        }

        [HttpGet("SponsorsByMatchCountAsPerYear", Name = "SponsorsByMatchCountAsPerYear")]
        public async Task<ActionResult<List<SponsorsByMatchCountAsPerYearView>>> GetSponsorsByMatchCountAsPerYear(int year)
        {
            var sponsors = await _sponsorDao.GetSponsorsByMatchCountAsPerYear(year);
            if (sponsors == null)
            {
                return NotFound();
            }
            return Ok(sponsors);
        }

        [HttpPost("AddPayment",Name = "AddPayment")]
        public async Task<IActionResult> AddPayment([FromBody] Payment payment)
        {
            var paymentId = await _sponsorDao.AddPayment(payment);
            if (paymentId == 0)
            {
                return NotFound();
            }
            return Ok(paymentId);
        }
    }
}
