using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class PatientAgreement
    {
        [Key]
        public int AgreementID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int FormID { get; set; }

        [Required]
        public string Time { get; set; }
    }
}
