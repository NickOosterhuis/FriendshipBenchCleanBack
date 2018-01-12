using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Contexts;
using WebApi.Models;
using WebApi.ViewModels;
using WebApi.ViewModels.Account;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly UserDBContext _context;

        public AccountController( UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config, UserDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _context = context;
        }

        // GET: api/account/user
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            // Retreive the requested user.
            User user = await _userManager.GetUserAsync(User);

            // Return statuscode 404 due to the user that can't be found.
            if (user == null)
                return NotFound();

            // Return statuscode 200 with the requested data.
            return Ok(user);
        }

        // POST: api/register/admin
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminViewModel Credentials)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create a new user.
            User admin = new User
            {
                UserName = Credentials.Email,
                Email = Credentials.Email,
            };

            // Add the new user and give it a role.
            var result = await _userManager.CreateAsync(admin, Credentials.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "admin");
                await _signInManager.SignInAsync(admin, isPersistent: false);
                return Ok("User successfully registered");
            }

            // Return an error if the action couldn't be executed succesfully.
            return Errors(result);
        }

        // POST: api/account/register/client
        [AllowAnonymous]
        [HttpPost("register/client")]
        public async Task<IActionResult> RegisterClient([FromBody] RegisterClientViewModel Credentials)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Register a new client.
            var client = new ClientUser
            {
                UserName = Credentials.Email,
                Email = Credentials.Email,
                FirstName = Credentials.FirstName,
                LastName = Credentials.LastName,
                Gender = Credentials.Gender,
                BirthDay = Credentials.BirthDay,
                StreetName = Credentials.StreetName,
                HouseNumber = Credentials.HouseNumber,
                Province = Credentials.Province,
                District = Credentials.District,
            };

            // Add the new user and give it a role.
            var result = await _userManager.CreateAsync(client, Credentials.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(client, "client");
                await _signInManager.SignInAsync(client, isPersistent: false);
                return Ok("User successfully registered");
            }

            // Return an error if the action couldn't be executed succesfully.
            return Errors(result);
        }

        // POST: api/account/register/healthworker
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("register/healthworker")]
        public async Task<IActionResult> RegisterHealthWorker([FromBody] RegisterHealthWorkerViewModel Credentials)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            // Register a new healthworker.
            var healthWorker = new HealthWorkerUser
            {
                UserName = Credentials.Email,
                Email = Credentials.Email,
                FirstName = Credentials.FirstName,
                LastName = Credentials.LastName,
                Gender = Credentials.Gender,
                BirthDay = Credentials.BirthDay,
                PhoneNumber = Credentials.PhoneNumber,
            };

            // Add the new healhtworker and give it a role.
            var result = await _userManager.CreateAsync(healthWorker, Credentials.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(healthWorker, "healthworker");
                await _signInManager.SignInAsync(healthWorker, isPersistent: false);
                return Ok("User successfully registered");
            }

            // Return an error if the action couldn't be executed succesfully.
            return Errors(result);
        }

        // POST: api/account/signin
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginViewModel Credentials)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the user can be logged in.
            var result = await _signInManager.PasswordSignInAsync(Credentials.Email, Credentials.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(Credentials.Email);
                return Ok("User signed in successfull");
            }

            // Return statuscode 401 due to a login fault.
            return new JsonResult("Unable to sign in") { StatusCode = 401 };

        }

        // POST: api/account/generatetoken
        [AllowAnonymous]
        [HttpPost("generatetoken")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginViewModel model)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Retreive the the requested user.
            var user = await _userManager.FindByEmailAsync(model.Email);

            // Return statuscode 404 due to the user that can't be found.
            if (user == null)
                return NotFound();

            // Check if the credentials are correct.
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded)
            {
                // Set the token variables.
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _config["JWTSettings:Issuer"],
                    audience: _config["JWTSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

                // Return statuscode 200 with the JWT.
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            // Return statuscode 401 due to a login fault.
            return new JsonResult("Unable to generate toke") { StatusCode = 401 };
        }

        // POST: api/account/signout
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("signout")]
        public async Task<IActionResult> SignOut()
        {
            // Let the user sign out.
            await _signInManager.SignOutAsync();
            JsonResult logoutMessage = new JsonResult("User is Logged out");
            return Ok(logoutMessage); 
        }

        // GET: api/account/currentUser/example@example.com
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("currentUser/{email}")]
        public async Task<IActionResult> GetCurrentUser([FromRoute] string email)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Retreive the requested user.
            User currentUser = await _userManager.FindByEmailAsync(email);

            // Return statuscode 404 due to the current user that can't be found.
            if (currentUser == null)
                return NotFound();

            // Return statuscode 200 with the requested data.
            return Ok(currentUser);
        }

        // PUT: api/Account/edit/example@example.com
        [HttpPut("edit/{email}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutClientUserByEmail([FromRoute] string email, [FromBody] EditUserViewModel user)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Retreive the current user.
            var currentUser = _context.Client.AsNoTracking().SingleOrDefault(x => x.Email == email);

            // Return statuscode 404 due to the user that can't be found.
            if (currentUser == null)
                return NotFound();

            // Create a new user model.
            ClientUser newUser = currentUser;
            newUser.StreetName = user.StreetName;
            newUser.HouseNumber = user.HouseNumber;
            newUser.Province = user.Province;
            newUser.District = user.District;

            // Try to save the requested changes.
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientUserExists(email))
                    return NotFound();
                else
                    throw;
            }

            // Return statuscode 204 when the user has been updated.
            return NoContent();
        }

        // PUT: api/Account/edit/example@example.com
        [HttpPut("addHealthworker/{email}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutHealthWorkerClientUserByEmail([FromRoute] string email, [FromBody] AddHealthworkerToUserViewModel user)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Retreive the current user.
            var currentUser = _context.Client.AsNoTracking().SingleOrDefault(x => x.Email == email);

            // Return statuscode 404 due to the user that can't be found.
            if (currentUser == null)
                return NotFound();

            // Update the healthworker relation of the user.
            ClientUser newUser = currentUser;
            currentUser.HealthWorker_Id = user.HealthWorker_Id;

            // Try to save the requested changes.
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientUserExists(email))
                    return NotFound();
                else
                    throw;
            }

            // Return statuscode 204 when the user has been updated.
            return NoContent();
        }

        private JsonResult Errors(IdentityResult result)
        {
            var items = result.Errors
                .Select(x => x.Description)
                .ToArray();
            return new JsonResult(items) { StatusCode = 400 };
        }

        private bool ClientUserExists(string id)
        {
            return _context.Client.Any(e => e.Id == id);
        }
    }   
}
