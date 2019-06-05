using System.Linq;
using AutoMapper;
using CommunityVLOG.API.Dtos;
using CommunityVLOG.API.Models;

namespace CommunityVLOG.API.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMainPhots).Url
                ))
                .ForMember(dest => dest.Age, opt =>
                    opt.ResolveUsing(d => d.DateOfBirth.CalculateAge()
                ));
            CreateMap<User, UserForDetailDto>()
            .ForMember(dest => dest.PhotoUrl, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMainPhots).Url
                ))
                .ForMember(dest => dest.Age, opt =>
                    opt.ResolveUsing(d => d.DateOfBirth.CalculateAge()
                ));
            CreateMap<Photo, PhotosForDetailDto>();
        }
    }
}