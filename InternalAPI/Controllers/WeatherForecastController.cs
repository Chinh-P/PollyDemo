using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InternalAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {


        public WeatherForecastController( )
        {
             
        }

        //[HttpGet]
        //public  Task<IActionResult> Get(int id)
        //{
        //    return string.Empty;
        //}

       

    }



}
