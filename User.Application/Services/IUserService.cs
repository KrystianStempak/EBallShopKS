using User.Domain.Models.Requests;
using User.Domain.Models.Response;

namespace User.Application.Services
{
    public interface IUserService
    {
        UserResponseDTO GetUser(int userId);
        public void RegisterUser(RegisterUserDTO dto);
        public void ResetPassword(ResetPasswordDTO dto);
        public void EditUser(int userId, EditUserDTO dto);
    }
}