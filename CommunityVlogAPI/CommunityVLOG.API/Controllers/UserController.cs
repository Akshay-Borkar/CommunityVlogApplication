using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CommunityVLOG.API.Data.CommunityRepository;
using CommunityVLOG.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityVLOG.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICommunityRepository _repo;
        private readonly IMapper _map;

        public UserController(ICommunityRepository repo, IMapper map)
        {
            _repo = repo;
            _map = map;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(){
            var users = await _repo.GetUsers();
            var usersToReturn = _map.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id){
            var user = await _repo.GetUser(id);
            var userToReturn = _map.Map<UserForDetailDto>(user);
            return Ok(userToReturn);
        }
    }
}