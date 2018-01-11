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
using WebApi.ViewModels.Questionnaires;
using WebApi.ViewModels.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Questionnaires")]
    public class QuestionnairesController : Controller
    {
        private readonly QuestionnaireDBContext _context;
        private readonly UserDBContext _userContext;

        public QuestionnairesController(QuestionnaireDBContext context, UserDBContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        // GET: api/Questionnaires
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetQuestionnaire(string clientId)
        {
            // Filter the questionnaires.
            IQueryable<Questionnaire> filteredQuestionnaires = _context.Questionnaire;
            if (!string.IsNullOrEmpty(clientId))
                filteredQuestionnaires = filteredQuestionnaires.Where(m => m.Client_id == clientId);

            // Return statuscode 204 due to lack of content.
            if (filteredQuestionnaires.Count() == 0)
                return NoContent();

            // Return statuscode 200 with the requested data.
            return Ok(filteredQuestionnaires);
        }

        // GET: api/Questionnaires/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetQuestionnaire([FromRoute] int id)
        {
            // Retreive the requested questionnaire.
            Questionnaire questionnaire = await _context.Questionnaire.SingleOrDefaultAsync(m => m.Id == id);

            // Return statuscode 404 due to the questionnaire that can't be found.
            if (questionnaire == null)
                return NotFound();

            // Retreive all foreign data.
            ClientUser client = _userContext.Client.Find(questionnaire.Client_id);

            // Return statuscode 500 due to corrupt data.
            if (client == null)
                return StatusCode(500);

            // Retreive all answers and question for the questionnaire.
            List<AnswerGetViewModel> answers = new List<AnswerGetViewModel>();
            foreach(Answers answer in _context.Answers.Where(a => a.Questionnaire_id == questionnaire.Id))
            {
                Questions question = _context.Questions.SingleOrDefault(m => m.Id == answer.Question_id);
                answers.Add(new AnswerGetViewModel
                {
                    QuestionId = question.Id,
                    Question = question.Question,
                    Answer = answer.Answer
                });
            }

            // Create an questionnaire viewmodel.
            QuestionnaireWithAnswersViewModel questionnaireViewModel = new QuestionnaireWithAnswersViewModel
            {
                Client = new ClientViewModel { id = client.Id, Email = client.Email, FirstName = client.FirstName, LastName = client.LastName, BirthDay = client.BirthDay, District = client.District, Gender = client.Gender, HouseNumber = client.HouseNumber, Province = client.Province, StreetName = client.StreetName },
                Time = questionnaire.Time,
                Answers = answers,
                Redflag = questionnaire.Redflag
            };

            // Return statuscode 200 with the requested data.
            return Ok(questionnaireViewModel);
        }

        // PUT: api/Questionnaires/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutQuestionnaire([FromRoute] int id, [FromBody] Questionnaire questionnaire)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Return statuscode 400 due to differences in the ID.
            if (id != questionnaire.Id)
                return BadRequest();

            // Try to save the changes requested changes.
            _context.Entry(questionnaire).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Questionnaire.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            // Return statuscode 204 when the questionnaire has been updated.
            return NoContent();
        }

        // POST: api/Questionnaires
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostQuestionnaire([FromBody] QuestionnairePostViewModel questionnaireViewModel)
        {
            // Return statuscode 400 due to non-valid post data.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create a new questionnaire.
            Questionnaire questionnaire = new Questionnaire
            {
                Time = questionnaireViewModel.Time,
                Client_id = questionnaireViewModel.Client_id,
                Redflag = questionnaireViewModel.Redflag
            };

            // Commit the changes to the database.
            _context.Questionnaire.Add(questionnaire);
            await _context.SaveChangesAsync();

            // Return the data of the newly created questionnaire.
            return CreatedAtAction("GetQuestionnaire", new { id = questionnaire.Id }, questionnaire);
        }

        // DELETE: api/Questionnaires/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteQuestionnaire([FromRoute] int id)
        {
            // Retreive the requested questionnaire.
            Questionnaire questionnaire = await _context.Questionnaire.SingleOrDefaultAsync(m => m.Id == id);

            // Return statuscode 404 due to the questionnaire that can't be found.
            if (questionnaire == null)
                return NotFound();
            
            // Remove the questionnaire.
            _context.Questionnaire.Remove(questionnaire);
            await _context.SaveChangesAsync();

            // Return statuscode 200 when the questionnaire has been deleted.
            return Ok(questionnaire);
        }
    }
}