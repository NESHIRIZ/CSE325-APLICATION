# W01 Assignment Evidence - Contoso Pizza

## GET - Retrieve All Pizzas
* **Request URL:** https://localhost:7294/pizza
* **Server Response Code:** 200 OK
* **Response Body (Showing 3 existing records):**
[
  { "id": 1, "name": "Classic Italian", "isGlutenFree": false },
  { "id": 2, "name": "Veggie", "isGlutenFree": true },
  { "id": 3, "name": "BBQ Chicken", "isGlutenFree": false }
]

## POST - Create a New Pizza
* **Request URL:** https://localhost:7294/pizza
* **Request Payload:** { "name": "Hawaii", "isGlutenFree": false }
* **Server Response Code:** 201 Created
* **Response Body:** { "id": 4, "name": "Hawaii", "isGlutenFree": false }

## PUT - Update an Existing Pizza
* **Request URL:** https://localhost:7294/pizza/3
* **Request Payload:** { "id": 3, "name": "Spicy BBQ Chicken", "isGlutenFree": false }
* **Server Response Code:** 204 No Content

## DELETE - Remove a Pizza
* **Request URL:** https://localhost:7294/pizza/1
* **Server Response Code:** 204 No Content

---

## Part 2: Sales Summary Report Function Code

Below is the text copy of the working C# function that builds the formatted 'summaryReport.txt' text file using StringBuilder and currency interpolation:

```csharp
void GenerateSalesSummary(string storesDirectory, string salesTotalDir, IEnumerable<string> salesFiles)
{
    double grandTotal = 0;
    System.Text.StringBuilder reportBuilder = new System.Text.StringBuilder();

    reportBuilder.AppendLine("Sales Summary");
    reportBuilder.AppendLine("----------------------------");
    
    int grandTotalPlaceholderIndex = reportBuilder.Length;
    reportBuilder.AppendLine("{GRAND_TOTAL_TOKEN}"); 
    reportBuilder.AppendLine(" Details:");

    foreach (var file in salesFiles)
    {      
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        double fileTotal = data?.Total ?? 0;
        grandTotal += fileTotal;

        string fileName = Path.GetFileName(file);
        reportBuilder.AppendLine($"  {fileName}: {fileTotal:C}");
    }

    string grandTotalText = $" Total Sales: {grandTotal:C}\n";
    reportBuilder.Replace("{GRAND_TOTAL_TOKEN}", grandTotalText);

    string summaryPath = Path.Combine(salesTotalDir, "summaryReport.txt");
    File.WriteAllText(summaryPath, reportBuilder.ToString());
}
```
