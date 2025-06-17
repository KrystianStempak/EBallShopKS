using AutoMapper;
using EBallShop.Models;
using EBallShop.ModelsDto;
using EBallShop.ModlesDto;

namespace EBallShop
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
