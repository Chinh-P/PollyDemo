using System;
using System.Collections.Generic;
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
            return Ok(100);
        }
    }
} 
