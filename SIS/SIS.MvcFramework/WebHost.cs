namespace SIS.MvcFramework
{
    using System.Globalization;
    using Logger;
    using Routing;
    using Services;
    using WebServer;
    using WebServer.Routing;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            var dependencyContainer = new ServiceCollection();

            dependencyContainer.AddService<IHashService, HashService>();
            dependencyContainer.AddService<IUserCookieService, UserCookieService>();
            dependencyContainer.AddService<ILogger>(() => new FileLogger($"log.txt"));

            application.ConfigureServices(dependencyContainer);
            var settings = application.Configure();

            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            var routingEngine = new RoutingEngine();
            routingEngine.RegisterRoutes(serverRoutingTable, application, settings, dependencyContainer);

           
            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
