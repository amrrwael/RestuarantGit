using AutoMapper;
using Delivery.Resutruant.API.Models.Domain;
using Delivery.Resutruant.API.Models.DTO;

namespace Delivery.Resutruant.API.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserProfileDto>().ReverseMap();

            CreateMap<RegisterDto, User>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) // Set UserName to Email
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
               .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
               .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
               .ReverseMap();
        }
    }
}
