using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommunityVLOG.API.Data.CommunityRepository;
using CommunityVLOG.API.Dtos;
using CommunityVLOG.API.Helpers;
using CommunityVLOG.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CommunityVLOG.API.Controllers
{
    [Authorize]
    [Route("api/user/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly ICommunityRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfigs;
        private Cloudinary _cloudinary;

        public PhotosController(ICommunityRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfigs)
        {
            _cloudinaryConfigs = cloudinaryConfigs;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
                _cloudinaryConfigs.Value.ApiKey,
                _cloudinaryConfigs.Value.ApiSecret,
                _cloudinaryConfigs.Value.CloudName
            );
            
            _cloudinary = new Cloudinary();

        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id){
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, PhotoForCreationDto photoForCreationDto){
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if(file.Length > 0){
                
                using(var stream = file.OpenReadStream()){
                    var uploadParams = new ImageUploadParams(){
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if(!userFromRepo.Photos.Any(x => x.IsMainPhots))
                photo.IsMainPhots = true;

            userFromRepo.Photos.Add(photo);

            if(await _repo.SaveAll()){
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new {id = photo.Id}, photoToReturn);
            }

            return BadRequest("Could not add the photo.");


        }
    }
}