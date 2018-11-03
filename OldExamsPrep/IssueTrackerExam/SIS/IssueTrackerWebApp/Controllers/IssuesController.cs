namespace IssueTrackerWebApp.Controllers
{
    using System;
    using System.Linq;
    using Models;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using ViewModels.Issues;

    public class IssuesController : BaseController
    {
        [Authorize()]
        public IHttpResponse All()
        {
            var statusValues = Enum.GetValues(typeof(Status));

            var issues = this.Db.Issues.Select(i => new IssueViewModel()
            {
                Id = i.Id,
                Name = i.Name,
                Status = i.Status.ToString(),
                Priority = i.Priority.ToString(),
                CreationDate = i.CreationDate.ToShortDateString(),
                Author = i.User.Username,
            }).ToList();

            var model = new AllViewModel()
            {
                Issues = issues,
                StatusValues = statusValues,
            };

            return this.View(model);
        }

        [Authorize()]
        public IHttpResponse Search(IssueSearchInputModel model)
        {
            var name = model.Name;
            var statusValues = Enum.GetValues(typeof(Status));

            var resultData = this.Db.Issues
                .Where(i => i.Name.Contains(name)).ToList();

            if (model.Status != "All")
            {
                var status = (Status) Enum.Parse(typeof(Status), model.Status);
                resultData = this.Db.Issues
                    .Where(i => i.Status == status && i.Name.Contains(name)).ToList();

            }

            var searchResult = resultData
                .Select(i => new IssueViewModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Status = i.Status.ToString(),
                    Priority = i.Priority.ToString(),
                    CreationDate = i.CreationDate.ToShortDateString(),
                    Author = i.User.Username,
                }).ToList();

            var modelView = new AllViewModel()
            {
                Issues = searchResult,
                StatusValues = statusValues,
            };


            return this.View("/Issues/All", modelView);
        }

        [Authorize()]
        public IHttpResponse Add()
        {
            var statusValues = Enum.GetValues(typeof(Status));
            var priorityValues = Enum.GetValues(typeof(Priority));

            var model = new AddIssueViewModel()
            {
                StatusValues = statusValues,
                PriorityValues = priorityValues
            };
            return this.View(model);
        }

        [Authorize()]
        [HttpPost()]
        public IHttpResponse Add(IssueInputViewModel model)
        {
            var userId = this.Db.Users.First(u => u.Username == this.User.Username).Id;
            var issue = new Issue()
            {
                Name = model.Name,
                Status = Enum.Parse<Status>(model.Status),
                Priority = Enum.Parse<Priority>(model.Priority),
                CreationDate = DateTime.UtcNow,
                UserId = userId
            };

            this.Db.Issues.Add(issue);
            this.Db.SaveChanges();

            return this.Redirect("/Issues/All");
        }

        [Authorize()]
        public IHttpResponse Edit(int id)
        {
            var issue = this.Db.Issues.FirstOrDefault(i => i.Id == id);

            if (issue == null)
            {
                return this.BadRequestError("Issue do not exist");
            }

            var statusValues = Enum.GetValues(typeof(Status));
            var priorityValues = Enum.GetValues(typeof(Priority));

            var model = new EditIssueViewModel()
            {
                OptionStatusValues = statusValues,
                OptionsPriorityValues = priorityValues,
                Id = id,
                Status = issue.Status.ToString(),
                Priority = issue.Priority.ToString(),
                Name = issue.Name
            };

            return this.View(model);
        }

        [Authorize()]
        [HttpPost()]
        public IHttpResponse Edit(EditIssueViewModel model)
        {

            var issue = this.Db.Issues.FirstOrDefault(i => i.Id == model.Id);

            if (issue == null)
            {
                return this.BadRequestError("Issue do not exist");
            }

            if (issue.User.Username != this.User.Username && this.User.Role != "Admin")
            {
                return this.Redirect("/Issues/All");
            }

            issue.Name = model.Name;
            issue.Status = Enum.Parse<Status>(model.Status);
            issue.Priority = Enum.Parse<Priority>(model.Priority);

            this.Db.SaveChanges();

            return this.Redirect("/Issues/All");
        }

        [Authorize()]
        public IHttpResponse Delete(int id)
        {
            var issue = this.Db.Issues.FirstOrDefault(i => i.Id == id);

            if (issue == null)
            {
                return this.Redirect("/Issues/All");
            }

            if (issue.User.Username != this.User.Username && this.User.Role != "Admin")
            {
                return this.Redirect("/Issues/All");
            }

            this.Db.Issues.Remove(issue);
            this.Db.SaveChanges();

            return this.Redirect("/Issues/All");
        }
    }
}

