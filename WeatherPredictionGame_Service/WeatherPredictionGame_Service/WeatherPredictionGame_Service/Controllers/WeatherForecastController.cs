using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Models;
using Service.Services;
using WeatherPredictionGame_Service.Models;

namespace WeatherPredictionGame_Service.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IGameService _gameService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IGameService gameService)
        {
            _logger = logger;
            _gameService = gameService;
        }
        [HttpPost]
        public async Task<int> InitGame(UserParameter model)
        {
            _gameService.FillCities();
            return await _gameService.initGame(model.UserName);
        }

        [HttpGet]
        public async Task<List<CityDto>> GetCitiesByGameId(int GameId)
        {
            return await _gameService.GetCitiesByGameId(GameId);
        }

        [HttpGet]
        public async Task<int> GetUserIdByGameId(int GameId)
        {
            return await _gameService.GetUserIdByGameId(GameId);
        }
        [HttpPost]
        public void InitUserGuess(InitUserGuessParameter model)
        {
            var test = model;
            _gameService.initUserGuess(model.LstUserGuessItemDto, model.UserId, model.GameId);
        }

        [HttpPost]
        public GameResultDto CalculateResultGame(CalculateGameParameter model)
        {
            return  _gameService.CalculateResultGame(model.GameId, model.UserId);
        }

        [HttpGet]
        public List<GameRecordItem> AllGameRecords()
        {
            return _gameService.AllGameRecords();
        }
    }
}
