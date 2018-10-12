namespace CakesWebApp
{
    using Controllers;
    using SIS.HTTP.Enums;
    using SIS.MvcFramework;
    using SIS.WebServer.Routing;

    public class StartUp:IMvcApplication
    {
        public void Configure(ServerRoutingTable routing)
        {
            routing.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController{Request = request}.Index();
            routing.Routes[HttpRequestMethod.Get]["/hello"] = request => new HomeController{Request = request}.WelcomeUser();
            routing.Routes[HttpRequestMethod.Get]["/register"] = request => new AccountController{Request = request}.Register(); 
            routing.Routes[HttpRequestMethod.Post]["/register"] = request => new AccountController{Request = request}.DoRegister(); 
            routing.Routes[HttpRequestMethod.Get]["/login"] = request => new AccountController{Request = request}.Login();
            routing.Routes[HttpRequestMethod.Post]["/login"] = request => new AccountController{Request = request}.DoLogin();
            routing.Routes[HttpRequestMethod.Get]["/logout"] = request => new AccountController{Request = request}.Logout();
            routing.Routes[HttpRequestMethod.Get]["/cakes/add"] = request => new CakeController{Request = request}.AddCake();
            routing.Routes[HttpRequestMethod.Post]["/cakes/add"] = request => new CakeController{Request = request}.DoAddCake();
            routing.Routes[HttpRequestMethod.Get]["/cakes/details"] = request => new CakeController{Request = request}.Details();
        }

        public void ConfigureServices()
        {
            //TODO: Implement IoC/DI
        }
    }
}
