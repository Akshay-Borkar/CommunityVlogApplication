using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommunityVLOG.API.Data;
using CommunityVLOG.API.Data.CommunityRepository;
using CommunityVLOG.API.Dtos;
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
        private readonly ICommunityRepository _repo;
        private readonly IMapper _map;

        public ValuesController(DataContext context, ICommunityRepository repo, IMapper map)
        {
            _context = context;
            _repo = repo;
            _map = map;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _repo.fetchUsers();
            var usersToReturn = _map.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
            // return Ok(listy);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetMember")]
        public async Task<IActionResult> GetMember(string memberId){
            int id = Convert.ToInt32(memberId);
            var user = await _repo.GetUser(id);
            var userToReturn = _map.Map<UserForDetailDto>(user);
            return Ok(userToReturn);
        }

        // [AllowAnonymous]
        // [HttpGet("{str}")]
        // public IActionResult GetValue(string str)
        // {
        //     List<string> listy = new List<string>();
        //     listy.Add("one");
        //     listy.Add("two");

        //     var strVal = 
        //     from strArr in listy
        //     where strArr.Contains(str)
        //     select strArr;

        //     return Ok(strVal);
        // }

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
