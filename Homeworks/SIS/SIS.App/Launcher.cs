namespace SIS.App
{
    using Framework;
    using Framework.Routers;
    using WebServer;

    class Launcher
    {
        static void Main(string[] args)
        {
            var handlingContext = new HttpRouteHandlingContext(
                new ControllerRouter(),
                new ResourceRouter());
            Server server = new Server(80, handlingContext);

            MvcEngine.Run(server);
        }
    }
}
