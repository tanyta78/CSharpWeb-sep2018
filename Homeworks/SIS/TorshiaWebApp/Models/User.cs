namespace TorshiaWebApp.Models
{
    public class User : BaseModel<int>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }

    }
}
