namespace SponsorAPI.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int ContractID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentStatus { get; set; }
    }
}
