using UsageTranslator.Services;

string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
string csvPath = Path.Combine(projectRoot, "Data", "Sample_Report.csv");
string jsonPath = Path.Combine(projectRoot, "Data", "typemap.json");
string outputPath = Path.Combine(projectRoot, "Output", "output.sql");

try
{
    var typeMap = JsonMapper.LoadJsonTypeMap(jsonPath);
    Console.WriteLine("file typemap loaded.");

    var parser = new CsvFileParser(typeMap);
    parser.Parse(csvPath);
    Console.WriteLine("CSV file has been parsed.");

    Console.WriteLine("\nRunning totals per product:");
    foreach (var (product, total) in parser.RunningTotals)
        Console.WriteLine($"  - {product}: {total}");

    var chargeableSql = SqlBuilder.BuildChargeableInsert(parser.ChargeableRecords);
    var domainsSql = SqlBuilder.BuildDomainInsert(parser.DomainRecords);

    Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

    File.WriteAllText(outputPath, $"{chargeableSql}\n\n{domainsSql}");

    Console.WriteLine($"\nSQL has been written to: {outputPath}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
