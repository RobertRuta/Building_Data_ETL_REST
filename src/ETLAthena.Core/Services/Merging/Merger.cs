using System.Linq;
using ETLAthena.Core.Models;
using ETLAthena.Core.DataStorage;

namespace ETLAthena.Core.Services.Merging
{
    public class Merger : IMerger
    {
        private readonly IDataStorageService _dataStorageService;

        public Merger(IDataStorageService dataStorageService)
        {
            _dataStorageService = dataStorageService;
        }

        public void Merge(BuildingModel data)
        {
            BuildingModel existingData = FindMatchingBuilding(data);

            if (existingData != null)
            {
                UpdateExistingData(existingData, data);
            }
            else
            {
                _dataStorageService.UpdateOrCreateBuilding(data);
            }
        }

        private BuildingModel FindMatchingBuilding(BuildingModel data)
        {
            // Match by ID
            var existingDataById = _dataStorageService.GetBuilding(data.Id);
            if (existingDataById != null && existingDataById.DataSource == data.DataSource)
                return existingDataById;

            // Match by Name // Quicker to sort?
            var existingDataByName = _dataStorageService.GetAllBuildings().FirstOrDefault(b => b.Name == data.Name);

            // Match by Postcode
            var existingDataByPostCode = _dataStorageService.GetAllBuildings().FirstOrDefault(b => b.Postcode == data.Postcode);

            // Choose the matching logic priority here
            return existingDataByName ?? existingDataByPostCode;
        }

        private void UpdateExistingData(BuildingModel existingData, BuildingModel newData) 
        {
            existingData.Name = newData.Name ?? existingData.Name;
            existingData.Address = newData.Address ?? existingData.Address;
            existingData.Postcode = newData.Postcode ?? existingData.Postcode;
            existingData.Latitude = newData.Latitude ?? existingData.Latitude;
            existingData.Longitude = newData.Longitude ?? existingData.Longitude;
            existingData.FloorCount = newData.FloorCount ?? existingData.FloorCount;
            existingData.FloorArea = newData.FloorArea ?? existingData.FloorArea;
            existingData.DataSource = newData.DataSource != existingData.DataSource ? "merged" : existingData.DataSource;
        }
    }
}
