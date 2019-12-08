using ClientApp.Database.Abstractions;
using ClientApp.Database.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientApp.Database
{
    public class HomeAccess : IHomeAccess
    {

        private readonly IHttpClientFactory _clientFactory;

        public HomeAccess(IHttpClientFactory factory)
        {
            _clientFactory = factory;
        }

        public async Task<QuestionAnswer> GetQuestionAnswer(string QuestionID, string PatientID)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:5001/Questionnaire/Answer");
            var client = _clientFactory.CreateClient();
            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("QuestionID", QuestionID));
            values.Add(new KeyValuePair<string, string>("PatientID", PatientID));
            request.Content = new FormUrlEncodedContent(values);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<QuestionAnswer>(stream);
            }
            return null;
        }

        public async Task<Questionnaire[]> GetQuestionnaires()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:5001/Questionnaire");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            Questionnaire[] questionnaires;

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                questionnaires = await JsonSerializer.DeserializeAsync<Questionnaire[]>(stream);
            }
            else
            {
                throw new Exception("There was an error in system.");
            }

            return questionnaires;
        }

        public async Task<bool> SubmitQuestionAnswer(QuestionAnswer answer)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:5001/Questionnaire/Submit");
            var client = _clientFactory.CreateClient();

            string json = JsonSerializer.Serialize(answer);
            request.Content = new StringContent(json);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<bool>(stream);
            }
            return false;
        }

        public async Task<string> PatientLogin(string Username, string Password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:5001/Patient/login");
            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("username", Username));
            values.Add(new KeyValuePair<string, string>("password", Password));
            request.Content = new FormUrlEncodedContent(values);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string result = "invalid";

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<string>(stream);
            }
            else
            {
                throw new Exception("Login failed");
            }

            return result;

        }

        public async Task<bool> PatientLogout(string patientID)
        {
            // TODO checks and receive the "OK" to logout
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:5001/Patient/logout");
            var client = _clientFactory.CreateClient();
            request.Headers.Add("cookieID", patientID);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                // TODO
            }
            return true;
        }

        public Task<bool> SubmitQuestionAnswer(string FormID, string PatientID, string QuestionID, string answer)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CallQuestionnaire(string FormID, string PatientID)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:5001/Questionnaire/Call");
            var client = _clientFactory.CreateClient();

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("FormID", FormID));
            values.Add(new KeyValuePair<string, string>("PatientID", PatientID));
            request.Content = new FormUrlEncodedContent(values);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<string>(stream);
            }

            return "invalid";
        }

        public async Task<bool> AcceptQuestionnaire(string FormID, string PatientID, string Date)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:5001/Questionnaire/AcceptAgreement");
            var client = _clientFactory.CreateClient();

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("FormID", FormID));
            values.Add(new KeyValuePair<string, string>("PatientID", PatientID));
            values.Add(new KeyValuePair<string, string>("Date", Date));
            request.Content = new FormUrlEncodedContent(values);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<bool>(stream);
            }
            return false;
        }

        public async Task<Question> GetQuestion(string FormID, string PatientID, string QuestionID)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:5001/Questionnaire/Question");
            var client = _clientFactory.CreateClient();
            request.Headers.Add("FormID", FormID);
            request.Headers.Add("QuestionID", QuestionID);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Question>(stream);
            }
            return null;
        }

        public async Task<QuestionnaireTrack[]> GetQuestionnaireTracks(string PatientID)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:5001/Questionnaire/Track");
            var client = _clientFactory.CreateClient();
            request.Headers.Add("PatientID", PatientID);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                QuestionnaireTrack[] tracks = await JsonSerializer.DeserializeAsync<QuestionnaireTrack[]>(stream);
                return tracks;
            }
            return new QuestionnaireTrack[0];
        }
    }
}
