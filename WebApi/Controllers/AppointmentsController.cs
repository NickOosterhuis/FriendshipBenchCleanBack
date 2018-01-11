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
using WebApi.ViewModels.Appointments;
using WebApi.ViewModels.HealthWorkers;
using WebApi.ViewModels.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Appointments")]
    public class AppointmentsController : Controller
    {
        private readonly AppointmentDBContext _context;
        private readonly UserDBContext _userContext;

        public AppointmentsController(AppointmentDBContext context, UserDBContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        // GET: api/Appointments
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<AppointmentGetViewModel> GetAppointments()
        {
            List<AppointmentGetViewModel> appointments = new List<AppointmentGetViewModel>();
            foreach(Appointment appointment in _context.Appointments)
            {
                ClientUser client = _userContext.Client.Find(appointment.ClientId);
                HealthWorkerUser healthworker = _userContext.HealthWorker.Find(appointment.HealthworkerId);

                if (healthworker != null && client != null)
                {
                    appointments.Add(new AppointmentGetViewModel
                    {
                        Id = appointment.Id,
                        Time = appointment.Time,
                        Status = _context.AppointmentStatuses.Find(appointment.StatusId),
                        Bench = _context.Benches.Find(appointment.BenchId),
                        Client = new ClientViewModel { id = client.Id, Email = client.Email, FirstName = client.FirstName, LastName = client.LastName, BirthDay = client.BirthDay, District = client.District, Gender = client.Gender, HouseNumber = client.HouseNumber, Province = client.Province, StreetName = client.StreetName },
                        Healthworker = new HealthWorkerViewModel { Id = healthworker.Id, Firstname = healthworker.FirstName, Lastname = healthworker.LastName, Birthday = healthworker.BirthDay, Gender = healthworker.Gender, Email = healthworker.Email, PhoneNumber = healthworker.PhoneNumber }
                    });
                }
                else return null;               
            }

            return appointments;
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAppointment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = await _context.Appointments.SingleOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            ClientUser client = _userContext.Client.Find(appointment.ClientId);
            HealthWorkerUser healthworker = _userContext.HealthWorker.Find(appointment.HealthworkerId);
            AppointmentGetViewModel viewModel = new AppointmentGetViewModel
            {
                Id = appointment.Id,
                Time = appointment.Time,
                Status = _context.AppointmentStatuses.Find(appointment.StatusId),
                Bench = _context.Benches.Find(appointment.BenchId),
                Client = new ClientViewModel { id = client.Id, Email = client.Email, FirstName = client.FirstName, LastName = client.LastName, BirthDay = client.BirthDay, District = client.District, Gender = client.Gender, HouseNumber = client.HouseNumber, Province = client.Province, StreetName = client.StreetName },
                Healthworker = new HealthWorkerViewModel { Id = healthworker.Id, Firstname = healthworker.FirstName, Lastname = healthworker.LastName, Birthday = healthworker.BirthDay, Gender = healthworker.Gender, Email = healthworker.Email }
            };

            return Ok(viewModel);
        }

        // PUT: api/Appointments/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutAppointment([FromRoute] int id, [FromBody] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.Id)
            {
                return BadRequest();
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        // POST: api/Appointments
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostAppointment([FromBody] AppointmentPostViewModel appointmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = new Appointment()
            {
                Time = appointmentViewModel.Time,
                StatusId = 1,
                BenchId = appointmentViewModel.BenchId,
                ClientId = appointmentViewModel.ClientId,
                HealthworkerId = appointmentViewModel.HealthworkerId
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointment);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAppointment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = await _context.Appointments.SingleOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok(appointment);
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}