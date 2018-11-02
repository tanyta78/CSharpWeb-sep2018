namespace GameStoreWebApp.Controllers
{
    using System;
    using System.Linq;
    using Models;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using ViewModels.Games;

    public class GamesController : BaseController
    {
        public IHttpResponse All()
        {
            var allGames = this.Db.Games.Select(
                x => new GameViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    Description = x.Description,
                    Thumbnail = x.Thumbnail,
                    Size = x.Size
                }).ToList();

            var model = new IndexViewModel
            {
                Games = allGames
            };

            return this.View(model);
        }

        [Authorize()]
        public IHttpResponse Owned()
        {
            var currentUser = this.Db.Users.First(u => u.Email == this.User.Info);
            var myGames = currentUser.Games.Select(
                x => new GameViewModel
                {
                    Id = x.GameId,
                    Title = x.Game.Title,
                    Price = x.Game.Price,
                    Description = x.Game.Description,
                    Thumbnail = x.Game.Thumbnail,
                    Size = x.Game.Size
                }).ToList();

            var model = new IndexViewModel
            {
                Games = myGames
            };

            return this.View("/Games/All", model);
        }

        [Authorize()]
        public IHttpResponse Details(int id)
        {
            var game = this.Db.Games.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return this.BadRequestError("Game do not exist.");
            }

            var model = new DetailsViewModel()
            {
                Id = game.Id,
                Title = game.Title,
                Price = game.Price,
                Description = game.Description,
                Size = game.Size,
                Trailer = game.Trailer,
                ReleasedOn = game.ReleasedOn.ToString("G")
            };


            return this.View(model);
        }

        [Authorize()]
        [HttpPost()]
        public IHttpResponse Buy(int id)
        {
            var game = this.Db.Games.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return this.BadRequestError("Game do not exist.");
            }

            var currentUser = this.Db.Users.First(u => u.Email == this.User.Info);

            var isGameBought = currentUser.Games.Any(g => g.GameId == id);

            if (isGameBought)
            {
                return this.Redirect("/games/owned");
            }

            var usergame = new UserGame()
            {
                GameId = id,
                UserId = currentUser.Id
            };

            this.Db.UserGames.Add(usergame);
            this.Db.SaveChanges();

            return this.Redirect("/games/owned");

        }

        [Authorize("Admin")]
        public IHttpResponse AllGames()
        {
            var allGames = this.Db.Games.Select(
                x => new GameViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    Size = x.Size
                }).ToList();

            var model = new IndexViewModel
            {
                Games = allGames
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Add(GameInputViewModel model) => model == null ? this.View(new GameInputViewModel()) : this.View(model);

        [Authorize("Admin")]
        [HttpPost()]
        public IHttpResponse Add(GameInputViewModel model, int other)
        {
            //verification

            var game = new Game()
            {
                Description = model.Description,
                Price = decimal.Parse(model.Price),
                Title = model.Title,
                Thumbnail = model.Thumbnail,
                Trailer = model.Trailer,
                Size = int.Parse(model.Size),
                ReleasedOn = DateTime.Parse(model.ReleasedOn)
            };

            this.Db.Games.Add(game);
            this.Db.SaveChanges();

            var id = game.Id;

            return this.Redirect("/games/details?id=" + id);
        }

        [Authorize("Admin")]
        public IHttpResponse Edit(int id)
        {
            var game = this.Db.Games.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return this.BadRequestError("Game do not exist.");
            }

            var model = new EditViewModel()
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Thumbnail = game.Thumbnail,
                Price = game.Price,
                Size = game.Size,
                Trailer = game.Trailer,

            };


            return this.View(model);
        }

        [Authorize("Admin")]
        [HttpPost()]
        public IHttpResponse Edit(GameInputViewModel model)
        {
            //verification
            var game = this.Db.Games.FirstOrDefault(g => g.Id == model.Id);

            if (game == null)
            {
                return this.BadRequestError("Game do not exist.");
            }

            game.Description = model.Description;
            game.Price = decimal.Parse(model.Price);
            game.Title = model.Title;
            game.Thumbnail = model.Thumbnail;
            game.Trailer = model.Trailer;
            game.Size = int.Parse(model.Size);
           

            this.Db.SaveChanges();

            return this.Redirect("/games/details?id=" + model.Id);
        }

        [Authorize("Admin")]
        public IHttpResponse Delete(int id)
        {
            var game = this.Db.Games.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return this.BadRequestError("Game do not exist.");
            }

            var model = new DeleteViewModel()
            {
                Id = game.Id,
                Title = game.Title
            };
           

            return this.View(model);
        }

        [Authorize("Admin")]
        [HttpPost()]
        public IHttpResponse Delete(DeleteViewModel model)
        {
            var game = this.Db.Games.FirstOrDefault(g => g.Id == model.Id);

            if (game == null)
            {
                return this.Redirect("/");
            }

            this.Db.Games.Remove(game);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

    }
}
