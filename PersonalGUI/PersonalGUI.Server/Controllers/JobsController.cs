using GavinHsLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace PersonalGUI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private DataAccessBridge _bridge;

        public JobsController(DataAccessBridge bridge)
        {
            _bridge = bridge;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<JobModel> jobs = await _bridge.GetJobsList();

            return Ok(jobs);
        }
    }
}
