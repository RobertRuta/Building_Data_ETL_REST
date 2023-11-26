using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace ETLAthena.Core.Services
{
    public class DataPullService : IDataPullService
    {
        private readonly HttpClient _httpClient;
        private readonly IAmazonS3 _s3Client;
        private readonly IDataIngestionService _dataIngestionService;
        private readonly ILogger<DataPullService> _logger;

        
        public DataPullService(HttpClient httpClient, IDataIngestionService dataIngestionService, ILogger<DataPullService> logger)
        {
            _httpClient = httpClient;
            _s3Client = new AmazonS3Client(Amazon.RegionEndpoint.EUCentral1);
            _dataIngestionService = dataIngestionService;
            _logger = logger;
        }

        public async Task PullS1DataFromSourceAsync(string s1ApiEndpoint)
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

        public async Task PullS2DataFromSourceAsync(string s2ApiEndpoint)
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


        public async Task PullS1DataFromS3Bucket(string bucketName, string key)
        {
            string jsonData = await DownloadFromS3(bucketName, key);
            _dataIngestionService.IngestDataFromSourceS1(jsonData);
        }

        public async Task PullS2DataFromS3Bucket(string bucketName, string key)
        {
            string jsonData = await DownloadFromS3(bucketName, key);
            _dataIngestionService.IngestDataFromSourceS2(jsonData);
        }

        private async Task<string> DownloadFromS3(string bucketName, string key)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
            using (Stream responseStream = response.ResponseStream)
            using (StreamReader reader = new StreamReader(responseStream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }    
}
