using GavinHsLogic;
using Models;
using Microsoft.AspNetCore.Mvc;

namespace PersonalGUI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private DataAccessBridge _bridge;

        public ProjectsController(DataAccessBridge bridge)
        {
            _bridge = bridge;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<ProjectModels> projects = await _bridge.GetProjectsList();

            return Ok(projects);
        }
    }
}
