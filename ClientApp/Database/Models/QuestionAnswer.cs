using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.Database.Models
{
    public class QuestionAnswer
    {
        [Key]
        public string AnswerID { get; set; }

        [Required]
        public string PatientID { get; set; }

        [Required]
        public string FormID { get; set; }

        [Required]
        public string QuestionID { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}
