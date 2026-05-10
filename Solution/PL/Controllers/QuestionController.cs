using BL.Helper;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            this._questionService = questionService;
        }

        [Authorize(Roles = RolesSelector.FieldResearcher)]
        [HttpGet]
        public async Task<IActionResult> GetQuestionsByIncidentClassType([FromQuery] int[] Ids)
        {
            var questions = await _questionService.GetQuestionByIncidentClassTypeAsync(Ids);

            return Ok(new
            {
                Success = true,
                Data = questions
            });
        }
    }
}
