namespace MishMashWebApp
{
    using SIS.MvcFramework;
    using SIS.MvcFramework.Services;

    public class StartUp : IMvcApplication
    {
        public MvcFrameworkSettings Configure()
        {
            return new MvcFrameworkSettings();
        }

        public void ConfigureServices(IServiceCollection collection)
        {

        }
    }
}
