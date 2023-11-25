using ETLAthena.Core.Models;
namespace ETLAthena.Core.DataStorage
{
    public interface IDataStorageService
    {
        BuildingModel GetBuilding(int id);
        IEnumerable<BuildingModel> GetAllBuildings();
        void UpdateOrCreateBuilding(BuildingModel building);
    }
}
