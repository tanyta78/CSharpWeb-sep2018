using SIS.MvcFramework;
using SIS.MvcFramework.Services;

namespace ChushkaWebApp
{
    public class Startup : IMvcApplication
    {
        public MvcFrameworkSettings Configure()
        {
            return new MvcFrameworkSettings() { PortNumber = 8000 };
        }

        public void ConfigureServices(IServiceCollection collection)
        {
        }
    }
}
