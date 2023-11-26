using Xunit;
using ETLAthena.Core.Models;
using ETLAthena.Core.Services.Transformation;
using ETLAthena.Core.Services.Merging;
using Moq;
using ETLAthena.Core.DataStorage;
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
            var s1Data = new S1Model
            {
                Id = 430196,
                Address1 = "11 Berkeley St",
                Address2 = "London, London, W1J 8, Greater London, United Kingdom",
                FloorCount = 6,
                Lat = 51.5081594,
                Lon = -0.1428097
            };

            var s2Data = new S2Model
            {
                Id = 430196,
                Name = "Building A",
                Address = "11 Berkeley St, London, W1J 8, Greater London, United Kingdom",
                Postcode = "W1J 8",
                FloorArea = 351.9981,
                Coordinates = new[] { 51.5081594, -0.1428097}
            };

            var merger = new Merger(mockDataStorageService.Object); // Assuming this is your merging service

            // Act
            // Transform the S1 and S2 data to BuildingModel objects
            var s1Building = new S1Transformer().Transform(s1Data);
            var s2Building = new S2Transformer().Transform(s2Data);

            // Merge the data
            merger.Merge(s1Building);
            merger.Merge(s2Building);

            // Retrieve the merged data for verification
            // This assumes the merger maintains an internal store or database
            var mergedData = merger.GetMergedData(s1Building.Id);

            // Assert
            Assert.NotNull(mergedData);
            Assert.Equal(s1Data.Id, mergedData.Id);
            Assert.Equal(s2Data.Name, mergedData.Name);
            Assert.Equal(s1Data.Address1 + ", " + s1Data.Address2, mergedData.Address);
            Assert.Equal(s2Data.Postcode, mergedData.Postcode);
            Assert.Equal(s1Data.FloorCount, mergedData.FloorCount.ToString());
            Assert.Equal(s2Data.FloorArea, mergedData.FloorArea);
            Assert.Equal(s1Data.Lat, mergedData.Latitude);
            Assert.Equal(s1Data.Lon, mergedData.Longitude);
        }
    }
}
