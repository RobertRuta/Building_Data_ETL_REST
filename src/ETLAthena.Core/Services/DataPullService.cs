namespace ETLAthena.Core.Services
{
    public class DataPullService : IDataPullService
    {
        private readonly HttpClient _httpClient;
        private readonly IDataIngestionService _dataIngestionService;
        private readonly ILogger<DataPullService> _logger;

        public DataPullService(HttpClient httpClient, IDataIngestionService dataIngestionService, ILogger<DataPullService> logger)
        {
            _httpClient = httpClient;
            _dataIngestionService = dataIngestionService;
            _logger = logger;
        }

        private async Task PullS1DataFromSourceAsync(string s1ApiEndpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(s1ApiEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    _dataIngestionService.IngestDataFromSourceS1(jsonData);
                }
                else
                {
                    _logger.LogError("Failed to pull data from source S1. Status code: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while pulling data from source S1.");
            }
        }

        private async Task PullS2DataFromSourceAsync(string s2ApiEndpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(s2ApiEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    _dataIngestionService.IngestDataFromSourceS2(jsonData);
                }
                else
                {
                    _logger.LogError("Failed to pull data from source S2. Status code: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while pulling data from source S2.");
            }
        }
    }    
}
