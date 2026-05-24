using DAL.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> AddAsync(int id)
        {
            if(id == 1)
                throw new Exception("This is a test exception for error handling.");
            return Ok();
        }
    }
}
