using Newtonsoft.Json;
using ETLAthena.Core.Models;

namespace ETLAthena.Core.Services
{
    public class DataIngestionService : IDataIngestionService
    {
        private readonly IDataProcessingService _dataProcessingService;

        public DataIngestionService(IDataProcessingService dataProcessingService)
        {
            _dataProcessingService = dataProcessingService;
        }

        public void IngestDataFromSourceS1(string jsonData)
        {
            var data = JsonConvert.DeserializeObject<S1Model>(jsonData);
            if (data != null)
            {
                // Process and validate data as needed
                _dataProcessingService.ProcessDataFromSourceS1(data);
            }
        }

        public void IngestDataFromSourceS2(string jsonData)
        {
            var data = JsonConvert.DeserializeObject<S2Model>(jsonData);
            if (data != null)
            {
                // Process and validate data as needed
                _dataProcessingService.ProcessDataFromSourceS2(data);
            }
        }
    }
}
