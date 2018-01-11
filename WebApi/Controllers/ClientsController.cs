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

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<ClientViewModel> GetClients()
        {
            List<ClientViewModel> clients = new List<ClientViewModel>();
            foreach (ClientUser client in _context.Client)
            {
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
            return clients;
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetClientUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clientUser = await _context.Client.SingleOrDefaultAsync(m => m.Id == id);

            if (clientUser == null)
            {
                return NotFound();
            }

            return Ok(clientUser);
        }
        

        //api/clients/connected/{email}
        [HttpGet("connected/{email}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<ClientViewModel> GetConnectedClients([FromRoute] string email)
        {

            var userId = "";

            foreach (HealthWorkerUser hw_u in _context.HealthWorker)
            {
                if (hw_u.Email == email)
                {
                    userId = hw_u.Id;
                }
            }



            if (userId == "")
            {
                return null;
            }

            List<ClientViewModel> clients = new List<ClientViewModel>();
            foreach (ClientUser client in _context.Client)
            {
                if (client.HealthWorker_Id == userId)
                {
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
            }
            return clients;
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutClientUser([FromRoute] string id, [FromBody] ClientUser clientUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clientUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(clientUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientUserExists(id))
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

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteClientUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clientUser = await _context.Client.SingleOrDefaultAsync(m => m.Id == id);
            if (clientUser == null)
            {
                return NotFound();
            }

            _context.Client.Remove(clientUser);
            await _context.SaveChangesAsync();

            return Ok(clientUser);
        }

        private bool ClientUserExists(string id)
        {
            return _context.Client.Any(e => e.Id == id);
        }
    }
}