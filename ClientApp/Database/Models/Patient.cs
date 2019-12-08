using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.Database.Models
{
    public class Patient
    {
        public int PatientID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
