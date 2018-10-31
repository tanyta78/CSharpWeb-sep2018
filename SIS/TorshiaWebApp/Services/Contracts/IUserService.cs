namespace TorshiaWebApp.Services.Contracts
{
    using Models;

    public interface IUserService
    {
        bool IsUserExistByUsernameAndPassword(string username, string password);

        bool IsUserExistByUsername(string username);
        
        User GetUser(string username, string password);

        User RegisterUser(string username, string password, string email);

        User GetUserByUsername(string identityUsername);
    }
}