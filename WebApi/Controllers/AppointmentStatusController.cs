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
    [Route("api/AppointmentStatus")]
    public class AppointmentStatusController : Controller
    {
        private readonly AppointmentDBContext _context;

        public AppointmentStatusController(AppointmentDBContext context)
        {
            _context = context;
        }

        // GET: api/AppointmentStatus
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<AppointmentStatus> GetAppointmentStatuses()
        {
            return _context.AppointmentStatuses;
        }

        // GET: api/AppointmentStatus/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAppointmentStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointmentStatus = await _context.AppointmentStatuses.SingleOrDefaultAsync(m => m.Id == id);

            if (appointmentStatus == null)
            {
                return NotFound();
            }

            return Ok(appointmentStatus);
        }

        private bool AppointmentStatusExists(int id)
        {
            return _context.AppointmentStatuses.Any(e => e.Id == id);
        }
    }
}