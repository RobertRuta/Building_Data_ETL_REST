using ETLAthena.Core.Models;
using ETLAthena.Core.DataStorage;
using ETLAthena.Core.Services.Merging.Helpers;
using System.ComponentModel;

namespace ETLAthena.Core.Services.Merging
{
    // Define possible match types
    public enum MatchType 
    {
        id,
        fuzzy,
        none
    }

    public class Matcher
    {
        private readonly IDataStorageService _dataStorageService;
        public MatchType matchType;

        public Matcher(IDataStorageService dataStorageService)
        {
            _dataStorageService = dataStorageService;
        }


        public BuildingModel FindMatchingBuilding(BuildingModel incomingData)
        {  
            // ID matching - match if id same and both buildings from same source

            BuildingModel existingDataById = _dataStorageService.GetBuilding(incomingData.Id);
            
            if (existingDataById != null && existingDataById.DataSource == incomingData.DataSource)
            {
                matchType = MatchType.id;
                return existingDataById;
            }
            
            // Otherwise check for fuzzy similarities between postcode and name - LevenshteinDistance

            matchType = MatchType.fuzzy;

            // Returns null if no match found or if postcode match found
            return FindFuzzyMatchingBuilding(incomingData);            
        }


        public BuildingModel FindFuzzyMatchingBuilding(BuildingModel incomingData)
        {
            var allBuildings = _dataStorageService.GetAllBuildings();
            BuildingModel bestMatchByName = null;
            BuildingModel bestMatchByPostcode = null;
            BuildingModel bestMatchByAddress = null;
            int bestNameDistance = int.MaxValue;
            int bestPostcodeDistance = int.MaxValue;
            int bestAddressDistance = int.MaxValue;

            int nameThreshold = incomingData.Name != null ? incomingData.Name.Length / 4 : -1;
            int addressThreshold = incomingData.Address != null ? (int)(incomingData.Address.Length * 0.3f) : -1;
            int postcodeThreshold = incomingData.Postcode != null ? (int)(incomingData.Postcode.Length * 0.1f) : -1;

            foreach (var building in allBuildings)
            {
                int distanceByName = MergeHelpers.LevenshteinDistance(building.Name, incomingData.Name);
                int distanceByPostcode = MergeHelpers.LevenshteinDistance(building.Postcode, incomingData.Postcode);
                int distanceByAddress = MergeHelpers.LevenshteinDistance(building.Address, incomingData.Address);

                if (distanceByName < bestNameDistance)
                {
                    bestMatchByName = building;
                    bestNameDistance = distanceByName;
                }

                if (distanceByAddress < bestAddressDistance)
                {
                    bestMatchByAddress = building;
                    bestAddressDistance = distanceByAddress;
                }

                if (distanceByPostcode < bestPostcodeDistance)
                {
                    bestMatchByPostcode = building;
                    bestPostcodeDistance = distanceByPostcode;
                }
            }

            // Decision logic to combine results
            if (bestMatchByName != null && bestMatchByName == bestMatchByAddress && bestMatchByName == bestMatchByPostcode && bestNameDistance < nameThreshold)
            {
                // Strongest match: all attributes point to the same building
                return bestMatchByName;
            }
            else if (bestMatchByName != null && bestMatchByName == bestMatchByAddress && bestNameDistance < nameThreshold)
            {
                // Next strongest match: name and address match
                return bestMatchByName;
            }
            else if (bestMatchByName != null && bestNameDistance < nameThreshold)
            {
                // Name match prioritized
                return bestMatchByName;
            }
            else if (bestMatchByAddress != null && bestAddressDistance < addressThreshold)
            {
                // Address match
                return bestMatchByAddress;
            }
            else
            {
                // I don't trust the postcode match, since many buildings are associated with one postcode
                matchType = MatchType.none;
                return null;
            }
        }

        public BuildingModel FindPrimitiveMatchingBuilding(BuildingModel data)
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
    }
}