using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using ETLAthena.API;
using ETLAthena.Core.Models;
using System.Globalization;

namespace ETLAthena.API.Helpers
{
    public class DataIngestionUtility
    {
        static public List<BuildingModel> BuildingsFromCSV(string filePath)
        {
            var buildings = new List<BuildingModel>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                BadDataFound = context => {}
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                while (csv.Read())
                {
                    try
                    {
                        var record = csv.GetRecord<BuildingModel>();
                        if (record != null)
                        {
                            buildings.Add(record);
                        }
                    }
                    catch (CsvHelperException ex)
                    {
                        // Log the error or handle it as needed
                        Console.WriteLine($"Error reading CSV at row {csv.Context.Parser.Row}: {ex.Message}");
                        // Optionally continue with the next record
                    }
                }
            }

            return buildings;
        }
    }
}