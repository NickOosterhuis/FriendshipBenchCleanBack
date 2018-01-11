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
        public IEnumerable<Questionnaire> GetQuestionnaire()
        {
            return _context.Questionnaire;
        }

        // GET: api/Questionnaires/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetQuestionnaire([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questionnaire = await _context.Questionnaire.SingleOrDefaultAsync(m => m.Id == id);

            if (questionnaire == null)
            {
                return NotFound();
            }

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

            ClientUser client = _userContext.Client.Find(questionnaire.Client_id);

            QuestionnaireWithAnswersViewModel questionnaireViewModel = new QuestionnaireWithAnswersViewModel
            {
                Client = new ClientViewModel { id = client.Id, Email = client.Email, FirstName = client.FirstName, LastName = client.LastName, BirthDay = client.BirthDay, District = client.District, Gender = client.Gender, HouseNumber = client.HouseNumber, Province = client.Province, StreetName = client.StreetName },
                Time = questionnaire.Time,
                Answers = answers,
                Redflag = questionnaire.Redflag
            };

            return Ok(questionnaireViewModel);
        }

        // PUT: api/Questionnaires/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutQuestionnaire([FromRoute] int id, [FromBody] Questionnaire questionnaire)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != questionnaire.Id)
            {
                return BadRequest();
            }

            _context.Entry(questionnaire).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionnaireExists(id))
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

        // POST: api/Questionnaires
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostQuestionnaire([FromBody] QuestionnairePostViewModel questionnaireViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Questionnaire questionnaire = new Questionnaire
            {
                Time = questionnaireViewModel.Time,
                Client_id = questionnaireViewModel.Client_id,
                Redflag = questionnaireViewModel.Redflag
            };

            _context.Questionnaire.Add(questionnaire);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestionnaire", new { id = questionnaire.Id }, questionnaire);
        }

        // DELETE: api/Questionnaires/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteQuestionnaire([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questionnaire = await _context.Questionnaire.SingleOrDefaultAsync(m => m.Id == id);
            if (questionnaire == null)
            {
                return NotFound();
            }

            _context.Questionnaire.Remove(questionnaire);
            await _context.SaveChangesAsync();

            return Ok(questionnaire);
        }

        private bool QuestionnaireExists(int id)
        {
            return _context.Questionnaire.Any(e => e.Id == id);
        }
    }
}