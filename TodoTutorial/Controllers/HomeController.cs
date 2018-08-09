using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TodoTutorial.Web.Controllers
{
    [Route("api/test")]
    public class HomeController : Controller
    {
        [HttpGet("index")]
        public string Index()
        {
            return "Hello world from MVC!";
        }
    }
}