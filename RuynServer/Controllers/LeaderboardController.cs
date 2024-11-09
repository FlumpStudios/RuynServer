using System.ComponentModel.DataAnnotations;
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
            [FromQuery] int skip,
            [FromQuery][Range(0, 50)] int take = 10)
        {
            var response = await _context.Leaderboard.OrderByDescending(x => x.Score)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Ok(response);
        }

        [ProducesResponseType<int?>(StatusCodes.Status200OK)]
        [HttpGet("{clientId}/{levelPackName}/{levelNumber}", Name = nameof(GetUserScore))]
        public async Task<IActionResult> GetUserScore([FromRoute] string clientId, [FromRoute] string levelPackName, [FromRoute] int levelNumber)
        {
            var userLeaderboard = await _context.Leaderboard.FirstOrDefaultAsync(x => x.ClientId == clientId && x.LevelPackName == levelPackName && x.LevelNumber == levelNumber);
            if (userLeaderboard == null)
            {
                return Ok(0);
            }
            return Ok(userLeaderboard.Score);
        }

        [ProducesResponseType<int?>(StatusCodes.Status200OK)]
        [HttpGet("{clientId}/{levelPackName}/{levelNumber}/rank", Name = nameof(GetRank))]
        public async Task<IActionResult> GetRank([FromRoute] string clientId, [FromRoute] string levelPackName, [FromRoute]int levelNumber)
        {
            return Ok(await CalcRank(clientId: clientId, levelPackName: levelPackName, levelNumber:levelNumber));
        }

        [ProducesResponseType<int?>(StatusCodes.Status200OK)]
        [HttpPost(Name = nameof(PostScore))]
        public async Task<IActionResult> PostScore([FromBody] Leaderboard leaderboard)
        {
            var currentScore = await _context.Leaderboard.Where(
                x => x.LevelPackName == leaderboard.LevelPackName && 
                x.ClientId == leaderboard.ClientId && 
                x.LevelNumber == x.LevelNumber).FirstOrDefaultAsync();

            if (currentScore is null)
            {
                _context.Add(leaderboard);
                
            }
            else if (currentScore.Score < leaderboard.Score)
            {
                currentScore.Score = leaderboard.Score;
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            return Ok(await CalcRank(clientId: leaderboard.ClientId, levelPackName: leaderboard.LevelPackName, levelNumber: leaderboard.LevelNumber));
        }

        private async Task<int?> CalcRank(string clientId, string levelPackName, int levelNumber)
        {
            var userScore = await _context.Leaderboard
                .Where(u => u.ClientId == clientId && u.LevelPackName == levelPackName && u.LevelNumber == u.LevelNumber)
                .Select(u => u.Score)
                .FirstOrDefaultAsync();

            if (userScore > 0)
            {
                var rank = await _context.Leaderboard
                    .CountAsync(u => u.Score > userScore) + 1;

                return rank;
            }

            return null; ;
        }
    }
}
