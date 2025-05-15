using System.Text;
using UsageTranslator.Models;

namespace UsageTranslator.Services
{
    /// <summary>
    /// This class is responsible for building SQL insert statements for chargeable and domain records.
    /// To achive this, it uses StringBuilder to construct the SQL statements and print the sql queries.
    /// In real world, this would be implemented by parameterized queries to avoid SQL Injection (using SqlCommand and parameters).
    /// </summary>
    public static class SqlBuilder
    {
        public static string BuildChargeableInsert(List<ChargeableRecord> records)
        {
            if (records.Count == 0)
                return "-- Ther is not any chargeable record to insert.";

            var sb = new StringBuilder();
            sb.AppendLine("-- chargeable table");
            sb.AppendLine("INSERT INTO chargeable (partnerID, product, partnerPurchasedPlanID, plan, usage) VALUES");

            for (int i = 0; i < records.Count; i++)
            {
                var r = records[i];
                string line = $"({r.PartnerId}, '{MakeSqlSafe(r.Product)}', '{MakeSqlSafe(r.PartnerPurchasedPlanId)}', '{MakeSqlSafe(r.Plan)}', {r.Usage})";
                sb.AppendLine(i < records.Count - 1 ? $"{line}," : $"{line};");
            }

            return sb.ToString();
        }

        public static string BuildDomainInsert(IEnumerable<DomainRecord> records)
        {
            var list = records.ToList();
            if (list.Count == 0)
                return "-- Ther is not any domain record to insert.";

            var sb = new StringBuilder();
            sb.AppendLine("-- domains table");
            sb.AppendLine("INSERT INTO domains (partnerPurchasedPlanID, domain) VALUES");

            for (int i = 0; i < list.Count; i++)
            {
                var r = list[i];
                string line = $"('{MakeSqlSafe(r.PartnerPurchasedPlanId)}', '{MakeSqlSafe(r.Domain)}')";
                sb.AppendLine(i < list.Count - 1 ? $"{line}," : $"{line};");
            }

            return sb.ToString();
        }

        private static string MakeSqlSafe(string input)
        {
            return input.Replace("'", "''");
        }
    }
}
