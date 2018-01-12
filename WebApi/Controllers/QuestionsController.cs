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
    [Route("api/Questions")]
    public class QuestionsController : Controller
    {
        private readonly QuestionnaireDBContext _context;

        public QuestionsController(QuestionnaireDBContext context)
        {
            _context = context;
        }

        // GET: api/Questions
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetQuestions()
        {
            // Return statuscode 204 due to lack of content.
            if (_context.Questions.Count() == 0)
                return NoContent();

            // Return statuscode 200 with the requested data.
            return Ok(_context.Questions);
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetQuestions([FromRoute] int id)
        {
            // Retreive the requested question.
            Questions question = await _context.Questions.SingleOrDefaultAsync(m => m.Id == id);

            // Return statuscode 404 due to the question that can't be found.
            if (question == null)
                return NotFound();

            // Return statuscode 200 with the requested data.
            return Ok(question);
        }

        // PUT: api/Questions/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutQuestions([FromRoute] int id, [FromBody] Questions questions)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Return statuscode 400 due to differences in the ID.
            if (id != questions.Id)
                return BadRequest();

            // Try to save the changes requested changes.
            _context.Entry(questions).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Questions.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            // Return statuscode 204 when the question has been updated.
            return NoContent();
        }

        // POST: api/Questions
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostQuestions([FromBody] Questions questions)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Commit the changes to the database.
            _context.Questions.Add(questions);
            await _context.SaveChangesAsync();

            // Return the data of the newly created question.
            return CreatedAtAction("GetQuestions", new { id = questions.Id }, questions);
        }
    }
}