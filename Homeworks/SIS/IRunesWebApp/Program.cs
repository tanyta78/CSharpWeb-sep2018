namespace IRunesWebApp
{
    using Controllers;
    using SIS.Framework.Routers;
    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Results;
    using SIS.WebServer.Routing;

    public class Program
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            var handlingContext = new HttpRouteHandlingContext(new ControllerRouter(), new ResourceRouter());


            ConfigureRouting(serverRoutingTable);

            Server server = new Server(80, handlingContext);

            server.Run();
        }

        private static void ConfigureRouting(ServerRoutingTable serverRoutingTable)
        {
            //Index (guest, logged-out) (route=”/Home/Index”, route=”/”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Home/Index"] = request => new RedirectResult("/");
            //Index (user, logged-in) (route=”/Home/Index”, route=”/”)
            //Register (guest, logged-out) (route=”/Users/Register”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Register"] = request => new UsersController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Users/Register"] = request => new UsersController().DoRegister(request);
            // Login (guest, logged-out) (route=”/Users/Login”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Login"] = request => new UsersController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Users/Login"] = request => new UsersController().DoLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Logout"] = request => new UsersController().Logout(request);
            //All Albums (user, logged-in) (route=”/Albums/All”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/All"] = request => new AlbumsController().All(request);
            //Album Create (user, logged-in) (route=”/Albums/Create”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/Create"] = request => new AlbumsController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Albums/Create"] = request => new AlbumsController().DoCreate(request);
            //Album Details (user, logged-in) (route=”/Albums/Details?id={albumId}”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/Details"] = request => new AlbumsController().Details(request);
            //Track Create (user, logged-in) (route=”/Tracks/Create?albumId={albumId}”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Tracks/Create"] = request => new TracksController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Tracks/Create"] = request => new TracksController().DoCreate(request);
            //Track Details (user, logged-in) (route=”/Tracks/Details?albumId={albumId}&trackId={trackId}”)
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Tracks/Details"] = request => new TracksController().Details(request);
        }
    }
}
