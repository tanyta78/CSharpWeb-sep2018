namespace SIS.App
{
    using Framework;
    using Framework.Routers;
    using WebServer;

    class Launcher
    {
        static void Main(string[] args)
        {

            Server server = new Server(80, new HttpHandler());

            MvcEngine.Run(server);
        }
    }
}
