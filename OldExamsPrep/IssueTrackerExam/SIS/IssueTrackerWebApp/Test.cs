
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework;
using SIS.MvcFramework.ViewEngine;
using IssueTrackerWebApp.Controllers;
namespace MyAppViews
{
    public class issues_editView : IView<IssueTrackerWebApp.Controllers.EditIssueViewModel>
    {
        public string GetHtml(IssueTrackerWebApp.Controllers.EditIssueViewModel model, MvcUserInfo user)
        {
            StringBuilder html = new StringBuilder();
            var Model = model;
            var User = user;

            html.AppendLine("<div class=\"container\">");
html.AppendLine("    <div class=\"row\">");
html.AppendLine("        <div class=\"jumbotron\">");
html.AppendLine("            <form method=\"POST\" action=\"/issues/edit?id=" + Model.Id + "\">");
html.AppendLine("                <div class=\"form-group\">");
html.AppendLine("                    <input name=\"Name\" type=\"text\" class=\"form-control\" placeholder=\"Enter issue name\" value=\"" + Model.Name + "\">");
html.AppendLine("                </div>");
html.AppendLine("                <div class=\"form-group\">");
html.AppendLine("                    <select name=\"Status\" class=\"form-control\" required>");
html.AppendLine("                        <optgroup label=\"Status\">");
html.AppendLine("                            <option value=\"\" disabled hidden>Status</option>");
                            foreach (var item in @Model.OptionStatusValues)
                            {
var status = @Model.Status;
html.AppendLine("                            <option value=\"" + item + "\"");
                                    if(item.ToString() == status.ToString())
                                    {
html.AppendLine("                                    selected");
                                    }
html.AppendLine("                                    >" + item + "</option>");
                            }
html.AppendLine("                        </optgroup>");
html.AppendLine("                    </select>");
html.AppendLine("                </div>");
html.AppendLine("                <div class=\"form-group\">");
html.AppendLine("                    <select name=\"Priority\" class=\"form-control\" required>");
html.AppendLine("                        <optgroup label=\"Priority\">");
html.AppendLine("                            <option value=\"\" disabled hidden>Priority</option>");
                            foreach (var elem in @Model.OptionsPriorityValues)
                            {
var priority = @Model.Priority;
html.AppendLine("                            <option value=\"" + elem + "\"");
                                    if(elem.ToString() == priority.ToString())
                                    {
html.AppendLine("                                    selected");
                                    }
html.AppendLine("                                    >" + elem + "</option>");
                            }
html.AppendLine("");
html.AppendLine("                        </optgroup>");
html.AppendLine("                    </select>");
html.AppendLine("                </div>");
html.AppendLine("                <div class=\"form-group\">");
html.AppendLine("                    <input class=\"btn btn-primary\" type=\"submit\" value=\"Edit\">");
html.AppendLine("                    <a href=\"javascript:window.history.back()\" class=\"btn btn-primary\">Cancel</a>");
html.AppendLine("                </div>");
html.AppendLine("            </form>");
html.AppendLine("        </div>");
html.AppendLine("    </div>");
html.AppendLine("</div>");


            return html.ToString().TrimEnd();
        }
    }
}