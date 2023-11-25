using ETLAthena.Core.Models;

namespace ETLAthena.Core.Services.Transformation
{
    public class S2Transformer : IS2Transformer
    {
        public BuildingModel Transform(S2Model data)
        {
            return new BuildingModel {
                Id = data.Id,
                Name = data.Name,
                Address = data.Address,
                Postcode = data.Postcode, // TODO: Extract postcode from Address
                Latitude = data.Coordinates[0],
                Longitude = data.Coordinates[1],
                FloorCount = null,
                FloorArea = data.FloorArea,
                DataSource = "S2",
            };
        }
    }
}