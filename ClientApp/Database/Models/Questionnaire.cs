using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.Database.Models
{
    public class Questionnaire
    {
        public string Title { get; set; }

        public int FormID { get; set; }

        public string QuestionIDs { get; set; }
    }
}
