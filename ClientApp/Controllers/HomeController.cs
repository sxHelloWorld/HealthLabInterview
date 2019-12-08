using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ClientApp.Database.Abstractions;
using ClientApp.Database.Models;
using ClientApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace ClientApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHomeAccess _homeAccess;

        public HomeController(IHomeAccess homeAccess)
        {
            _homeAccess = homeAccess;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // TODO Check authorization
            if (Request.Cookies.ContainsKey("login"))
            {
                return Redirect("/Questionnaire");
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            string loginResult = await _homeAccess.PatientLogin(username, password);
            if (loginResult == "invalid")
            {
                Response.Headers.Add("Error", "Invalid Login Information");
                return View("Index");
            }

            string[] results = loginResult.Split(",");

            if (results[0] == "success")
            {
                // Login Cookie
                Response.Cookies.Append("login", results[1]);
                return Redirect("/Questionnaire");
            }
            return View("Index");
        }

        [HttpGet("/Questionnaire")]
        public async Task<IActionResult> QuestionnaireList()
        {
            // TODO Check authorization
            if (Request.Cookies.ContainsKey("login"))
            {

                Questionnaire[] questionnaires = await _homeAccess.GetQuestionnaires();
                QuestionnaireTrack[] questionnaireTracks = await _homeAccess.GetQuestionnaireTracks(ExtractPatientID());

                QuestionnaireListViewModel viewModel = new QuestionnaireListViewModel();
                viewModel.questionnaires = questionnaires;
                viewModel.questionnaireTracks = questionnaireTracks;

                return View("QuestionnaireList", viewModel);
            }
            return Redirect("/");
        }

        [HttpGet("/Questionnaire/AcceptAgreement")]
        public async Task<IActionResult> ViewAgreement(string formid, string formtitle, string date)
        {
            AgreementViewModel viewModel = new AgreementViewModel();

            viewModel.FormID = formid;
            viewModel.FormTitle = formtitle;
            viewModel.Date = date;
            return View("Agreement", viewModel);
        }

        [HttpPost("/Questionnaire")]
        public async Task<IActionResult> CallQuestionnaire([FromForm] string formid, [FromForm] string formtitle)
        {
            // TODO Check authorization
            if (Request.Cookies.ContainsKey("login"))
            {
                string result = await _homeAccess.CallQuestionnaire(formid, ExtractPatientID());
                Response.Headers.Add("formid", formid);
                if (result == "invalid")
                {
                    return Redirect("/");
                }
                else if (result == "notagreed")
                {
                    return RedirectToAction("ViewAgreement", new { formid, formtitle, date = DateTime.Today.ToShortDateString() });
                }
                else if (result == "review" || result == "complete")
                {
                    return Redirect("/Questionnaire/Review");
                }
                else
                {
                    return RedirectToAction("ViewQuestionnaire", new { formid = result });
                }
            }
            return Redirect("/");
        }

        [HttpGet("/Questionnaire/Review")]
        public async Task<IActionResult> ReviewQuestionnaire([FromHeader] string formid)
        {
            if (Request.Cookies.ContainsKey("login"))
            {

            }
            return Redirect("/");
        }

        [HttpGet("/Questionnaire/View")]
        public async Task<IActionResult> ViewQuestionnaire(string formid)
        {
            if (Request.Cookies.ContainsKey("login"))
            {
                string result = await _homeAccess.CallQuestionnaire(formid, ExtractPatientID());
                if (result == "complete" || result == "review")
                {
                    Response.Headers.Add("formid", formid);
                    return Redirect("/Questionnaire/Review");
                }
                Question question = await _homeAccess.GetQuestion(formid, ExtractPatientID(), result);
                QuestionAnswer questionAnswer = await _homeAccess.GetQuestionAnswer(result, ExtractPatientID());
                if (questionAnswer == null)
                {
                    questionAnswer = new QuestionAnswer();
                    questionAnswer.FormID = formid;
                    questionAnswer.PatientID = ExtractPatientID();
                    questionAnswer.QuestionID = result;
                }
                QuestionnaireQuestionViewModel viewModel = new QuestionnaireQuestionViewModel();
                viewModel.CurrentQuestion = question;
                viewModel.CurrentAnswer = questionAnswer;

                return View("QuestionnaireQuestion", viewModel);
            }
            return Redirect("/");
        }

        [HttpPost("/Questionnaire/View")]
        public async Task<IActionResult> SubmitQuestionAnswer ([FromForm] string formid, [FromForm] string questionid, [FromForm] string answer, [FromForm] string answerid = "")
        {
            if (Request.Cookies.ContainsKey("login"))
            {
                string PatientID = ExtractPatientID();
                QuestionAnswer questionAnswer = new QuestionAnswer();
                if (answerid.Length != 0)
                {
                    questionAnswer.AnswerID = answerid;
                }
                questionAnswer.FormID = formid;
                questionAnswer.PatientID = PatientID;
                questionAnswer.QuestionID = questionid;
                questionAnswer.Answer = answer;
                bool result = await _homeAccess.SubmitQuestionAnswer(questionAnswer);
                if (result)
                {
                    return RedirectToAction("ViewQuestionnaire", new { formid });
                }
                // TODO
                return Redirect("/");
            }
            return Redirect("/");
        }

        [HttpPost("/Questionnaire/AcceptAgreement")]
        public async Task<IActionResult> AgreeQuestionnaire([FromForm] string formid, [FromForm] string date)
        {
            if (Request.Cookies.ContainsKey("login"))
            {
                string PatientID = ExtractPatientID();
                bool ok = await _homeAccess.AcceptQuestionnaire(formid, PatientID, date);
                if (ok)
                {
                    Response.Headers.Add("formid", formid);
                    return Redirect("/Questionnaire/View");
                }
                Response.Headers.Add("Error", "Problem in agreement process");
                return Redirect("/");
            }
            return Redirect("/");
        }

        [HttpPost("/Logout")]
        public async Task<IActionResult> PatientLogout(string action)
        {
            if(Request.Cookies.ContainsKey("login"))
            {
                bool result = await _homeAccess.PatientLogout(ExtractPatientID());
                Response.Cookies.Delete("login");
            }
            return Redirect("/");
        }

        private string ExtractPatientID()
        {
            string patientID = "";
            Request.Cookies.TryGetValue("login", out patientID);
            return patientID;
        }
    }
}
