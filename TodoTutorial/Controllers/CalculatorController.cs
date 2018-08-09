using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoTutorial.Web.Controllers
{
    [Route("api/calculator")]
    public class CalculatorController : Controller
    {
        [HttpGet("x/{x}/y/{y}")]
        public int Sum([FromRoute] int x, [FromRoute] int y)
        {
            return x + y;
        }
    }
}
