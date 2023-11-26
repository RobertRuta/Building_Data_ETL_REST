using Xunit;
using ETLAthena.Core.Models;
using ETLAthena.Core.Services;
using Moq;
using Newtonsoft.Json;

namespace ETLAthena.Core.Tests
{
    public class DataIngestionServiceTests
    {
        [Fact]
        public void IngestDataFromSourceS1_ValidData_ShouldCallProcessData()
        {
            // Arrange
            var mockDataProcessingService = new Mock<IDataProcessingService>();
            var dataIngestionService = new DataIngestionService(mockDataProcessingService.Object);
            var testData = new S1Model
            {
                Id = 1234,
                Address1 = "The Shard, 32 London Bridge St",
                Address2 = "London SE1 9SG",
                Lat = -0.0865,
                Lon = 51.5045,
                FloorCount = 0
            };

            string jsonTestData = JsonConvert.SerializeObject(testData);
            // Act
            var result = dataIngestionService.IngestDataFromSourceS1(jsonTestData);

            // Assert
            mockDataProcessingService.Verify(m => m.ProcessDataFromSourceS1(It.IsAny<S1Model>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(testData.Id, result.Id);
            Assert.Equal(testData.Address1, result.Address1);
            Assert.Equal(testData.Address2, result.Address2);
            Assert.Equal(testData.Lat, result.Lat);
            Assert.Equal(testData.Lon, result.Lon);
            Assert.Equal(testData.FloorCount, result.FloorCount);
        }

        [Fact]
        public void IngestDataFromSourceS2_ValidData_ShouldCallProcessData()
        {
            // Arrange
            var mockDataProcessingService = new Mock<IDataProcessingService>();
            var dataIngestionService = new DataIngestionService(mockDataProcessingService.Object);
            var testData = new S2Model
            {
                Id = 1234,
                Name = "The Shard",
                Address = "The Shard, 32 London Bridge St, London",
                Postcode = "SE1 9SG",
                Coordinates = new[] {-0.0865, 51.5045},
                FloorArea = 2.2
            };

            string jsonTestData = JsonConvert.SerializeObject(testData);
            // Act
            var result = dataIngestionService.IngestDataFromSourceS2(jsonTestData);

            // Assert
            mockDataProcessingService.Verify(m => m.ProcessDataFromSourceS2(It.IsAny<S2Model>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(testData.Id, result.Id);
            Assert.Equal(testData.Name, result.Name);
            Assert.Equal(testData.Address, result.Address);
            Assert.Equal(testData.Postcode, result.Postcode);
            Assert.Equal(testData.Coordinates, result.Coordinates);
            Assert.Equal(testData.FloorArea, result.FloorArea);
        }
    }
}