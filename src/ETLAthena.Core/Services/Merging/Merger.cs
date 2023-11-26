using ETLAthena.Core.Models;
using ETLAthena.Core.DataStorage;

namespace ETLAthena.Core.Services.Merging
{
    public class Merger : IMerger
    {
        private readonly IDataStorageService _dataStorageService;
        private Matcher matcher;

        public Merger(IDataStorageService dataStorageService)
        {
            _dataStorageService = dataStorageService;
            matcher = new Matcher(_dataStorageService);
        }


        public void Merge(BuildingModel data)
        {
            BuildingModel existingData = matcher.FindMatchingBuilding(data);

            if (existingData != null)
            {
                UpdateExistingData(existingData, data);
            }
            else
            {
                _dataStorageService.UpdateOrCreateBuilding(data);
            }
        }


        private void UpdateExistingData(BuildingModel existingData, BuildingModel newData) 
        {
            MatchType matchType = matcher.matchType;

            // Check if existingData is null
            if (existingData == null)
                throw new ArgumentNullException(nameof(existingData), "Existing data cannot be null.");

            // Check if matchType is not 'id' or 'fuzzy'
            if ((matchType != MatchType.id && matchType != MatchType.fuzzy) || (matchType == MatchType.none))
                throw new InvalidOperationException($"Invalid match type: {matchType}. Only 'id' and 'fuzzy' are allowed.");

            switch (matchType)
            {
                case MatchType.id:
                    existingData.Id = existingData.Id;
                    existingData.Name = newData.Name ?? existingData.Name;
                    existingData.Address = newData.Address ?? existingData.Address;
                    existingData.Postcode = newData.Postcode ?? existingData.Postcode;
                    existingData.Latitude = newData.Latitude ?? existingData.Latitude;
                    existingData.Longitude = newData.Longitude ?? existingData.Longitude;
                    existingData.FloorCount = newData.FloorCount ?? existingData.FloorCount;
                    existingData.FloorArea = newData.FloorArea ?? existingData.FloorArea;
                    existingData.DataSource = newData.DataSource;
                    break;
                
                case MatchType.fuzzy:
                    existingData.Id = newData.Id;
                    existingData.Name = newData.Name ?? existingData.Name;
                    existingData.Address = newData.Address ?? existingData.Address;
                    existingData.Postcode = newData.Postcode ?? existingData.Postcode;
                    existingData.Latitude = newData.Latitude ?? existingData.Latitude;
                    existingData.Longitude = newData.Longitude ?? existingData.Longitude;
                    existingData.FloorCount = newData.FloorCount ?? existingData.FloorCount;
                    existingData.FloorArea = newData.FloorArea ?? existingData.FloorArea;
                    existingData.DataSource = newData.DataSource;
                    break;
                
                default:
                    throw new InvalidOperationException($"Invalid match type: {matchType}. Only 'id' and 'fuzzy' are allowed.");

            }
        }
    }
}
