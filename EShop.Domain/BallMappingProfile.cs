using AutoMapper;
using EShop.Domain.Models;
using EShop.Domain.ModelsDto;

namespace EShop.Domain
{
    public class BallMappingProfile : Profile
    {
        public BallMappingProfile() 
        {
            CreateMap<Ball, BallDto>()
                .ForMember(m => m.CategoryId, c => c.MapFrom(s => s.Category.Id))
                .ForMember(m => m.CategoryName, c => c.MapFrom(s => s.Category.Name));

            CreateMap<CreateBallDto, Ball>()
                .ForMember(r => r.Category, c => c.MapFrom(dto => new Category() { Name = dto.CategoryName }));
        
        }
    }
}
