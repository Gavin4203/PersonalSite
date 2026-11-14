using Microsoft.AspNetCore.Mvc;
using GavinHsLogic;
using Models;
using PersonalGUI.Server.Models;
using DataAccess;
using Microsoft.AspNetCore.Authorization;

namespace PersonalGUI.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AddController : ControllerBase
    {
        private DataAccessBridge _bridge;

        public AddController(DataAccessBridge bridge)
        {
            _bridge = bridge;
        }

        //Post--------------------------------------------------------------------------------------------------------------

        [HttpPost("addP")]
        public async Task<IActionResult> AddProject([FromBody] ProjectViewModel project)
        {
            if (!Auth())
            {
                return Unauthorized();
            }

            ProjectModels model = project.ToProjModel();

            int result = await _bridge.AddProject(model);

            if (result <= 0)
            {
                return StatusCode(500, "Could not add to database");
            }

            return Ok("Successfully added to database");
        }

        [HttpPost("addJ")]
        public async Task<IActionResult> AddJob([FromBody] JobViewModel job)
        {
            if (!Auth())
            {
                return Unauthorized();
            }

            JobModel model = job.ToJobModel();

            int result = await _bridge.AddJobs(model);

            if (result <= 0)
            {
                return StatusCode(500, "Could not add to database");
            }

            return Ok("Successfully added to database");
        }

        [HttpPost("resumeUpload")]
        public async Task<IActionResult> Upload(IFormFile resUpload)
        {
            if (!Auth())
            {
                return Unauthorized();
            }

            if (resUpload == null || resUpload.Length == 0)
            {
                return BadRequest("No File Uploaded");
            }

            int result = await _bridge.UploadResumay(resUpload);

            if(result <= 0)
            {
                return StatusCode(500, "Could not upload");
            }

            return Ok("File Uploaded");
        }

        //Auth--------------------------------------------------------------------------------------------------------------
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
