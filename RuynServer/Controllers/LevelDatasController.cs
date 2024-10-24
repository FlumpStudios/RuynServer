using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RuynServer.Data;
using RuynServer.Models;

namespace RuynServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LevelDataController : ControllerBase
    {
        private readonly RuynServerContext _context;

        public LevelDataController(RuynServerContext context)
        {
            _context = context;
        }


        [HttpGet(Name = nameof(GetLevelList))]
        public async Task<IActionResult> GetLevelList(
            [FromQuery] OrderByFilters orderBy,
            [FromQuery] string? nameSearch,
            [FromQuery] string? authorSeach,
            [FromQuery] int skip,
            [FromQuery][MaxLength(50)] int take = 10)
        {
            var response = _context.LevelData.
                Where(x => nameSearch == null || x.LevelPackName.Contains(nameSearch)).
                Where(x => authorSeach == null || x.LevelPackName.Contains(authorSeach));

            switch (orderBy)
            {
                case OrderByFilters.UploadedDate:
                    response.OrderBy(x => x.UploadDate);
                    break;
                case OrderByFilters.DownloadCount:
                    response.OrderBy(x => x.DownloadCount);
                    break;
                case OrderByFilters.LevelCount:
                    response.OrderBy(x => x.LevelCount);
                    break;
                case OrderByFilters.name:
                    response.OrderBy(x => x.LevelPackName);
                    break;
                case OrderByFilters.author:
                    response.OrderBy(x => x.Author);
                    break;
                default:
                    break;
            }

            response.Skip(skip).Take(take);

            return Ok(response.ToList());

        }

        [HttpGet("{id}", Name = nameof(GetLevelPackById))]
        public async Task<IActionResult> GetLevelPackById([FromRoute] int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var levelData = await _context.LevelData
                .FirstOrDefaultAsync(m => m.Id == id);

            if (levelData == null)
            {
                return NotFound();
            }

            return Ok(levelData);
        }

        
        [HttpPost(Name = nameof(AddLevelPack))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLevelPack([FromBody] LevelData levelData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(levelData);
                await _context.SaveChangesAsync();
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
