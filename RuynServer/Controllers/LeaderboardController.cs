using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuynServer.Data;
using RuynServer.Models;

namespace RuynServer.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class LeaderboardController(RuynServerContext context) : ControllerBase
    {
        private readonly RuynServerContext _context = context;

        [ProducesResponseType<IEnumerable<Leaderboard>>(StatusCodes.Status200OK)]
        [HttpGet(Name = nameof(GetLeaderboards))]
        public async Task<IActionResult> GetLeaderboards(
            [FromQuery] string levelPack,
            [FromQuery] int levelNumber,
            [FromQuery] int skip,
            [FromQuery][Range(0, 50)] int take = 10)
        {
            List<Leaderboard> response = await GetHighScoreQuery(levelPack, levelNumber, skip, take);

            return Ok(response);
        }

        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [HttpGet("asstring",Name = nameof(GetLeaderboardsString))]
        public async Task<IActionResult> GetLeaderboardsString(
            [FromQuery] string levelPack,
            [FromQuery] int levelNumber,
            [FromQuery] int skip,
            [FromQuery] string? clientId = null,
            [FromQuery][Range(0, 50)] int take = 10)
        {
            List<Leaderboard> response = await GetHighScoreQuery(levelPack, levelNumber, skip, take);

            int i = skip + 1;
            StringBuilder stringBuilder = new();

            stringBuilder.Append($"{"Rank",-5} {"Username",-15} {"Score",10}");

            foreach (var entry in response)
            {
                if (entry.UserName is null)
                {
                    entry.UserName = "Anonymous";
                }
                string username = entry.UserName.Length > 12
                ? entry.UserName.Substring(0, 12) + "..."
                : entry.UserName;

                stringBuilder.Append($"{i,-5} {username,-15} {entry.Score,10}");
                i++;
            }

            if (clientId is not null)
            {
                stringBuilder.Append($"{string.Empty,-5} {string.Empty,-15} {string.Empty,10}");
                stringBuilder.Append($"{string.Empty,-5} {string.Empty,-15} {string.Empty,10}");
                stringBuilder.Append($"{"Your Rank",-9} {string.Empty,-11} {string.Empty,10}");

                string? rankString = await GetRankString(clientId: clientId, levelPackName: levelPack, levelNumber: levelNumber);
                if (rankString is not null)
                {
                    stringBuilder.Append(rankString);
                }
            }

            return Ok(stringBuilder.ToString());
        }

        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [HttpGet("{levelPackName}/{levelNumber}/high", Name = nameof(GetHighScore))]
        public async Task<IActionResult> GetHighScore([FromRoute] string levelPackName, [FromRoute] int levelNumber)
        {
            var highScore = await _context.Leaderboard.Where(x =>
            x.LevelPackName == levelPackName &&
            x.LevelNumber == levelNumber)
                .OrderByDescending(x => x.Score).FirstOrDefaultAsync();

            if (highScore == null)
            {
                return Ok();
            }

            string response = $"{highScore.UserName} {highScore.Score}";
            return Ok(response);
        }

        [ProducesResponseType<int?>(StatusCodes.Status200OK)]
        [HttpGet("{clientId}/{levelPackName}/{levelNumber}", Name = nameof(GetUserScore))]
        public async Task<IActionResult> GetUserScore([FromRoute] string clientId, [FromRoute] string levelPackName, [FromRoute] int levelNumber)
        {
            int? score = await _context.Leaderboard
                .Where(x => x.ClientId == clientId && x.LevelPackName == levelPackName && x.LevelNumber == levelNumber).Select(x => x.Score).FirstOrDefaultAsync();
            
            return Ok(score ?? 0);
        }
        
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [HttpGet("{clientId}/{levelPackName}/{levelNumber}/rankasstring", Name = nameof(GetRankAsString))]
        public async Task<IActionResult> GetRankAsString([FromRoute] string clientId, [FromRoute] string levelPackName, [FromRoute] int levelNumber)
        {
            var userScore = await _context.Leaderboard
                .Where(u => u.ClientId == clientId && u.LevelPackName == levelPackName && u.LevelNumber == u.LevelNumber)
                .FirstOrDefaultAsync();

            if (userScore is not null)
            {
                var rank = await _context.Leaderboard
                    .CountAsync(u => u.Score > userScore.Score) + 1;

                StringBuilder stringBuilder = new();
                
                if (userScore.UserName is null)
                {
                    userScore.UserName = "Anonymous";
                }

                string username = userScore.UserName.Length > 12
                ? string.Concat(userScore.UserName.AsSpan(0, 12), "...")
                : userScore.UserName;

                stringBuilder.AppendLine($"{rank,-5} {username,-15} {userScore.Score,10}");

                return Ok(stringBuilder.ToString());
            }

            return Ok(null);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost(Name = nameof(PostScore))]
        public async Task<IActionResult> PostScore([FromBody] Leaderboard leaderboard)
        {
            var currentScore = await _context.Leaderboard.Where(
                x => x.LevelPackName == leaderboard.LevelPackName && 
                x.ClientId == leaderboard.ClientId && 
                x.LevelNumber == leaderboard.LevelNumber).FirstOrDefaultAsync();

            if (currentScore is null)
            {
                _context.Add(leaderboard);
                await _context.SaveChangesAsync();
                
            }
            else if (currentScore.Score <leaderboard.Score)
            {
                currentScore.Score = leaderboard.Score;
                _context.Leaderboard.Update(currentScore);
                await _context.SaveChangesAsync();
            }            
            return Ok();
        }

        private async Task<int?> CalcRank(int score, string levelPackName, int levelNumber)
        {
            var rank = (await _context.Leaderboard.CountAsync(u => u.LevelPackName == levelPackName && u.LevelNumber == levelNumber && u.Score > score)) + 1;                
            return rank;
        }

        private async Task<string?> GetRankString(string clientId, string levelPackName, int levelNumber)
        {
            var userScore = await _context.Leaderboard
                   .Where(u => u.ClientId == clientId && u.LevelPackName == levelPackName && u.LevelNumber == levelNumber)
                   .FirstOrDefaultAsync();

            if (userScore is not null)
            {
                var rank = await CalcRank(score: userScore.Score, levelPackName: levelPackName, levelNumber: levelNumber);

                StringBuilder stringBuilder = new();

                if (userScore.UserName is null)
                {
                    userScore.UserName = "Anonymous";
                }

                string username = userScore.UserName.Length > 12
                ? string.Concat(userScore.UserName.AsSpan(0, 12), "...")
                : userScore.UserName;

                stringBuilder.Append($"{rank,-5} {username,-15} {userScore.Score,10}");

                return stringBuilder.ToString();
            }

            return null;
        }

        private async Task<List<Leaderboard>> GetHighScoreQuery(string levelPack, int levelNumber, int skip, int take)
        {
            return await _context.Leaderboard.Where(x => x.LevelPackName == levelPack && x.LevelNumber == levelNumber)
                            .OrderByDescending(x => x.Score)
                            .Skip(skip)
                            .Take(take)
                            .ToListAsync();
        }
    }
}
