namespace CakesWebApp
{
    using System;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Logger;
    using SIS.MvcFramework.Services;

    public class StartUp : IMvcApplication
    {
        public void Configure()
        {

        }

        public void ConfigureServices(IServiceCollection collection)
        {
            collection.AddService<IHashService, HashService>();
            collection.AddService<IUserCookieService, UserCookieService>();
            collection.AddService<ILogger, FileLogger>();
            collection.AddService<ILogger>(()=> new FileLogger($"log_{DateTime.Now.Date}.txt"));
        }
    }
}
