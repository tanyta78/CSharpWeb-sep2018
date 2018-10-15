namespace SIS.App
{
    using Framework;
    using Framework.Routers;
    using WebServer;

    class Launcher
    {
        static void Main(string[] args)
        {

            Server server = new Server(8000, new ControllerRouter());

            MvcEngine.Run(server);
        }
    }
}
