using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityVLOG.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityVLOG.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;
        public ValuesController(DataContext context)
        {
            _context = context;            
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var value = _context.Values.ToList();
            return Ok(value);
        }

        [AllowAnonymous]
        [HttpGet("{str}")]
        public IActionResult GetValue(string str)
        {
            List<string> listy = new List<string>();
            listy.Add("one");
            listy.Add("two");

            var strVal = 
            from strArr in listy
            where strArr.Contains(str)
            select strArr;

            return Ok(strVal);
        }

        // GET api/values
        // [HttpGet]
        // public IActionResult GetDetails()
        // {
        //     var value = _context.Values.ToList();
        //     return Ok(value);
        // }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
