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
using WebApi.ViewModels.Clients;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Clients")]
    public class ClientsController : Controller
    {
        private readonly UserDBContext _context;

        public ClientsController(UserDBContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetClients()
        {
            // Return statuscode 204 due to lack of content.
            if (_context.Client.Count() == 0)
                return NoContent();

            // Create a list with all the requested clients.
            List<ClientViewModel> clients = new List<ClientViewModel>();
            foreach (ClientUser client in _context.Client)
            {
                // Create a client viewmodel and add it to the list with clients.
                clients.Add(new ClientViewModel
                {
                    id = client.Id,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Gender = client.Gender,
                    BirthDay = client.BirthDay,
                    Email = client.Email,
                    StreetName = client.StreetName,
                    HouseNumber = client.HouseNumber,
                    Province = client.Province,
                    District = client.District
                });
            }

            // Return statuscode 200 with the requested data.
            return Ok(clients);
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetClientUser([FromRoute] string id)
        {
            // Retreive the requested client.
            ClientUser client = await _context.Client.SingleOrDefaultAsync(m => m.Id == id);

            // Return statuscode 404 due to the client that can't be found.
            if (client == null)
                return NotFound();

            // Return statuscode 200 with the requested data.
            return Ok(client);
        }
        
        // GET: api/Clients/connected/{email}
        [HttpGet("connected/{email}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetConnectedClients([FromRoute] string email)
        {
            // Find the healthworker with the given email.
            HealthWorkerUser healthworker = _context.HealthWorker.SingleOrDefault(m => m.Email == email);

            // Return statuscode 400 due to an invalid email.
            if (healthworker == null)
                return NotFound("No healthworker with this email.");

            // Retreive a list of all clients which belong to this healthworker.
            IQueryable<ClientUser> clientsOfHealthworker = _context.Client.Where(m => m.HealthWorker_Id == healthworker.Id);

            // Create a list with all those clients.
            List<ClientViewModel> clients = new List<ClientViewModel>();
            foreach (ClientUser client in _context.Client)
            {
                // Create a client viewmodel and add it to the list with all clients.
                clients.Add(new ClientViewModel
                {
                    id = client.Id,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Gender = client.Gender,
                    BirthDay = client.BirthDay,
                    Email = client.Email,
                    StreetName = client.StreetName,
                    HouseNumber = client.HouseNumber,
                    Province = client.Province,
                    District = client.District
                });  
            }

            // Return statuscode 200 with the requested data.
            return Ok(clients);
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutClientUser([FromRoute] string id, [FromBody] ClientUser clientUser)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Return statuscode 400 due to differences in the ID.
            if (id != clientUser.Id)
                return BadRequest();

            // Try to save the changes requested changes.
            _context.Entry(clientUser).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Client.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            // Return statuscode 204 when the client has been updated.
            return NoContent();
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteClientUser([FromRoute] string id)
        {
            // Retreive the requested client.
            ClientUser client = await _context.Client.SingleOrDefaultAsync(m => m.Id == id);

            // Return statuscode 404 due to the client that can't be found.
            if (client == null)
                return NotFound();

            // Remove the client.
            _context.Client.Remove(client);
            await _context.SaveChangesAsync();

            // Return statuscode 200 when the client has been deleted.
            return Ok(client);
        }
    }
}