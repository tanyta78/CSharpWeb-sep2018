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

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/hello"] = request => new HomeController().WelcomeUser(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/register"] = request => new AccountController().Register(request); 
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/register"] = request => new AccountController().DoRegister(request); serverRoutingTable.Routes[HttpRequestMethod.Get]["/login"] = request => new AccountController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/login"] = request => new AccountController().DoLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/logout"] = request => new AccountController().Logout(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/cakes/add"] = request => new CakeController().AddCake(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/cakes/add"] = request => new CakeController().DoAddCake(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/cakes/details"] = request => new CakeController().Details(request);
          //  Server server = new Server(80, serverRoutingTable);

          //  server.Run();
        }
    }
}
