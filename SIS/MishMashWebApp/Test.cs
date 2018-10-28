using System.Text;
namespace MyAppViews
{
    using System.Linq;
    using SIS.MvcFramework.ViewEngine;

    public class HomeHomeTestView : IView<MishMashWebApp.ViewModels.Home.HomeTestViewModel>
    {
        public string GetHtml(MishMashWebApp.ViewModels.Home.HomeTestViewModel model, string user)
        {
            StringBuilder html = new StringBuilder();
            var Model = model;
            var User = user;
            html.AppendLine("<div>");
            if (Model.UserRole == "Admin")
            {
                html.AppendLine("    <h1 class=\"text-center text-mishmash\">Welcome, Admin-" + Model.Username+"</h1>");
            }
            else
            {
                html.AppendLine("    <h1 class=\"text-center text-mishmash\">Welcome, " + Model.Username+"</h1>");
            }
            html.AppendLine("</div>");
            html.AppendLine("");
            html.AppendLine("<h5 class=\"text-mishmash mt-3\">Your channels</h5>");
            html.AppendLine("<hr class=\"bg-white hr-2\" />");
            html.AppendLine("<div class=\"container-fluid\">");
            html.AppendLine("    <div class=\"row justify-content-start\">");
            for (var i = 0; i < Model.YourChannels.Count(); i++)
            {
                if (i % 5 == 0 && i > 1)
                {
                    html.AppendLine("    </div>");
                    html.AppendLine("    <div class=\"row justify-content-start mt-3\">");
                }
                var item = Model.YourChannels.ToList()[i];
                html.AppendLine("        <div class=\"card border-info ml-3 bg-mishmash col-md-2 \" style=\"max-width: 18rem;\">");
                html.AppendLine("            <div class=\"card-body text-center\">");
                html.AppendLine("                <h5 class=\"card-title text-white\">" + item.Name + "</h5>");
                html.AppendLine("                <hr class=\"bg-white hr-2\" />");
                html.AppendLine("                <h5 class=\"card-text text-white\">" + item.Type + " Channel</h5>");
                html.AppendLine("                <hr class=\"bg-white hr-2\" />");
                html.AppendLine("                <h5 class=\"card-text text-white\">" + item.FollowersCount + " Following</h5>");
                html.AppendLine("                <hr class=\"bg-white hr-2\" />");
                html.AppendLine("                <h5 class=\"card-text text-white\">Following</h5>");
                html.AppendLine("            </div>");
                html.AppendLine("        </div>");
            }
            html.AppendLine("    </div>");
            html.AppendLine("</div>");
            html.AppendLine("<h5 class=\"text-mishmash mt-3\">Suggested</h5>");
            html.AppendLine("<hr class=\"bg-white hr-2\" />");
            html.AppendLine("<div class=\"container-fluid\">");
            html.AppendLine("    <div class=\"row justify-content-start\">");
            for (var i = 0; i < Model.Suggested.Count(); i++)
            {
                if (i % 5 == 0 && i > 1)
                {
                    html.AppendLine("    </div>");
                    html.AppendLine("    <div class=\"row justify-content-start mt-3\">");
                }
                var channel = Model.Suggested.ToList()[i];
                html.AppendLine("        <div class=\"card border-info ml-3 bg-mishmash col-md-2 \" style=\"max-width: 18rem;\">");
                html.AppendLine("            <div class=\"card-body text-center\">");
                html.AppendLine("                <h5 class=\"card-title text-white\">" + channel.Name + "</h5>");
                html.AppendLine("                <hr class=\"bg-white hr-2\" />");
                html.AppendLine("                <h5 class=\"card-text text-white\">" + channel.Type + " Channel</h5>");
                html.AppendLine("                <hr class=\"bg-white hr-2\" />");
                html.AppendLine("                <h5 class=\"card-text text-white\">" + channel.FollowersCount + " Following</h5>");
                html.AppendLine("                <h5 class=\"card-text text-white\">");
                html.AppendLine("                    <a href=\"/channels/follow?id=" + channel.Id + "\" class=\"text-white\">Follow</a>");
                html.AppendLine("                    <a href=\"/channels/details?id=" + channel.Id + "\" class=\"text-white\">Details</a>");
                html.AppendLine("                </h5>");
                html.AppendLine("            </div>");
                html.AppendLine("        </div>");
            }
            html.AppendLine("    </div>");
            html.AppendLine("</div>");
            html.AppendLine("<h5 class=\"text-mishmash mt-3\">See Other</h5>");
            html.AppendLine("<hr class=\"bg-white hr-2\" />");
            html.AppendLine("<div class=\"container-fluid\">");
            html.AppendLine("    <div class=\"row justify-content-start\">");
            for (var i = 0; i < Model.SeeOther.Count(); i++)
            {
                if (i % 5 == 0 && i > 1)
                {
                    html.AppendLine("    </div>");
                    html.AppendLine("    <div class=\"row justify-content-start mt-3\">");
                }
                var other = Model.SeeOther.ToList()[i];
                html.AppendLine("        <div class=\"card border-info ml-3 bg-mishmash col-md-2 \" style=\"max-width: 18rem;\">");
                html.AppendLine("            <div class=\"card-body text-center\">");
                html.AppendLine("                <h5 class=\"card-title text-white\">" + other.Name + "</h5>");
                html.AppendLine("                <hr class=\"bg-white hr-2\" />");
                html.AppendLine("                <h5 class=\"card-text text-white\">" + other.Type + " Channel</h5>");
                html.AppendLine("                <hr class=\"bg-white hr-2\" />");
                html.AppendLine("                <h5 class=\"card-text text-white\">" + other.FollowersCount + " Following</h5>");
                html.AppendLine("                <h5 class=\"card-text text-white\">");
                html.AppendLine("                    <a href=\"/channels/follow?id=" + other.Id + "\" class=\" text-white\">Follow</a>");
                html.AppendLine("                    <a href=\"/channels/details?id=" + other.Id + "\" class=\" text-white\">Details</a>");
                html.AppendLine("                </h5>");
                html.AppendLine("            </div>");
                html.AppendLine("        </div>");
            }
            html.AppendLine("    </div>");
            html.AppendLine("</div>");


            return html.ToString().TrimEnd();
        }
    }
}

