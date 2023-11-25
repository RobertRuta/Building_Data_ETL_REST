using ETLAthena.Core.Models;

namespace ETLAthena.Core.DataStorage
{
    public class InMemoryDataStore : IDataStorageService
    {
        private readonly Dictionary<int, BuildingModel> _buildings;

        public InMemoryDataStore()
        {
            _buildings = new Dictionary<int, BuildingModel>();
        }

        public BuildingModel GetBuilding(int id)
        {
            _buildings.TryGetValue(id, out var building);
            return building;
        }

        public IEnumerable<BuildingModel> GetAllBuildings()
        {
            return _buildings.Values;
        }

        public void UpdateOrCreateBuilding(BuildingModel building)
        {
            if (building == null) return;

            _buildings[building.Id] = building;
        }
    }
}
