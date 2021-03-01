using Data.Data;
using Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class GameService: IGameService
    {
        private IWeatherContext _context { get; set; }
        private readonly IMemoryCache _cache;

        public GameService(IWeatherContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task<List<CityDto>> GetCitiesByGameId(int gameId)
        {
            var lst = _context.CityGames.Where(h=>h.GameId == gameId).Include(h=>h.City).Select(h=>h.City).ToList();
            var res= lst.Select(h => h.ToDto()).ToList();
            return res;
        }
        public async Task<int> GetUserIdByGameId(int gameId)
        {
            return _context.UserGames.Where(h => h.GameId == gameId).Include(h => h.User).Select(h => h.User).FirstOrDefault().UserId;
        }
        public void initUserGuess(List<UserGuessItemDto> userGuesses, int userId, int gameId)
        {
            List<UserGuess> guesses = new List<UserGuess>();
            foreach (var item in userGuesses)
            {
                var cityGameId = _context.CityGames.Where(h => h.CityId == item.CityId && h.GameId == gameId).FirstOrDefault().CityGameId;
                var userGameId = _context.UserGames.Where(h => h.UserId == userId && h.GameId == gameId).FirstOrDefault().UserGameId;
                guesses.Add(new UserGuess
                {
                    UserGameId = userGameId,
                    CityGameId = cityGameId,
                    OrderGuess =(byte) userGuesses.Where(h=>h.CityId == item.CityId).FirstOrDefault().GuessOrder,
                    Rdate= DateTime.Now
                });
            }
            _context.UserGuesses.AddRange(guesses);
            _context.SaveChanges();
        }
        public GameResultDto CalculateResultGame(int gameId, int userId)
        {
            List<CityOrderItemDto> cityOrders = _context.Games.Include(h=>h.CityGames).Where(h=>h.GameId == gameId).FirstOrDefault().CityGames
                                                    .Select(h=>new CityOrderItemDto { 
                                                                                        CityId = h.CityId,
                                                                                        Order= h.Order,
                                                                                        Temp = h.Temp
                                                                                    }).OrderBy(h=>h.CityId).ToList();
            var guessesIds = _context.UserGames.Include(h => h.UserGuesses).Where(h => h.GameId == gameId && h.UserId == userId)
                                                            .FirstOrDefault().UserGuesses.Select(h=>h.UserGuessId).ToList();
            List<UserGuessItemDto> userGuessCityOrders = _context.UserGuesses.Where(h=> guessesIds.Contains(h.UserGuessId)).Include(h=>h.CityGame.City).Select(h => new UserGuessItemDto
                                                            {
                                                                CityId = h.CityGame.CityId,
                                                                CityName = h.CityGame.City.Name,
                                                                GuessOrder = h.OrderGuess
                                                            }).OrderBy(h => h.CityId).ToList();
            var gameDate = _context.Games.Where(h => h.GameId == gameId).FirstOrDefault().EndTime;
            
            var lstcities = _context.Cities.ToList();
            foreach (var item in cityOrders)
            {
                item.CityName = lstcities.Where(h => h.CityId == item.CityId).FirstOrDefault().Name;
            }
            int NumberOfTrueGuess = 0;
            for (int i = 0; i < cityOrders.Count; i++)
            {
                if (cityOrders[i].Order == userGuessCityOrders[i].GuessOrder)
                    NumberOfTrueGuess++;
            }
            return new GameResultDto
            {
                CityOrders = cityOrders,
                UserGuessCityOrders = userGuessCityOrders,
                Date = gameDate,
                NumberOfCities = _context.Games.Include(h => h.CityGames).Where(h => h.GameId == gameId).FirstOrDefault().CityGames.Count,
                NumberOfTrueGuess = NumberOfTrueGuess
            };
        }
        public async Task<int> initGame(string userName)
        {
            var game = new Game
            {
                StartTime = DateTime.Now
            };
            _context.Games.Add(game);

            var user = new User
            {
                DisplayName = userName
            };
            _context.Users.Add(user);
            
            _context.SaveChanges();

            var userGame = new UserGame
            {
                Rdate = DateTime.Now,
                GameId = game.GameId,
                UserId = user.UserId
            };
            _context.UserGames.Add(userGame);

            var lstCity = GetAllCitiesCatch().Take(5);
            var orderLst =await GetCityOrderFromApi(lstCity.Select(h => h.CityId).ToList());
            List<CityGame> lstCityGame = new List<CityGame>();
            foreach (var item in lstCity)
            {
                var citygame = new CityGame
                {
                    CityId = item.CityId,
                    GameId = game.GameId,
                    Order = orderLst.Where(h => h.CityId == item.CityId).FirstOrDefault().Order,
                    Temp = orderLst.Where(h => h.CityId == item.CityId).FirstOrDefault().Temp,
                };
                lstCityGame.Add(citygame);
            }
            _context.CityGames.AddRange(lstCityGame);

            _context.SaveChanges();

            return game.GameId;
        }
        async Task<List<CityOrderItemDto>> GetCityOrderFromApi(List<int> cityIds)
        {
            List<CityTempItem> lstTemps = new List<CityTempItem>();
            foreach (var item in cityIds)
            {
                var city = _context.Cities.Where(h => h.CityId == item).FirstOrDefault();
                var temp = await External_Api.WeatherApi.GetCityTemp(city.Lat, city.Long);
                lstTemps.Add(new CityTempItem { CityId = item, Temp=temp });
            }
            lstTemps = lstTemps.OrderBy(h => h.Temp).ToList();
            List<CityOrderItemDto> lstOrder = new List<CityOrderItemDto>();
            for (byte i = 1; i <= lstTemps.Count; i++)
            {
                lstOrder.Add(new CityOrderItemDto
                {
                    CityId = lstTemps[i-1].CityId,
                    Order = i,
                    Temp = lstTemps[i - 1].Temp
                });
            }
            return lstOrder;
        }

        public void FillCities()
        {
            if (!_context.Cities.Any())
            {
                _context.Cities.Add(new City
                {
                    Name = "London",
                    Lat = "51.506321",
                    Long = "-0.12714"
                });
                _context.Cities.Add(new City
                {
                    Name = "Paris",
                    Lat = "48.856930",
                    Long = "2.341200"
                });
                _context.Cities.Add(new City
                {
                    Name = "Berlin",
                    Lat = "52.516071",
                    Long = "13.376980"
                });
                _context.Cities.Add(new City
                {
                    Name = "New York",
                    Lat = "40.71455",
                    Long = "-74.007118"
                });
                _context.Cities.Add(new City
                {
                    Name = "Rome",
                    Lat = "41.903111",
                    Long = "12.495760"
                });
            }

            _context.SaveChanges();
        }
        IEnumerable<City> GetAllCitiesCatch()
        {
            List<City> LstCities = new List<City>();
            if (_cache.TryGetValue("LstCities", out LstCities) == false)
            {
                LstCities = _context.Cities.OrderBy(h => h.CityGames.Count).ToList();
                _cache.Set("LstCities", LstCities, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20) });
            }
            return LstCities;
        }
        public List<GameRecordItem> AllGameRecords()
        {
            var userGames = _context.UserGames.Include(h => h.User).Include(h => h.UserGuesses).Include(h => h.Game).ThenInclude(h => h.CityGames).ToList();
            List<GameRecordItem> lstall = new List<GameRecordItem>();
            foreach (var item in userGames)
            {
                var cityIds = item.Game.CityGames.Select(h => h.CityId).ToList();
                var cities = _context.Cities.Where(h => cityIds.Contains(h.CityId)).ToList().Select(h => h.Name).ToList();
                var citynames = string.Join(",", cities);

                List<CityOrderItemDto> cityOrders = item.Game.CityGames
                                        .Select(h => new CityOrderItemDto
                                        {
                                            CityId = h.CityId,
                                            Order = h.Order,
                                            Temp = h.Temp
                                        }).OrderBy(h => h.CityId).ToList();
                var guessesIds = item.UserGuesses.Select(h => h.UserGuessId).ToList();
                List<UserGuessItemDto> userGuessCityOrders = _context.UserGuesses.Where(h => guessesIds.Contains(h.UserGuessId)).Include(h => h.CityGame.City).Select(h => new UserGuessItemDto
                {
                    CityId = h.CityGame.CityId,
                    CityName = h.CityGame.City.Name,
                    GuessOrder = h.OrderGuess
                }).OrderBy(h => h.CityId).ToList();
                int NumberOfTrueGuess = 0;
                if(userGuessCityOrders.Any())
                    for (int i = 0; i < cityOrders.Count; i++)
                    {
                        if (cityOrders[i].Order == userGuessCityOrders[i].GuessOrder)
                            NumberOfTrueGuess++;
                    }
                GameRecordItem gameRecord = new GameRecordItem();
                gameRecord.Date = item.Game.StartTime;
                gameRecord.CityNames = citynames;
                gameRecord.NumberOfCities = cities.Count;
                gameRecord.NumberOfTrueGuess = NumberOfTrueGuess;
                gameRecord.UserName = item.User.DisplayName;

                lstall.Add(gameRecord);
            }
            return lstall;
        }

        class CityTempItem
        {
            public int CityId { get; set; }
            public int Temp { get; set; }
        }
    }

    public interface IGameService
    {
        Task<List<CityDto>> GetCitiesByGameId(int GameId);
        Task<int> GetUserIdByGameId(int gameId);
        Task<int> initGame(string userName);
        void initUserGuess(List<UserGuessItemDto> userGuesses, int userId, int gameId);
        GameResultDto CalculateResultGame(int gameId, int userId);
        List<GameRecordItem> AllGameRecords();
        void FillCities();
    }
}
