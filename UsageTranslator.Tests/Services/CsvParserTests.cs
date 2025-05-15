using System.Reflection;
using UsageTranslator.Services;

namespace UsageTranslator.Tests.Services
{
    public class CsvParserTests
    {
        [Fact]
        public void ApplyUnitReduction_KnownPartNumber_AppliesRule()
        {
            var parser = new CsvFileParser([]);

            var method = typeof(CsvFileParser)
                .GetMethod("ApplyUnitReduction", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.NotNull(method);

            var result = method!.Invoke(parser, ["PMQ00005GB0R", 10000]);
            Assert.Equal(2, result);
        }
    }
}
