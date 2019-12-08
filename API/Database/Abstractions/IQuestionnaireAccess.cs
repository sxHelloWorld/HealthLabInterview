using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Database.Abstractions
{
    public interface IQuestionnaireAccess
    {
        Task<bool> SubmitQuestionAnswer(QuestionAnswer answer);

        Task<QuestionAnswer> GetQuestionAnswer(int QuestionID, int PatientID);

        Task<Questionnaire[]> GetQuestionnaires();

        Task<QuestionnaireTrack[]> GetQuestionnaireTracks(int PatientID);

        Task<string> CallQuestionnaire(int FormID, int PatientID);

        Task<bool> AcceptQuestionnaire(int FormID, int PatientID, string Date);

        Task<Question> GetQuestion(int FormID, int QuestionID);

        Task<QuestionAnswer[]> ReviewAnswersQuestionnaire(int FormID, int PatientID);

        Task<Question[]> ReviewQuestionsQuestionnaire(int FormID);
    }
}
