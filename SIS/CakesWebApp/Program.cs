namespace CakesWebApp
{
    using Controllers;
    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Routing;

   public class Program
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController{Request = request}.Index();
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/hello"] = request => new HomeController{Request = request}.WelcomeUser();
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/register"] = request => new AccountController{Request = request}.Register(); 
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/register"] = request => new AccountController{Request = request}.DoRegister(); 
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/login"] = request => new AccountController{Request = request}.Login();
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/login"] = request => new AccountController{Request = request}.DoLogin();
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/logout"] = request => new AccountController{Request = request}.Logout();
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/cakes/add"] = request => new CakeController{Request = request}.AddCake();
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/cakes/add"] = request => new CakeController{Request = request}.DoAddCake();
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/cakes/details"] = request => new CakeController{Request = request}.Details();

            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }
    }
}
