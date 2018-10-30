namespace TorshiaWebApp
{
    using Services;
    using Services.Contracts;
    using SIS.Framework.Api;
    using SIS.Framework.Services;
    using SIS.Framework.Services.Contracts;

    public class StartUp : MvcApplication
    {
        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
            dependencyContainer.RegisterDependency<IHashService, HashService>();
            dependencyContainer.RegisterDependency<IUserCookieService, UserCookieService>();
            dependencyContainer.RegisterDependency<IUserService, UserService>();
            dependencyContainer.RegisterDependency<ITaskService, TaskService>();
            dependencyContainer.RegisterDependency<IReportService, ReportService>();

        }
    }
}
