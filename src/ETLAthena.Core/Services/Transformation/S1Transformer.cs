using ETLAthena.Core.Models;

namespace ETLAthena.Core.Services.Transformation
{
    public class S1Transformer : IS1Transformer
    {
        public BuildingModel Transform(S1Model data)
        {
            return new BuildingModel {
                Id = data.Id,
                Name = "",
                Address = data.Address1 + ", " + data.Address2,
                Postcode = "", // TODO: Extract postcode from Address
                Latitude = data.Lat,
                Longitude = data.Lon,
                FloorCount = data.FloorCount,
                FloorArea = null,
                DataSource = "S1",
                IsMerged = false,
            };
        }

        public List<BuildingModel> Transform(List<S1Model> datalist) 
        {
            List<BuildingModel> buildings = new List<BuildingModel>();
            foreach (S1Model data in datalist)
            {
                buildings.Add(Transform(data));
            }

            return buildings;
        }
    }
}