using Microsoft.AspNetCore.Mvc;
using ETLAthena.Core.Models;
using ETLAthena.Core.Services;
using ETLAthena.Core.DataStorage;

namespace ETLAthena.API.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class BuildingsController : ControllerBase
    {
        private readonly IDataStorageService _dataStorageService;

        public BuildingsController(IDataStorageService dataStorageService)
        {
            _dataStorageService = dataStorageService;
        }


        [HttpGet("test-buildings")]
        public ActionResult<IEnumerable<BuildingModel>> GetBuildings()
        {
            var buildings = new List<BuildingModel>
            {
                new BuildingModel { Id = 1, Name = "Building 1", Address = "123 Main St" },
                new BuildingModel { Id = 2, Name = "Building 2", Address = "456 Elm St" }
            };

            return Ok(buildings);
        }
        
        
        [HttpGet("all-buildings")]
        public ActionResult<IEnumerable<BuildingModel>> GetAllBuildings()
        {
            var buildings = _dataStorageService.GetAllBuildings();
            return Ok(buildings);
        }
    }
}