using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Database.Abstractions;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuestionnaireController : ControllerBase
    {
        private readonly IQuestionnaireAccess _questionnaireAccess;

        public QuestionnaireController(IQuestionnaireAccess questionnaireAccess)
        {
            _questionnaireAccess = questionnaireAccess;
        }

        [HttpGet("/Questionnaire")]
        public async Task<IActionResult> Index()
        {
            return new JsonResult(await _questionnaireAccess.GetQuestionnaires());
        }

        [HttpGet("/Questionnaire/Question")]
        public async Task<IActionResult> GetQuestion([FromHeader] int FormID, [FromHeader] int QuestionID)
        {
            return new JsonResult(await _questionnaireAccess.GetQuestion(FormID, QuestionID));
        }

        [HttpPost("/Questionnaire/Answer")]
        public async Task<IActionResult> GetQuestionAnswer([FromForm] string QuestionID, [FromForm] string PatientID)
        {
            return new JsonResult(await _questionnaireAccess.GetQuestionAnswer(int.Parse(QuestionID), int.Parse(PatientID)));
        }

        [HttpPost("/Questionnaire/Submit")]
        public async Task<IActionResult> SubmitAnswer([FromForm] string content)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IgnoreNullValues = true;
            QuestionAnswer answer = JsonSerializer.Deserialize<QuestionAnswer>(content, options);
            QuestionAnswer questionAnswer = new QuestionAnswer();
            return new JsonResult(await _questionnaireAccess.SubmitQuestionAnswer(answer));
        }

        [HttpPost("/Questionnaire/Call")]
        public async Task<IActionResult> Call([FromForm] string FormID, [FromForm] string PatientID)
        {
            return new JsonResult(await _questionnaireAccess.CallQuestionnaire(int.Parse(FormID), int.Parse(PatientID)));
        }

        [HttpPost("/Questionnaire/AcceptAgreement")]
        public async Task<IActionResult> AcceptAgreement([FromForm] string FormID, [FromForm] string PatientID, [FromForm] string Date)
        {
            return new JsonResult(await _questionnaireAccess.AcceptQuestionnaire(int.Parse(FormID), int.Parse(PatientID), Date));
        }

        [HttpGet("/Questionnaire/ReviewAnswer")]
        public async Task<IActionResult> ReviewAnswers([FromHeader] string FormID, [FromHeader] string PatientID)
        {
            return new JsonResult(await _questionnaireAccess.ReviewAnswersQuestionnaire(int.Parse(FormID), int.Parse(PatientID)));
        }

        [HttpGet("/Questionnaire/ReviewQuestion")]
        public async Task<IActionResult> ReviewQuestions([FromHeader] string FormID)
        {
            // TODO Verify if user is logged in to prevent leaking the questions
            return new JsonResult(await _questionnaireAccess.ReviewQuestionsQuestionnaire(int.Parse(FormID)));
        }

        [HttpGet("/Questionnaire/Track")]
        public async Task<IActionResult> GetTracks([FromHeader] string PatientID)
        {
            return new JsonResult(await _questionnaireAccess.GetQuestionnaireTracks(int.Parse(PatientID)));
        }
    }
}