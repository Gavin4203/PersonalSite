using Microsoft.AspNetCore.Mvc;
using GavinHsLogic;
using Models;
using PersonalGUI.Server.Models;
using Microsoft.AspNetCore.Authorization;

namespace PersonalGUI.Server.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class EditController : ControllerBase
    {
        private DataAccessBridge _bridge;

        public EditController(DataAccessBridge bridge)
        {
            _bridge = bridge;
        }

        //Get---------------------------------------------------------------------------------------------------------------

        [HttpGet("getP")]
        public async Task<IActionResult> GetProjectData()
        {
            try
            {
                if (!Auth())
                {
                    return Unauthorized();
                }

                List<ProjectModels> projects = await _bridge.GetProjectsList();

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getJ")]
        public async Task<IActionResult> GetJobData()
        {
            if (!Auth())
            {
                return Unauthorized();
            }

            List<JobModel> jobs = await _bridge.GetJobsList();

            return Ok(jobs);
        }

        //Update---------------------------------------------------------------------------------------------------------------

        [HttpPost("updateP")]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectViewModel project)
        {
            if (!Auth())
            {
                return Unauthorized();
            }

            ProjectModels model = project.ToProjModel();

            //sanitize

            int result = await _bridge.UpdateProject(model);

            if(result <= 0)
            {
                return StatusCode(500, "Failed to update database");
            }

            return Ok(model);
        }

        [HttpPost("updateJ")]
        public async Task<IActionResult> UpdateJob([FromBody] JobViewModel job)
        {
            if (!Auth())
            {
                return Unauthorized();
            }

            JobModel model = job.ToJobModel();

            //sanitize

            int result = await _bridge.UpdateJobs(model);

            if (result <= 0)
            {
                return StatusCode(500, "Failed to update database");
            }

            return Ok(model);
        }

        //Delete---------------------------------------------------------------------------------------------------------------

        [HttpPost("deleteP")]
        public async Task<IActionResult> DeleteProj([FromBody] int id)
        {
            if (!Auth())
            {
                return Unauthorized();
            }

            int result = await _bridge.DeleteProject(id);

            return Ok(id);
        }

        [HttpPost("deleteJ")]
        public async Task<IActionResult> DeleteJob([FromBody] int id)
        {
            if (!Auth())
            {
                return Unauthorized();
            }

            int result = await _bridge.DeleteJobs(id);

            return Ok(id);
        }

        //Auth----------------------------------------------------------------------------------------------------------------
        private bool Auth()
        {
            var sesh = HttpContext.Session.GetString("authenticated");
            if (sesh != "true")
            {
                return false;
            }

            return true;
        }
    }
}
