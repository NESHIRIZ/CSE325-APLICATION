using System.Globalization;
using System.Text;

namespace SalesTracker;

internal static class Program
{
    private static void Main()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var storesDirectory = Path.Combine(currentDirectory, "stores");
        var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");

        if (!Directory.Exists(storesDirectory))
        {
            Directory.CreateDirectory(storesDirectory);
        }

        var sampleStores = new[] { "store101", "store102", "store103" };
        foreach (var store in sampleStores)
        {
            var storePath = Path.Combine(storesDirectory, store);
            Directory.CreateDirectory(storePath);

            var salesFile = Path.Combine(storePath, "sales.json");
            var value = store switch
            {
                "store101" => "1250.45",
                "store102" => "980.20",
                "store103" => "1540.75",
                _ => "0"
            };

            File.WriteAllText(salesFile, value);
        }

        var outputPath = Path.Combine(salesTotalDir, "summaryReport.txt");
        var summary = GenerateSalesSummary(storesDirectory);
        Directory.CreateDirectory(salesTotalDir);
        File.WriteAllText(outputPath, summary);

        Console.WriteLine(summary);
    }

    public static string GenerateSalesSummary(string salesDir)
    {
        var builder = new StringBuilder();
        var salesFiles = Directory.EnumerateFiles(salesDir, "sales.json", SearchOption.AllDirectories)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToList();

        decimal totalSales = 0m;

        foreach (var salesFile in salesFiles)
        {
            var directoryName = Path.GetFileName(Path.GetDirectoryName(salesFile) ?? string.Empty);
            var rawValue = File.ReadAllText(salesFile).Trim();

            if (decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var salesAmount))
            {
                totalSales += salesAmount;
                var fileName = Path.GetFileName(salesFile);
                builder.AppendLine($"  {fileName}: {salesAmount:C}");
            }
        }

        var report = new StringBuilder();
        report.AppendLine("Sales Summary");
        report.AppendLine("----------------------------");
        report.AppendLine($" Total Sales: {totalSales:C}");
        report.AppendLine();
        report.AppendLine(" Details:");
        report.Append(builder);

        return report.ToString();
    }
}
