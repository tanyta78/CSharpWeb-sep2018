namespace IRunesWebApp
{
    using Services;
    using Services.Contracts;
    using SIS.Framework;
    using SIS.Framework.Routers;
    using SIS.Framework.Services;
    using SIS.Framework.Services.Contracts;
    using SIS.WebServer;

    public class Program
    {
        static void Main(string[] args)
        {
            var dependencyContainer = new DependencyContainer();

            dependencyContainer.RegisterDependency<IHashService, HashService>();
            dependencyContainer.RegisterDependency<IUserCookieService, UserCookieService>();
            dependencyContainer.RegisterDependency<IUserService, UserService>();
            dependencyContainer.RegisterDependency<ITrackService, AlbumService>();
            dependencyContainer.RegisterDependency<ITrackService, TrackService>();


            var handlingContext = new HttpRouteHandlingContext(new ControllerRouter(dependencyContainer), new ResourceRouter());

            Server server = new Server(8000, handlingContext);

            MvcEngine.Run(server);
        }

    }
}
