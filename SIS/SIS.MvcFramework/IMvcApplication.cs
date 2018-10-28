namespace SIS.MvcFramework
{
    using Services;

    public interface IMvcApplication
    {
        MvcFrameworkSettings Configure();

        void ConfigureServices(IServiceCollection collection);
    }
}