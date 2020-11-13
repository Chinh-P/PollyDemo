using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ThirdPartyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotStableAmazonInventoryController : ControllerBase
    {
        static int _requestCount = 0;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            await Task.Delay(100); 
            _requestCount++;
            var currentSecond = DateTime.Now.Second;
            Debug.WriteLine(DateTime.Now);
            if (currentSecond % 4 == 0 )  // ok on first 10 second
            {
                return Ok(100);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong");
        }

    }
} 
