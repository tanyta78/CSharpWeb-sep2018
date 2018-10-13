namespace SIS.MvcFramework
{
    using Services;

    public interface IMvcApplication
    {
        void Configure();

        void ConfigureServices(IServiceCollection collection);
    }
}