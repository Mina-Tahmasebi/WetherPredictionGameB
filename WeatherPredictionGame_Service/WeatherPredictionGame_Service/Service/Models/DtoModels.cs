using Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public List<CityGameDto> CityGames { get; set; }
    }

    public class CityGameDto
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public int GameId { get; set; }
        public byte Order { get; set; }
        public CityDto City { set; get; }
        public GameDto Game { set; get; }
        public List<UserGuessDto> UserGuesses { get; set; }
    }

    public class GameDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<UserGameDto> UserGames { get; set; }
        public List<CityGameDto> CityGames { get; set; }
    }

    public class UserGameDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int GameId { get; set; }
        public DateTime Rdate { get; set; }
        public UserDto User { set; get; }
        public GameDto Game { set; get; }
        public List<UserGuessDto> UserGuesses { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public List<UserGameDto> UserGames { get; set; }
    }
    public class UserGuessDto
    {
        public int Id { get; set; }
        public int CityGameId { get; set; }
        public int UserGameId { get; set; }
        public byte OrderGuess { get; set; }
        public DateTime Rdate { get; set; }
        public CityGameDto CityGame { set; get; }
        public UserGameDto UserGame { set; get; }
    }
    public class UserGuessItemDto
    {
        public int CityId { get; set; }
        public int GuessOrder { get; set; }
        public string CityName { get;  set; }
    }

    public class CityOrderItemDto
    {
        public int CityId { get; set; }
        public byte Order { get; set; }
        public string CityName { get; set; }
        public int Temp { get; set; }
    }
    public class GameResultDto
    {
        public DateTime Date { set; get; }
        public int NumberOfCities { get; set; }
        public List<CityOrderItemDto> CityOrders { get; set; }
        public List<UserGuessItemDto> UserGuessCityOrders { get; set; }
        public int NumberOfTrueGuess { set; get; }

    }
    public class GameRecordItem
    {
        public DateTime Date { get; set; }
        public int NumberOfCities { get; set; }
        public int NumberOfTrueGuess { set; get; }
        public string CityNames { get; set; }
        public string UserName { get; set; }

    }
}
