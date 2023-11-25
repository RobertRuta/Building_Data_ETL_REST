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
                _dataProcessingService.ProcessDataFromSourceS1(data);
            }
        }

        public void IngestBulkDataFromSourceS1(string jsonData)
        {
            var datalist = JsonConvert.DeserializeObject<List<S1Model>>(jsonData);
            if (datalist != null)
            {
                foreach (var data in datalist)
                {
                    _dataProcessingService.ProcessDataFromSourceS1(data);
                }
            }
        }

        public void IngestDataFromSourceS2(string jsonData)
        {
            var data = JsonConvert.DeserializeObject<S2Model>(jsonData);
            if (data != null)
            {
                
                _dataProcessingService.ProcessDataFromSourceS2(data);
            }
        }

        public void IngestBulkDataFromSourceS2(string jsonData)
        {
            var datalist = JsonConvert.DeserializeObject<List<S2Model>>(jsonData);
            if (datalist != null)
            {
                foreach (var data in datalist)
                {
                    _dataProcessingService.ProcessDataFromSourceS2(data);
                }
            }
        }
    }
}
