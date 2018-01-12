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
        public IActionResult GetBenches()
        {
            // Return statuscode 204 due to lack of content.
            if (_context.Benches.Count() == 0)
                return NoContent();

            // Return statuscode 200 with the requested data.
            return Ok(_context.Benches);
        }

        // GET: api/Benches/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetBench([FromRoute] int id)
        {
            // Retreive the requested bench.
            Bench bench = await _context.Benches.SingleOrDefaultAsync(m => m.Id == id);

            // Return statuscode 404 due to the bench that can't be found.
            if (bench == null)
                return NotFound();

            // Return statuscode 200 with the requested data.
            return Ok(bench);
        }

        // PUT: api/Benches/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutBench([FromRoute] int id, [FromBody] Bench bench)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Return statuscode 400 due to differences in the ID.
            if (id != bench.Id)
                return BadRequest();

            // Try to save the changes requested changes.
            _context.Entry(bench).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Benches.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            // Return statuscode 204 when the bench has been updated.
            return NoContent();
        }

        // POST: api/Benches
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostBench([FromBody] Bench bench)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Commit the changes to the database.
            _context.Benches.Add(bench);
            await _context.SaveChangesAsync();

            // Return the data of the newly created bench.
            return CreatedAtAction("GetBench", new { id = bench.Id }, bench);
        }

        // DELETE: api/Benches/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteBench([FromRoute] int id)
        {
            // Retreive the requested bench.
            Bench bench = await _context.Benches.SingleOrDefaultAsync(m => m.Id == id);

            // Return statuscode 404 due to the bench that can't be found.
            if (bench == null)
                return NotFound();

            // Remove the bench.
            _context.Benches.Remove(bench);
            await _context.SaveChangesAsync();

            // Return statuscode 200 when the appointment has been deleted.
            return Ok(bench);
        }
    }
}