using GavinHsLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace PersonalGUI.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ResumayController : Controller
    {
        private DataAccessBridge _bridge;

        public ResumayController(DataAccessBridge bridge)
        {
            _bridge = bridge;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ResumayModel resumay = await _bridge.ReturnResumay();

            return File(resumay.ResumayPDF,resumay.ResumayMimeType);
        }
    }
}
