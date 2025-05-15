namespace UsageTranslator.Models
{
    public class ChargeableRecord
    {
        public int PartnerId { get; set; }
        public required string Product { get; set; }
        public required string PartnerPurchasedPlanId { get; set; }
        public required string Plan { get; set; }
        public int Usage { get; set; }
    }
}
