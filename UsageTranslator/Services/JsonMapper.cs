namespace UsageTranslator.Services
{
    public static class JsonMapper
    {
        public static Dictionary<string, string> LoadJsonTypeMap(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var typeMap = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            return typeMap ?? throw new Exception("Failed to deserialize JSON type map.");
        }
    }
}
