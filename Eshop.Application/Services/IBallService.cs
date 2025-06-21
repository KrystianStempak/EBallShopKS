using EShop.Domain.ModelsDto;

namespace EShop.Application.Services
{
    public interface IBallService
    {
        BallDto GetById(int id);
        IEnumerable<BallDto> GetAll();
        int Create(CreateBallDto dto);
        bool Delete(int id);
        bool Update(int id, UpdateBallDto dto);
    }
}