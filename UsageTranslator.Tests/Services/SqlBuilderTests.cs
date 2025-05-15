using System.Reflection;
using UsageTranslator.Services;

namespace UsageTranslator.Tests.Services
{
    public class SqlBuilderTests
    {
        [Fact]
        public void MakeSqlSafe_SingleQuote_IsEscaped()
        {
            var method = typeof(SqlBuilder)
            .GetMethod("MakeSqlSafe", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(method);

            string input = "Mother's Day";
            string expected = "Mother''s Day";

            var result = method!.Invoke(null, [input]);

            Assert.Equal(expected, result);
        }
    }
}
 