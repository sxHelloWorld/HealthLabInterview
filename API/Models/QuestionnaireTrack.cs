using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class QuestionnaireTrack
    {
        [Key]
        public int TrackID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int FormID { get; set; }

        public int CurrentQuestionID { get; set; }

        public bool Complete { get; set; }

        public bool Review { get; set; }
    }
}
