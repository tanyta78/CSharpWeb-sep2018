namespace TorshiaWebApp.Services.Contracts
{
    using Models;

    public interface IUserService
    {
        bool IsUserExistByUsernameAndPassword(string username, string password);

        User GetUser(string username, string password);

        User RegisterUser(string username, string password, string confirmPassword, string email);

        User GetUserByUsername(string identityUsername);
    }
}