using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CommunityVLOG.API.Data.CommunityRepository;
using CommunityVLOG.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CommunityVLOG.API.Helpers;
using CommunityVLOG.API.Models;

namespace CommunityVLOG.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
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
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams){

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;
            if(string.IsNullOrEmpty(userParams.Gender)){
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }
            
            var users = await _repo.GetUsers(userParams);
            var usersToReturn = _map.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id){
            var user = await _repo.GetUser(id);
            var userToReturn = _map.Map<UserForDetailDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto){
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            _map.Map(userForUpdateDto, userFromRepo);
            
            if(await _repo.SaveAll())
                return NoContent();

            throw new  KeyNotFoundException($"Updating user {id} failed on save");
        }




        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId){
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var like = await _repo.GetLike(id, recipientId);

            if(like != null)
                return BadRequest("You already liked this user");
                
            if(await _repo.GetUser(recipientId) == null)
                return NotFound();

            like = new Like{
                LikerId = id,
                LikeeId = recipientId
            };

            _repo.Add<Like>(like);
            if(await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to like user.");
        }
    }
}