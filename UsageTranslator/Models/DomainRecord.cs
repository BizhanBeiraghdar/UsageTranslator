namespace UsageTranslator.Models
{
    public class DomainRecord
    {
        public required string PartnerPurchasedPlanId { get; set; }
        public required string Domain { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is DomainRecord other)
            {
                return this.PartnerPurchasedPlanId == other.PartnerPurchasedPlanId &&
                       this.Domain == other.Domain;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PartnerPurchasedPlanId, Domain);
        }
    }
}
