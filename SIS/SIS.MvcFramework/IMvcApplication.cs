namespace SIS.MvcFramework
{
    using WebServer.Routing;

    public interface IMvcApplication
    {
        void Configure(ServerRoutingTable routing);

        void ConfigureServices();
    }
}