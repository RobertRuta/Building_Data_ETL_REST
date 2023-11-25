using ETLAthena.Core.Models;
namespace ETLAthena.Core.DataStorage
{
    public interface IDataStorageService
    {
        BuildingModel GetBuilding(int id);
        void UpdateOrCreateBuilding(BuildingModel building);
    }
}
