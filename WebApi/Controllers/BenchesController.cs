using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts;
using WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Benches")]
    public class BenchesController : Controller
    {
        private readonly AppointmentDBContext _context;

        public BenchesController(AppointmentDBContext context)
        {
            _context = context;
        }

        // GET: api/Benches
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<Bench> GetBenches()
        {
            return _context.Benches;
        }

        // GET: api/Benches/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetBench([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bench = await _context.Benches.SingleOrDefaultAsync(m => m.Id == id);

            if (bench == null)
            {
                return NotFound();
            }

            return Ok(bench);
        }

        // PUT: api/Benches/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutBench([FromRoute] int id, [FromBody] Bench bench)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bench.Id)
            {
                return BadRequest();
            }

            _context.Entry(bench).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BenchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Benches
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostBench([FromBody] Bench bench)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Benches.Add(bench);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBench", new { id = bench.Id }, bench);
        }

        // DELETE: api/Benches/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteBench([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bench = await _context.Benches.SingleOrDefaultAsync(m => m.Id == id);
            if (bench == null)
            {
                return NotFound();
            }

            _context.Benches.Remove(bench);
            await _context.SaveChangesAsync();

            return Ok(bench);
        }

        private bool BenchExists(int id)
        {
            return _context.Benches.Any(e => e.Id == id);
        }
    }
}