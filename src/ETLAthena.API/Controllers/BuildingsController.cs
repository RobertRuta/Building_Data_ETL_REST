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

        public BuildingsController(IDataStorageService dataStorageService, IDataIngestionService dataIngestionService)
        {
            _dataStorageService = dataStorageService;
            _dataIngestionService = dataIngestionService;
        }

        
        [HttpGet("all-buildings")]
        public ActionResult<IEnumerable<BuildingModel>> GetAllBuildings()
        {
            var buildings = _dataStorageService.GetAllBuildings();
            return Ok(buildings);
        }

        
        [HttpPost("ingest/S1/single")]
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
        
        [HttpPost("ingest/S1/bulk")]
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

        
        [HttpPost("ingest/S2/single")]
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
        
        [HttpPost("ingest/S2/bulk")]
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
}