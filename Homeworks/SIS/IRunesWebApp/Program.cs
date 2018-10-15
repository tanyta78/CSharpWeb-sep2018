namespace IRunesWebApp
{
    using Controllers;
    using SIS.Framework;
    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Results;
    using SIS.WebServer.Routing;

    public class Program
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            var handler = new HttpHandler(serverRoutingTable);


            //Index (guest, logged-out) (route=”/Home/Index”, route=”/”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Home/Index"] = request => new RedirectResult("/");
            //Index (user, logged-in) (route=”/Home/Index”, route=”/”)
            //Register (guest, logged-out) (route=”/Users/Register”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Register"] = request => new AccountController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Users/Register"] = request => new AccountController().DoRegister(request);
            // Login (guest, logged-out) (route=”/Users/Login”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Login"] = request => new AccountController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Users/Login"] = request => new AccountController().DoLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Logout"] = request => new AccountController().Logout(request);
            //All Albums (user, logged-in) (route=”/Albums/All”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/All"] = request => new AlbumController().All(request);
            //Album Create (user, logged-in) (route=”/Albums/Create”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/Create"] = request => new AlbumController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Albums/Create"] = request => new AlbumController().DoCreate(request);
            //Album Details (user, logged-in) (route=”/Albums/Details?id={albumId}”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/Details"] = request => new AlbumController().Details(request);
            //Track Create (user, logged-in) (route=”/Tracks/Create?albumId={albumId}”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Tracks/Create"] = request => new TrackController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Tracks/Create"] = request => new TrackController().DoCreate(request);
            //Track Details (user, logged-in) (route=”/Tracks/Details?albumId={albumId}&trackId={trackId}”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Tracks/Details"] = request => new TrackController().Details(request);

            Server server = new Server(80, handler);

            server.Run();
        }
    }
}
