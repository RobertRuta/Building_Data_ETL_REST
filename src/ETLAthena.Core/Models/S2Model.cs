namespace ETLAthena.Core.Models
{
    public class S2Model
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        public double[]? Coordinates { get; set; }
        public double? FloorArea { get; set; }
    }
}
