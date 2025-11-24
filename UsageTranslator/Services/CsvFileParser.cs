using System.Text.RegularExpressions;
using UsageTranslator.Models;

// 222222

namespace UsageTranslator.Services
{
    public class CsvFileParser
    {
        private readonly Dictionary<string, string> _typeMap;
        private readonly HashSet<int> _skippedPartnerIds = [26392];
        private readonly Dictionary<string, int> _unitReductionRules = new()
        {
            { "EA000001GB0O", 1000 },
            { "PMQ00005GB0R", 5000 },
            { "SSX006NR", 1000 },
            { "SPQ00001MB0R", 2000 }
        };

        public List<ChargeableRecord> ChargeableRecords { get; private set; } = [];
        public HashSet<DomainRecord> DomainRecords { get; private set; } = [];
        public Dictionary<string, int> RunningTotals { get; private set; } = [];

        public CsvFileParser(Dictionary<string, string> typeMap)
        {
            _typeMap = typeMap;
        }

        public void Parse(string path)
        {
            using var reader = new StreamReader(path);
            string? headerLine = reader.ReadLine();
            if (headerLine == null)
            {
                Console.WriteLine("CSV file is not valid.");
                return;
            }

            var headers = headerLine.Split(',');

            int partnerIdIndex = Array.IndexOf(headers, "PartnerID");
            int partNumberIndex = Array.IndexOf(headers, "PartNumber");
            int itemCountIndex = Array.IndexOf(headers, "itemCount");
            int planIndex = Array.IndexOf(headers, "plan");
            int accountGuidIndex = Array.IndexOf(headers, "accountGuid");
            int domainsIndex = Array.IndexOf(headers, "domains");

            while (!reader.EndOfStream)
            {
                var row = reader.ReadLine()?.Split(',');
                if (row == null || row.Length != headers.Length) continue;

                int.TryParse(row[partnerIdIndex], out int partnerId);
                var partNumber = row[partNumberIndex];
                var itemCountStr = row[itemCountIndex];
                var plan = row[planIndex];
                var rawGuid = row[accountGuidIndex];
                var domain = row[domainsIndex];

                if (string.IsNullOrWhiteSpace(partNumber))
                {
                    Console.WriteLine("Skipping >>>> missing PartNumber");
                    continue;
                }

                if (!_typeMap.ContainsKey(partNumber))
                {
                    Console.WriteLine($"Skipping >>>> PartNumber {partNumber} not found in typemap");
                    continue;
                }

                if (!int.TryParse(itemCountStr, out int itemCount) || itemCount <= 0)
                {
                    Console.WriteLine($"Skipping >>>> invalid itemCount {itemCountStr}");
                    continue;
                }

                if (_skippedPartnerIds.Contains(partnerId))
                {
                    Console.WriteLine($"Skipping >>>> PartnerID {partnerId} is in the ignore list");
                    continue;
                }

                string product = _typeMap[partNumber];
                int usage = ApplyUnitReduction(partNumber, itemCount);
                string cleanedGuid = CleanGuid(rawGuid);

                ChargeableRecords.Add(new ChargeableRecord
                {
                    PartnerId = partnerId,
                    Product = product,
                    PartnerPurchasedPlanId = cleanedGuid,
                    Plan = plan,
                    Usage = usage
                });

                DomainRecords.Add(new DomainRecord
                {
                    PartnerPurchasedPlanId = cleanedGuid,
                    Domain = domain
                });

                if (!RunningTotals.ContainsKey(product))
                    RunningTotals[product] = 0;
                RunningTotals[product] += itemCount;
            }
        }

        private int ApplyUnitReduction(string partNumber, int itemCount)
        {
            return _unitReductionRules.TryGetValue(partNumber, out int divisor)
                ? itemCount / divisor
                : itemCount;
        }

        private string CleanGuid(string guid) => Regex.Replace(guid, "[^a-zA-Z0-9]", "");
    }
}
