namespace User.Application.Services
{
    public interface ILoginService
    {
        public string Login(string username, string password);
    }
}