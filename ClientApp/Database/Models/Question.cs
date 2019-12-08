using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.Database.Models
{
    public class Question
    {
        public int QuestionID { get; set; }

        public int FormID { get; set; }

        public string QuestionAsk { get; set; }

        public QuestionForm QuestionType { get; set; }

        public string Choices { get; set; }

        public string Note { get; set; }
    }
}
