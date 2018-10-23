namespace IRunesWebApp
{
    using Services;
    using SIS.Framework;
    using SIS.Framework.Routers;
    using SIS.Framework.Services;
    using SIS.WebServer;

    public class Program
    {
        static void Main(string[] args)
        {
            var dependencyContainer = new DependencyContainer();

            dependencyContainer.RegisterDependency<IHashService, HashService>();
            dependencyContainer.RegisterDependency<IUserCookieService, UserCookieService>();

            var handlingContext = new HttpRouteHandlingContext(new ControllerRouter(dependencyContainer), new ResourceRouter());

            Server server = new Server(8000, handlingContext);

            MvcEngine.Run(server);
        }

    }
}
