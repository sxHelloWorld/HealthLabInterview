using ClientApp.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.ViewModels
{
    public class QuestionnaireListViewModel
    {
        public Questionnaire[] questionnaires { get; set; }

        public QuestionnaireTrack[] questionnaireTracks { get; set; }
    }
}
