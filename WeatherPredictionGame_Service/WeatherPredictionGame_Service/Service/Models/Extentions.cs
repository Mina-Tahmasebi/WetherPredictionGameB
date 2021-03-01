using Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Models
{
    public static class ExtentionMethods
    {
        public static CityDto ToDto(this City model,bool loadNP = false)
        {
            if (model == null) return null;
            return new CityDto
            {
                Id = model.CityId,
                Lat = model.Lat,
                Long = model.Long,
                Name = model.Name,
                CityGames = (loadNP? model.CityGames.Select(h=>h.ToDto()).ToList():null)
            };
        }
        public static GameDto ToDto(this Game model, bool loadNP = false)
        {
            if (model == null) return null;
            return new GameDto
            {
                Id = model.GameId,
                EndTime = model.EndTime,
                StartTime = model.StartTime,
                UserGames = (loadNP ? model.UserGames.Select(h => h.ToDto()).ToList() : null),
                CityGames = (loadNP ? model.CityGames.Select(h => h.ToDto()).ToList() : null)
            };
        }
        public static UserDto ToDto(this User model, bool loadNP = false)
        {
            if (model == null) return null;
            return new UserDto
            {
                Id = model.UserId,
                DisplayName = model.DisplayName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserGames = (loadNP ? model.UserGames.Select(h => h.ToDto()).ToList() : null)
            };
        }

        public static CityGameDto ToDto(this CityGame model, bool loadNP = false)
        {
            if (model == null) return null;
            return new CityGameDto
            {
                Id = model.CityGameId,
                City = (loadNP ? model.City.ToDto() : null),
                GameId = model.GameId,
                CityId = model.CityId,
                Order = model.Order,
                Game = (loadNP ? model.Game.ToDto() : null)
            };
        }
        public static UserGameDto ToDto(this UserGame model, bool loadNP = false)
        {
            if (model == null) return null;
            return new UserGameDto
            {
                Id = model.UserGameId,
                Rdate = model.Rdate,
                GameId = model.GameId,
                UserId = model.UserId,
                Game = (loadNP ? model.Game.ToDto() : null),
                User = (loadNP ? model.User.ToDto() : null)
            };
        }
        public static UserGuessDto ToDto(this UserGuess model, bool loadNP = false)
        {
            if (model == null) return null;
            return new UserGuessDto
            {
                Id = model.UserGuessId,
                Rdate = model.Rdate,
                CityGameId = model.CityGameId,
                UserGameId = model.CityGameId,
                OrderGuess = model.OrderGuess,
                CityGame = (loadNP ? model.CityGame.ToDto(): null),
                UserGame = (loadNP ? model.UserGame.ToDto(): null),
            };
        }
    }
}
