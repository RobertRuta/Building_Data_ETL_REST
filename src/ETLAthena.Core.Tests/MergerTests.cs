using Xunit;
using ETLAthena.Core.Models;
using ETLAthena.Core.Services.Transformation;
using ETLAthena.Core.Services.Merging;
using Moq;
using ETLAthena.Core.DataStorage;
using ETLAthena.Core.Services;
using Newtonsoft.Json;
// Add any other necessary using statements

namespace ETLAthena.Core.Tests
{
    public class MergerTests
    {
        [Fact]
        public void Merge_ValidS1_ValidS2_ReturnsCorrectlyMergedData()
        {
            // Arrange
            var mockDataStorageService = new Mock<IDataStorageService>();
            var mockDataIngestionService = new Mock<IDataIngestionService>();
            var s1ExistingData = new S1Model
            {
                Id = 430196,
                Address1 = "11 Berkeley St",
                Address2 = "London, London, W1J 8, Greater London, United Kingdom",
                FloorCount = 6,
                Lat = 51.5081594,
                Lon = -0.1428097
            };
            string jsonTestData = JsonConvert.SerializeObject(s1ExistingData);
            mockDataIngestionService.Object.IngestDataFromSourceS1(jsonTestData);

            var s2IncomingData = new S2Model
            {
                Id = 4301960,
                Name = "Building A",
                Address = "11 Berkeley St, London, W1J 8, Greater London, United Kingdom",
                Postcode = "W1J 8",
                FloorArea = 351.9981,
                Coordinates = new[] { 51.5081594, -0.1428097}
            };

            var merger = new Merger(mockDataStorageService.Object); // Init merger

            // Act
            // Transform the S1 and S2 data to BuildingModel objects
            var s2Building = new S2Transformer().Transform(s2IncomingData);

            // Merge the data
            merger.Merge(s2Building);
            BuildingModel newBuilding = mockDataStorageService.Object.GetBuilding(s1ExistingData.Id);

            // Assert
            Assert.Equal(newBuilding.Id, s1ExistingData.Id);
            Assert.Equal(newBuilding.Name, s2IncomingData.Name);
            Assert.Equal(newBuilding.Address, s2IncomingData.Address);
            Assert.Equal(newBuilding.Postcode, s2IncomingData.Postcode);
            Assert.Equal(newBuilding.FloorCount, s1ExistingData.FloorCount);
            Assert.Equal(newBuilding.FloorArea, s2IncomingData.FloorArea);
            Assert.Equal(newBuilding.Latitude, s1ExistingData.Lat);
            Assert.Equal(newBuilding.Longitude, s1ExistingData.Lon);
        }
    }
}
