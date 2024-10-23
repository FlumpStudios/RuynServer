using System;
using System.Collections.Generic;
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


        [HttpGet("{id}")]

        public async Task<IActionResult> Details([FromRoute] int? id)
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] LevelData levelData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(levelData);
                await _context.SaveChangesAsync();
            }
            return Ok(levelData);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            var levelData = await _context.LevelData.FindAsync(id);
            if (levelData != null)
            {
                _context.LevelData.Remove(levelData);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool LevelDataExists(int id)
        {
            return _context.LevelData.Any(e => e.Id == id);
        }
    }
}
