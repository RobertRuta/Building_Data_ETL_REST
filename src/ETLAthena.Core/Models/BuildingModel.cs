namespace ETLAthena.Core.Models
{
    public class BuildingModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? FloorCount { get; set; }
        public double? FloorArea { get; set; }
        public string? DataSource { get; set; } // To track the source of the last update
    }
}