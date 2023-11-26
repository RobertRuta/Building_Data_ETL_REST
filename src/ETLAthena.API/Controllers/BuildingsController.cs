using Microsoft.AspNetCore.Mvc;
using ETLAthena.Core.Models;
using ETLAthena.Core.Services;
using ETLAthena.Core.DataStorage;
using System.Text.Json;

namespace ETLAthena.API.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class BuildingsController : ControllerBase
    {
        private readonly IDataStorageService _dataStorageService;
        private readonly IDataIngestionService _dataIngestionService;
        private readonly IDataPullService _dataPullService;

        public BuildingsController(IDataStorageService dataStorageService, IDataIngestionService dataIngestionService, IDataPullService dataPullService)
        {
            _dataStorageService = dataStorageService;
            _dataIngestionService = dataIngestionService;
            _dataPullService = dataPullService;
        }

        
        [HttpGet("all-buildings")]
        public ActionResult<IEnumerable<BuildingModel>> GetAllBuildings()
        {
            var buildings = _dataStorageService.GetAllBuildings();
            return Ok(buildings);
        }

        
        [HttpPost("ingest/push/S1/single")]
        public IActionResult IngestS1Single([FromBody] JsonElement jsonData)
        {
            try
            {
                _dataIngestionService.IngestDataFromSourceS1(jsonData.ToString()); // Assuming a generic ingestion method
                return Ok();
            }
            catch (Exception ex)
            {
                // Log exception and return appropriate error response
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("ingest/push/S1/bulk")]
        public IActionResult IngestS1Bulk([FromBody] JsonElement jsonDataList)
        {
            try
            {
                _dataIngestionService.IngestBulkDataFromSourceS1(jsonDataList.ToString()); // Ingest each record
                return Ok();
            }
            catch (Exception ex)
            {
                // Log exception and return appropriate error response
                return BadRequest(ex.Message);
            }
        }
        
        // Endpoint to manually trigger the data pull with a specified URL
        [HttpPost("ingest/pull/S1")]
        public async Task<IActionResult> PullS1Data([FromBody] DataPullRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ApiUrl))
            {
                return BadRequest("API URL is required.");
            }

            try
            {
                await _dataPullService.PullS1DataFromSourceAsync(request.ApiUrl);
                return Ok($"Data pull initiated successfully for {request.ApiUrl}.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error initiating data pull for {request.ApiUrl}: {ex.Message}");
            }
        }
        
        // Endpoint to manually trigger the data pull with a specified URL
        [HttpPost("ingest/pull/S2")]
        public async Task<IActionResult> PullS2Data([FromBody] DataPullRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ApiUrl))
            {
                return BadRequest("API URL is required.");
            }

            try
            {
                await _dataPullService.PullS2DataFromSourceAsync(request.ApiUrl);
                return Ok($"Data pull initiated successfully for {request.ApiUrl}.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error initiating data pull for {request.ApiUrl}: {ex.Message}");
            }
        }

        
        [HttpPost("ingest/push/S2/single")]
        public IActionResult IngestS2Single([FromBody] JsonElement jsonData)
        {
            try
            {
                _dataIngestionService.IngestDataFromSourceS2(jsonData.ToString()); // Assuming a generic ingestion method
                return Ok();
            }
            catch (Exception ex)
            {
                // Log exception and return appropriate error response
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("ingest/push/S2/bulk")]
        public IActionResult IngestS2Bulk([FromBody] JsonElement jsonDataList)
        {
            try
            {
                _dataIngestionService.IngestBulkDataFromSourceS2(jsonDataList.ToString()); // Ingest each record
                return Ok();
            }
            catch (Exception ex)
            {
                // Log exception and return appropriate error response
                return BadRequest(ex.Message);
            }
        }
    }

    public class DataPullRequest
    {
        public string ApiUrl { get; set; }
    }
}