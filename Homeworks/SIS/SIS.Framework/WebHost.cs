namespace SIS.Framework
{
    using Api.Contracts;
    using Routers;
    using Services;
    using WebServer;
    using WebServer.Api;

    public static class WebHost
   {
       private const int HostingPort = 8000;

       public static void Start(IMvcApplication application)
       {
           IDependencyContainer container = new DependencyContainer();
           application.ConfigureServices(container);

           IHttpHandler controllerRouter = new ControllerRouter(container);
            application.Configure();

           Server server = new Server(HostingPort,new HttpRouteHandlingContext(controllerRouter,new ResourceRouter()));
            server.Run();
       }
   }
}
