using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Logging
    {
        public string UserID { get; set; }

        public DateTime Date { get; set; }

        public string Action { get; set; }

        public string Page { get; set; }

        public string Message { get; set; }
    }
}
