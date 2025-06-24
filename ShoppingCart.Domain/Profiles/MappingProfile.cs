using AutoMapper;
using EShop.Domain.Models;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ModelsDto;

namespace ShoppingCart.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EShop.Domain.Models.Ball, ShoppingCart.Domain.Models.Ball>()
                .ForMember(dest => dest.BallId, opt => opt.MapFrom(src => src.Id)) 
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => 1))     // domyślnie 1 sztuka
                .ForMember(dest => dest.Id, opt => opt.Ignore())                    
                .ForMember(dest => dest.CartId, opt => opt.Ignore())                
                .ForMember(dest => dest.Cart, opt => opt.Ignore());

            CreateMap<Domain.Models.Ball, BallDto>()
                .ForMember(dest => dest.BallId, opt => opt.MapFrom(src => src.BallId));

            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id));

        }
    }
}
