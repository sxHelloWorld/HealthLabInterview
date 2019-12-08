using ClientApp.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.Database.Abstractions
{
    public interface IHomeAccess
    {
        Task<bool> SubmitQuestionAnswer(QuestionAnswer questionAnswer);

        Task<QuestionAnswer> GetQuestionAnswer(string QuestionID, string PatientID);

        Task<Questionnaire[]> GetQuestionnaires();

        Task<QuestionnaireTrack[]> GetQuestionnaireTracks(string PatientID);

        Task<string> CallQuestionnaire(string FormID, string PatientID);

        Task<Question> GetQuestion(string FormID, string PatientID, string QuestionID);

        Task<bool> AcceptQuestionnaire(string FormID, string PatientID, string Date);

        Task<string> PatientLogin(string Username, string Password);

        Task<bool> PatientLogout(string cookieID);

        


    }
}
