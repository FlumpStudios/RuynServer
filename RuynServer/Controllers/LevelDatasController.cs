using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RuynServer.Data;
using RuynServer.Models;

namespace RuynServer.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class LevelDataController(RuynServerContext context) : ControllerBase
    {
        private readonly RuynServerContext _context = context;

        [HttpGet(Name = nameof(GetLevelList))]
        [ProducesResponseType<IEnumerable<LevelListResponse>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLevelList(
            [FromQuery] string? search,
            [FromQuery] int skip,
            [FromQuery][Range(0,50)] int take = 10,
            [FromQuery] OrderByFilters orderBy = OrderByFilters.UploadedDate,
            [FromQuery] bool decending = false)
        {
            var response = _context.LevelData
                .Select(x => new LevelListResponse
                {
                    Id = x.Id,
                    LevelPackName = x.LevelPackName,
                    Author = x.Author,
                    LevelCount = x.LevelCount,
                    DownloadCount = x.DownloadCount,
                    UploadDate = x.UploadDate
                })
                .Where(x => string.IsNullOrEmpty(search) || x.LevelPackName.ToLower().Contains(search.ToLower()) || x.Author.ToLower().Contains(search.ToLower()));

            switch (orderBy)
            {
                case OrderByFilters.UploadedDate:
                    response = decending ? response.OrderByDescending(x => x.UploadDate) : response.OrderBy(x => x.UploadDate);
                    break;
                case OrderByFilters.DownloadCount:
                    response = decending ? response.OrderByDescending(x => x.DownloadCount) : response.OrderBy(x => x.DownloadCount);                    
                    break;
                case OrderByFilters.LevelCount:
                    response = decending ? response.OrderByDescending(x => x.LevelCount) : response.OrderBy(x => x.LevelCount);
                    break;
                case OrderByFilters.name:
                    response = decending ? response.OrderByDescending(x => x.LevelPackName) : response.OrderBy(x => x.LevelPackName);
                    break;
                case OrderByFilters.author:
                    response = decending ? response.OrderByDescending(x => x.Author) : response.OrderBy(x => x.Author);                    
                    break;
                default:
                    break;
            }

    
            response = response.Skip(skip).Take(take);

            var finalResponse = await response.ToListAsync();

            return Ok(finalResponse);

        }

        [HttpGet("{id}", Name = nameof(GetLevelPackById))]
        [ProducesResponseType<LevelData>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLevelPackById([FromRoute] int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            LevelData? levelData = await _context.LevelData
                    .FirstOrDefaultAsync(m => m.Id == id);

            if (levelData == null)
            {
                return NotFound();
            }

            levelData.DownloadCount++;
            _context.LevelData.Update(levelData);
            await _context.SaveChangesAsync();
            return Ok(levelData);
        }


        [HttpPost(Name = nameof(AddLevelPack))]
        public async Task<IActionResult> AddLevelPack([FromBody] LevelData levelData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(levelData);
                try
                {

                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {

                    if (e is not null && e.InnerException is not null && e.InnerException is SqliteException)
                    {
                        SqliteException ex = e.InnerException as SqliteException;
                        
                        if (ex is not null && ex.SqliteErrorCode == 19)
                        {
                            return Conflict("Name already exists");
                        }
                    }
                    throw;
                }
            }
            return Ok(levelData);
        }

        [HttpPost("{id}", Name = nameof(DeleteLevelPack))]
        public async Task<IActionResult> DeleteLevelPack([FromRoute] int id)
        {
            var levelData = await _context.LevelData.FindAsync(id);
            if (levelData != null)
            {
                _context.LevelData.Remove(levelData);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
