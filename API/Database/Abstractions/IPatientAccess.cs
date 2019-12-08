using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Database.Abstractions
{
    public interface IPatientAccess
    {
        Task<bool> RegisterPatient(string Username, string Password);

        Task<string> LoginPatient(string Username, string Password);

        Task<bool> LogoutPatient(string loginID);
    }
}
