namespace SIS.App
{
    using HTTP.Enums;
    using WebServer;
    using WebServer.Routing;

    class Launcher
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);

            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
