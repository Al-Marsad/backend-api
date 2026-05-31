using System;
using BL.Queue.Interfaces;
using DAL.DBContext;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IIncidentClassificationQueue _queue;
        public TestController(
                IIncidentClassificationQueue queue)
        {
            _queue = queue;
        }
        
        
        [HttpGet("{id}")]
        public async Task<IActionResult> AddAsync(int id)
        {
            _queue.Enqueue(id.ToString());

            return Ok(new { 
                message = $"Enqueued incident ID: {id}"
            });
        }
    }
}
