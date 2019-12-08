using ClientApp.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.ViewModels
{
    public class QuestionnaireQuestionViewModel
    {
        public string FormID { get; set; }

        public string QuestionID { get; set; }

        public Question CurrentQuestion { get; set; }

        public QuestionAnswer CurrentAnswer { get; set; }
    }
}
