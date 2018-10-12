namespace SIS.MvcFramework
{
    using WebServer.Routing;

    public interface IMvcApplication
    {
        void Configure();

        void ConfigureServices();
    }
}