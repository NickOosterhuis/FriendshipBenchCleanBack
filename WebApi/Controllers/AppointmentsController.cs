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
using System.Net.Http;
using System.Net;

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
        public IActionResult GetAppointments(string clientId, string healthworkerId)
        {
            // Filter the appointments.
            IQueryable<Appointment> filteredAppointments = _context.Appointments;
            if (!string.IsNullOrEmpty(clientId))
                filteredAppointments = filteredAppointments.Where(m => m.ClientId == clientId);
            if (!string.IsNullOrEmpty(healthworkerId))
                filteredAppointments = filteredAppointments.Where(m => m.HealthworkerId == healthworkerId);

            // Return statuscode 204 due to lack of content.
            if (filteredAppointments.Count() == 0)
                return NoContent();

            // Create a list with all the requested appointments.
            List<AppointmentGetViewModel> appointmentViewModels = new List<AppointmentGetViewModel>();
            foreach (Appointment appointment in filteredAppointments)
            {
                // Retreive all foreign data.
                ClientUser client = _userContext.Client.Find(appointment.ClientId);
                HealthWorkerUser healthworker = _userContext.HealthWorker.Find(appointment.HealthworkerId);
                Bench bench = _context.Benches.Find(appointment.BenchId);
                AppointmentStatus status = _context.AppointmentStatuses.Find(appointment.StatusId);

                // Return statuscode 500 due to corrupt data.
                if (healthworker == null || client == null || bench == null || status == null)
                    return StatusCode(500);

                // Create an appointment viewmodel and add it to the list with appointments.
                appointmentViewModels.Add(new AppointmentGetViewModel
                {
                    Id = appointment.Id,
                    Time = appointment.Time,
                    Status = status,
                    Bench = bench,
                    Client = new ClientViewModel { id = client.Id, Email = client.Email, FirstName = client.FirstName, LastName = client.LastName, BirthDay = client.BirthDay, District = client.District, Gender = client.Gender, HouseNumber = client.HouseNumber, Province = client.Province, StreetName = client.StreetName },
                    Healthworker = new HealthWorkerViewModel { Id = healthworker.Id, Firstname = healthworker.FirstName, Lastname = healthworker.LastName, Birthday = healthworker.BirthDay, Gender = healthworker.Gender, Email = healthworker.Email, PhoneNumber = healthworker.PhoneNumber }
                });             
            }

            // Return statuscode 200 with the requested data.
            return Ok(appointmentViewModels);
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAppointment([FromRoute] int id)
        {
            // Retreive the requested appointment.
            Appointment appointment = await _context.Appointments.SingleOrDefaultAsync(m => m.Id == id);

            // Return statuscode 404 due to the appointment that can't be found.
            if (appointment == null)
                return NotFound();

            // Retreive all foreign data.
            ClientUser client = _userContext.Client.Find(appointment.ClientId);
            HealthWorkerUser healthworker = _userContext.HealthWorker.Find(appointment.HealthworkerId);
            Bench bench = _context.Benches.Find(appointment.BenchId);
            AppointmentStatus status = _context.AppointmentStatuses.Find(appointment.StatusId);

            // Return statuscode 500 due to corrupt data.
            if (healthworker == null || client == null || bench == null || status == null)
                return StatusCode(500);

            // Create an appointment viewmodel.
            AppointmentGetViewModel appointmentViewModel = new AppointmentGetViewModel
            {
                Id = appointment.Id,
                Time = appointment.Time,
                Status = status,
                Bench = bench,
                Client = new ClientViewModel { id = client.Id, Email = client.Email, FirstName = client.FirstName, LastName = client.LastName, BirthDay = client.BirthDay, District = client.District, Gender = client.Gender, HouseNumber = client.HouseNumber, Province = client.Province, StreetName = client.StreetName },
                Healthworker = new HealthWorkerViewModel { Id = healthworker.Id, Firstname = healthworker.FirstName, Lastname = healthworker.LastName, Birthday = healthworker.BirthDay, Gender = healthworker.Gender, Email = healthworker.Email }
            };

            // Return statuscode 200 with the requested data.
            return Ok(appointmentViewModel);
        }

        // PUT: api/Appointments/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutAppointment([FromRoute] int id, [FromBody] Appointment appointment)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Return statuscode 400 due to differences in the ID.
            if (id != appointment.Id)
                return BadRequest();

            // Try to save the changes requested changes.
            _context.Entry(appointment).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Appointments.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            // Return statuscode 204 when the appointment has been updated.
            return NoContent();
        }

        // POST: api/Appointments
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostAppointment([FromBody] AppointmentPostViewModel appointmentViewModel)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create a new appointment.
            Appointment appointment = new Appointment()
            {
                Time = appointmentViewModel.Time,
                StatusId = 1,
                BenchId = appointmentViewModel.BenchId,
                ClientId = appointmentViewModel.ClientId,
                HealthworkerId = appointmentViewModel.HealthworkerId
            };

            // Commit the changes to the database.
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // Return the data of the newly created appointment.
            return CreatedAtAction("GetAppointment", new { id = appointment.Id }, appointment);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAppointment([FromRoute] int id)
        {
            // Retreive the requested appointment.
            Appointment appointment = await _context.Appointments.SingleOrDefaultAsync(m => m.Id == id);

            // Return statuscode 404 due to the appointment that can't be found.
            if (appointment == null)
                return NotFound();

            // Remove the appointment.
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            // Return statuscode 200 when the appointment has been deleted.
            return Ok(appointment);
        }
    }
}