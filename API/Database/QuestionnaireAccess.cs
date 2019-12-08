using API.Configuration;
using API.Database.Abstractions;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Database
{
    public class QuestionnaireAccess : IQuestionnaireAccess
    {
        public async Task<QuestionAnswer> GetQuestionAnswer(int QuestionID, int PatientID)
        {
            using (var db = new DatabaseConfiguration())
            {
                QuestionAnswer answer = await db.QuestionAnswers.FirstAsync(q => q.QuestionID == QuestionID && q.PatientID == PatientID);

                if (answer == null)
                {
                    answer = new QuestionAnswer();
                    answer.PatientID = PatientID;
                    answer.QuestionID = QuestionID;
                }

                return answer;
            }
        }

        public async Task<Questionnaire[]> GetQuestionnaires()
        {
            using (var db = new DatabaseConfiguration())
            {
                return await db.Questionnaires.ToArrayAsync();
            }
        }

        public async Task<bool> SubmitQuestionAnswer(QuestionAnswer answer)
        {
            using (var db = new DatabaseConfiguration())
            {
                try
                {
                    await db.QuestionAnswers.AddAsync(answer);
                    QuestionnaireTrack questionnaireTrack = await db.QuestionnaireTracks.SingleAsync(track => track.FormID == answer.FormID && track.PatientID == answer.PatientID);
                    Questionnaire questionnaire = await db.Questionnaires.SingleAsync(quest => quest.FormID == questionnaireTrack.FormID);
                    string[] questionIDs = questionnaire.QuestionIDs.Split(",");
                    int currentIndex = Array.IndexOf(questionIDs, answer.QuestionID.ToString());
                    if (currentIndex + 1 > questionIDs.Length)
                    {
                        questionnaireTrack.Review = true;
                    }
                    else
                    {
                        questionnaireTrack.CurrentQuestionID = int.Parse(questionIDs[currentIndex + 1]);
                    }
                    await db.QuestionnaireTracks.AddAsync(questionnaireTrack);
                    await db.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    // TODO
                    return false;
                }
                
            }
        }

        public async Task<string> CallQuestionnaire(int FormID, int PatientID)
        {
            using (var db = new DatabaseConfiguration())
            {
                bool agreed = await db.PatientAgreements.AnyAsync(a => a.FormID == FormID && a.PatientID == PatientID);
                if (agreed)
                {
                    bool trackExist = await db.QuestionnaireTracks.AnyAsync(t => t.FormID == FormID && t.PatientID == PatientID);
                    if (trackExist)
                    {
                        QuestionnaireTrack track = await db.QuestionnaireTracks.SingleAsync(t => t.FormID == FormID && t.PatientID == PatientID);
                        if (track.Complete)
                        {
                            return "complete";
                        }
                        else if (track.Review)
                        {
                            return "review";
                        }
                        return track.CurrentQuestionID.ToString();
                    }
                    Questionnaire questionnaire = await db.Questionnaires.SingleAsync(q => q.FormID == FormID);
                    return questionnaire.QuestionIDs.Split(",").First();
                }
                return "notagreed";
            }
        }

        public async Task<bool> AcceptQuestionnaire(int FormID, int PatientID, string Date)
        {
            using (var db = new DatabaseConfiguration())
            {
                bool agreed = await db.PatientAgreements.AnyAsync(a => a.FormID == FormID && a.PatientID == PatientID);
                if (!agreed)
                {
                    try
                    {
                        PatientAgreement agreement = new PatientAgreement();
                        agreement.FormID = FormID;
                        agreement.PatientID = PatientID;
                        agreement.Time = Date;
                        await db.PatientAgreements.AddAsync(agreement);

                        QuestionnaireTrack track = new QuestionnaireTrack();
                        track.PatientID = PatientID;
                        track.FormID = FormID;
                        track.Review = false;
                        track.Complete = false;

                        Questionnaire questionnaire = await db.Questionnaires.SingleAsync(q => q.FormID == FormID);

                        track.CurrentQuestionID = int.Parse(questionnaire.QuestionIDs.Split(",").First());

                        await db.QuestionnaireTracks.AddAsync(track);

                        await db.SaveChangesAsync();
                        return true;
                    }
                    catch
                    {
                        // TODO
                        return false;
                    }
                }
                return true;
            }
        }

        public async Task<QuestionAnswer[]> ReviewAnswersQuestionnaire(int FormID, int PatientID)
        {
            using (var db = new DatabaseConfiguration())
            {
                // TODO
                // Need to optimize the search so we don't have to
                // manually select each answer from the list where FormID and PatientID
                // matched
                List<QuestionAnswer> queryArray = await db.QuestionAnswers.ToListAsync();
                List<QuestionAnswer> answers = new List<QuestionAnswer>();
                for (int index = 0; index < queryArray.Count(); index++)
                {
                    QuestionAnswer answer = answers[index];
                    if (answer.FormID == FormID && answer.PatientID == PatientID)
                    {
                        answers.Add(answer);
                    }
                }
                return answers.ToArray();
            }
        }

        public async Task<Question[]> ReviewQuestionsQuestionnaire(int FormID)
        {
            using (var db = new DatabaseConfiguration())
            {
                // TODO
                // Optimize the search as well
                List<Question> queryArray = await db.Questions.ToListAsync();
                List<Question> questions = new List<Question>();
                for (int index = 0; index < queryArray.Count(); index++)
                {
                    Question question = queryArray[index];
                    if (question.FormID == FormID)
                    {
                        questions.Add(question);
                    }
                }
                return questions.ToArray();
            }
        }

        public async Task<Question> GetQuestion(int FormID, int QuestionID)
        {
            using (var db = new DatabaseConfiguration())
            {
                return await db.Questions.SingleAsync(q => q.FormID == FormID && q.QuestionID == QuestionID);
            }
        }

        public async Task<QuestionnaireTrack[]> GetQuestionnaireTracks(int PatientID)
        {
            using (var db = new DatabaseConfiguration())
            {
                QuestionnaireTrack[] tracks = await db.QuestionnaireTracks.ToArrayAsync();
                List<QuestionnaireTrack> questionnaireTracks = new List<QuestionnaireTrack>();
                foreach (var t in tracks)
                {
                    if (t.PatientID == PatientID)
                    {
                        questionnaireTracks.Add(t);
                    }
                }

                return questionnaireTracks.ToArray();
            }
        }
    }
}
