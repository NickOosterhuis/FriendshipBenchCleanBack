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
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Answers")]
    public class AnswersController : Controller
    {
        private readonly QuestionnaireDBContext _context;

        public AnswersController(QuestionnaireDBContext context)
        {
            _context = context;
        }

        // POST: api/Answers
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostAnswers([FromBody] List<AnswerPostViewModel> answerViewModels)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Commit every answer.
            foreach (AnswerPostViewModel answerViewModel in answerViewModels)
            {
                Answers answers = new Answers
                {
                    Answer = answerViewModel.Answer,
                    Questionnaire_id = answerViewModel.Questionnaire_id,
                    Question_id = answerViewModel.Question_id
                };
                _context.Answers.Add(answers);
            }
            await _context.SaveChangesAsync();

            // Return statuscode 204 when the answers have been posted.
            return NoContent();
        }

        // DELETE: api/Answers/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAnswers([FromRoute] int id)
        {
            // Retreive the requested answer.
            Answers answer = await _context.Answers.SingleOrDefaultAsync(m => m.Id == id);

            // Return statuscode 404 due to the answer that can't be found.
            if (answer == null)
                return NotFound();

            // Remove the answer.
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();

            // Return statuscode 200 when the answer has been deleted.
            return Ok(answer);
        }
    }
}