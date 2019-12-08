using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class QuestionAnswer
    {
        [Key]
        public int AnswerID { get; set; }

        public int PatientID { get; set; }

        public int FormID { get; set; }

        public int QuestionID { get; set; }

        public string Answer { get; set; }
    }
}
