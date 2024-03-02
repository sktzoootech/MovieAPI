using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        public MovieController(ILogger<MovieController> logger)
        {
            _logger = logger;            
        }

        [HttpGet]
        public async Task<ActionResult> MakeRequest()
        {
            return Ok("Hello");
        }
    }
}