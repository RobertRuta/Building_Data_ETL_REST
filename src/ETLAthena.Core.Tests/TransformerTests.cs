using Xunit;
using ETLAthena.Core.Models;
using ETLAthena.Core.Services.Transformation;

namespace ETLAthena.Core.Tests
{
    public class TransformerTests
    {
        [Fact]
        public void Transform_ValidS1Model_ReturnsCorrectBuildingModel()
        {
            // Arrange
            var testData = new S1Model
            {
                Id = 1234,
                Address1 = "The Shard, 32 London Bridge St",
                Address2 = "London SE1 9SG",
                Lat = -0.0865,
                Lon = 51.5045,
                FloorCount = 95
            };

            var transformer = new S1Transformer();

            // Act
            var result = transformer.Transform(testData);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Name);
            Assert.Equal(testData.Id, result.Id);
            Assert.Equal($"{testData.Address1}, {testData.Address2}", result.Address);
            Assert.Equal(testData.Lat, result.Latitude);
            Assert.Equal(testData.Lon, result.Longitude);
            Assert.Equal(testData.FloorCount, result.FloorCount);
            Assert.Null(result.FloorArea); // FloorArea should be null for S1 data
            Assert.Equal("S1", result.DataSource);
            Assert.False(result.IsMerged);
        }
        
        [Fact]
        public void Transform_ValidS2Model_ReturnsCorrectBuildingModel()
        {
            // Arrange
            var testData = new S2Model
            {
                Id = 1234,
                Name = "The Shard",
                Address = "The Shard, 32 London Bridge St, London",
                Postcode = "SE1 9SG",
                Coordinates = new[] {-0.0865, 51.5045},
                FloorArea = 2.2
            };

            var transformer = new S2Transformer();

            // Act
            var result = transformer.Transform(testData);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testData.Name, result.Name);
            Assert.Equal(testData.Id, result.Id);
            Assert.Equal(testData.Address, result.Address);
            Assert.Equal(testData.Coordinates[0], result.Latitude);
            Assert.Equal(testData.Coordinates[1], result.Longitude);
            Assert.Equal(testData.FloorArea, result.FloorArea);
            Assert.Null(result.FloorCount); // FloorArea should be null for S1 data
            Assert.Equal("S2", result.DataSource);
            Assert.False(result.IsMerged);
        }
    }
}
