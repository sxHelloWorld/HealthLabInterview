using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Questionnaire
    {
        [Required]
        public string Title { get; set; }

        [Key]
        public int FormID { get; set; }

        public string QuestionIDs { get; set; }
    }
}
