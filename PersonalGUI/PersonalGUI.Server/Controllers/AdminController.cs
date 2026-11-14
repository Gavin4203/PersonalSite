using GavinHsSecurity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace PersonalGUI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private PassHandler _passHandler;

        [HttpPost("pass")]
        public IActionResult Verify([FromForm] string password)
        {
            if (password == null)
            {
                return Unauthorized();
            }

            password = Sanitize(password);

            _passHandler = new PassHandler();

            if (!_passHandler.VerifyPassword(password))
            {
                return Unauthorized();
            }

            HttpContext.Session.SetString("authenticated", "true");

            return Ok();
        }

        private string Sanitize(string pass)
        {
            return Regex.Replace(pass, @"</?[^>]+(>|$)", "");

        }
    }
}
