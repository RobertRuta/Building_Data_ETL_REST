using Microsoft.AspNetCore.Mvc;
using ETLAthena.Core.Models;

namespace ETLAthena.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildingsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<BuildingModel>> GetBuildings()
        {
            var buildings = new List<BuildingModel>
            {
                new BuildingModel { Id = 1, Name = "Building 1", Address = "123 Main St" },
                new BuildingModel { Id = 2, Name = "Building 2", Address = "456 Elm St" }
            };

            return Ok(buildings);
        }
    }
}