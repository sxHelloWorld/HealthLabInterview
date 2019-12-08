using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class QuestionnaireLog
    {
        [Key]
        public int FormID { get; set; }

        [Key]
        public int QuestionID { get; set; }

        [Key]
        public int PatientID { get; set; }

        public DateTime Date { get; set; }

        public long Duration { get; set; }

        public string Action { get; set; }

    }
}
