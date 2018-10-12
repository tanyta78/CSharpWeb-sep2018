namespace SIS.MvcFramework
{
    using WebServer;
    using WebServer.Routing;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            application.ConfigureServices();

            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            application.Configure(serverRoutingTable);
            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }
    }
}
