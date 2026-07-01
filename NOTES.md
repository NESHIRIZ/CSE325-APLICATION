# Submission Notes

## Additional Seeded Pizza Record
The service layer includes a third seeded pizza profile to satisfy the rubric requirement: "BBQ Chicken & Bacon" with Id 3 and IsGlutenFree set to false.

## Verified HTTP Operation Matrix

| HTTP Operation | Endpoint | Verified Response Code |
| --- | --- | --- |
| GET | /api/pizza | 200 OK |
| GET by id | /api/pizza/{id} | 200 OK |
| POST | /api/pizza | 201 Created |
| PUT | /api/pizza/{id} | 204 No Content |
| DELETE | /api/pizza/{id} | 204 No Content |

## Sales Summary Generator
```csharp
public static string GenerateSalesSummary(string salesDir)
{
    var builder = new StringBuilder();
    var salesFiles = Directory.EnumerateFiles(salesDir, "sales.json", SearchOption.AllDirectories)
        .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
        .ToList();

    decimal totalSales = 0m;
    var details = new List<string>();

    foreach (var salesFile in salesFiles)
    {
        var directoryName = Path.GetFileName(Path.GetDirectoryName(salesFile) ?? string.Empty);
        var rawValue = File.ReadAllText(salesFile).Trim();

        if (decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var salesAmount))
        {
            totalSales += salesAmount;
            details.Add($"{directoryName}: {salesAmount:C}");
        }
    }

    builder.AppendLine("Sales Summary");
    builder.AppendLine("----------------------------");
    builder.AppendLine($"Total Sales: {totalSales:C}");
    builder.AppendLine();
    builder.AppendLine("Details:");

    foreach (var detail in details)
    {
        builder.AppendLine($" {detail}");
    }

    return builder.ToString();
}
```
