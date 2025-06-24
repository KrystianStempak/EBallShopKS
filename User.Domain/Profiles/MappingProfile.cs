using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models.Requests;
using User.Domain.Models.Response;
using User.Domain.Models.Entities;

namespace User.Domain.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User.Domain.Models.Entities.User, UserResponseDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.LastLoginAt, opt => opt.MapFrom(src => src.LastLoginAt));

            CreateMap<RegisterUserDTO, User.Domain.Models.Entities.User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())        
                .ForMember(dest => dest.Role, opt => opt.Ignore())  
                .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore()); // może być null

        }
    }
}
