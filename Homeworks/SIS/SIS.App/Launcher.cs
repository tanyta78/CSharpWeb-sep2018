namespace SIS.App
{
    using Framework;
    using Framework.Routers;
    using Framework.Services;
    using WebServer;

    class Launcher
    {
        static void Main(string[] args)
        {
            var dependencyContainer = new DependencyContainer();
            var handlingContext = new HttpRouteHandlingContext(
                new ControllerRouter(dependencyContainer),
                new ResourceRouter());
            Server server = new Server(80, handlingContext);

            MvcEngine.Run(server);
        }
    }
}
