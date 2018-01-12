using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts;
using WebApi.Models;
using WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebApi.ViewModels.HealthWorkers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/HealthWorkers")]
    public class HealthWorkersController : Controller
    {
        private readonly UserDBContext _context;

        public HealthWorkersController(UserDBContext context)
        {
            _context = context;
        }

        // GET: api/HealthWorkers
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetHealthWorkers()
        {
            // Return statuscode 204 due to lack of content.
            if (_context.HealthWorker.Count() == 0)
                return NoContent();

            // Create a list with all the requested healthworkers.
            List<HealthWorkerViewModel> healthWorkers = new List<HealthWorkerViewModel>();
            foreach (HealthWorkerUser healthWorker in _context.HealthWorker)
            {
                // Create an healthworker viewmodel and add it to the list with healthworkers.
                healthWorkers.Add(new HealthWorkerViewModel
                {
                    Id = healthWorker.Id,
                    Firstname = healthWorker.FirstName,
                    Lastname = healthWorker.LastName,
                    Gender = healthWorker.Gender,
                    Birthday = healthWorker.BirthDay,
                    Email = healthWorker.Email,
                    PhoneNumber = healthWorker.PhoneNumber,
                });
            }

            // Return statuscode 200 with the requested data.
            return Ok(healthWorkers); 
        }

        // GET: api/HealthWorkers/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetHealthWorkerUser([FromRoute] string id)
        {
            // Retreive the requested healthworker.
            HealthWorkerUser healthworker = await _context.HealthWorker.SingleOrDefaultAsync(m => m.Id == id);

            // Return statuscode 404 due to the healthworker that can't be found.
            if (healthworker == null)
                return NotFound();

            // Create a healthworker viewmodel
            HealthWorkerViewModel healthworkerViewModel = new HealthWorkerViewModel
            {
                Id = healthworker.Id,
                Firstname = healthworker.FirstName,
                Lastname = healthworker.LastName,
                Email = healthworker.Email,
                Birthday = healthworker.BirthDay,
                Gender = healthworker.Gender,
                PhoneNumber = healthworker.PhoneNumber
            };

            // Return statuscode 200 with the requested data.
            return Ok(healthworkerViewModel);
        }

        // PUT: api/HealthWorkers/1
        [HttpPut("edit/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutHealthworker([FromRoute] string id, [FromBody] EditHealthWorkerViewModel healthworker)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Retreive the current healthworker information.
            HealthWorkerUser currentHealthworker = _context.HealthWorker.AsNoTracking().SingleOrDefault(x => x.Id == id);

            // Creata a new healthworker model.
            HealthWorkerUser user = new HealthWorkerUser
            {
                Email = currentHealthworker.Email,
                PasswordHash = currentHealthworker.PasswordHash,
                AccessFailedCount = currentHealthworker.AccessFailedCount,
                BirthDay = healthworker.BirthDay,
                ConcurrencyStamp = currentHealthworker.ConcurrencyStamp,
                EmailConfirmed = currentHealthworker.EmailConfirmed,
                FirstName = healthworker.FirstName,
                Gender = healthworker.Gender,
                Id = currentHealthworker.Id,
                LastName = healthworker.LastName,
                LockoutEnabled = currentHealthworker.LockoutEnabled,
                LockoutEnd = currentHealthworker.LockoutEnd,
                NormalizedEmail = currentHealthworker.NormalizedEmail,
                NormalizedUserName = currentHealthworker.NormalizedUserName,
                PhoneNumber = healthworker.PhoneNumber,
                PhoneNumberConfirmed = currentHealthworker.PhoneNumberConfirmed,
                SecurityStamp = currentHealthworker.SecurityStamp,
                TwoFactorEnabled = currentHealthworker.TwoFactorEnabled,
                UserName = currentHealthworker.UserName,
            };

            // Try to save the requested changes.
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.HealthWorker.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            // Return statuscode 204 when the healthworker has been updated.
            return NoContent();
        }


        // DELETE: api/HealthWorkers/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteHealthWorkerUser([FromRoute] string id)
        {
            // Retreive the requested healthworker.
            HealthWorkerUser healthworker = await _context.HealthWorker.SingleOrDefaultAsync(m => m.Id == id);

            // Return statuscode 404 due to the healthworker that can't be found.
            if (healthworker == null)
                return NotFound();

            // Remove the healthworker.
            _context.HealthWorker.Remove(healthworker);
            await _context.SaveChangesAsync();

            // Return statuscode 200 when the healhtworker has been deleted.
            return Ok(healthworker);
        }
    }
}