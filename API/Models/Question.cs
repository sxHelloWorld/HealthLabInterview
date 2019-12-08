using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Question
    {
        [Key]
        public int QuestionID { get; set; }

        [Required]
        public int FormID { get; set; }

        [Required]
        public string QuestionAsk { get; set; }

        [Required]
        public QuestionForm QuestionType { get; set; }

        public string Choices { get; set; }

        public string Note { get; set; }
    }
}
