using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Database.Abstractions;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IntervieweeProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {

        private readonly IPatientAccess _patientAccess;

        public PatientController(IPatientAccess patientAccess)
        {
            _patientAccess = patientAccess;
        }

        [HttpPost("/Patient/login")]
        public async Task<IActionResult> PatientLogin([FromForm] string Username,[FromForm] string Password)
        {
            return new JsonResult(await _patientAccess.LoginPatient(Username, Password));
        }

        [HttpPost("/Patient/register")]
        public async Task<IActionResult> PatientRegister([FromForm] string Username, [FromForm] string Password)
        {
            return new JsonResult(await _patientAccess.RegisterPatient(Username, Password));
        }

        [HttpPost("/Patient/logout")]
        public async Task<IActionResult> PatientLogout([FromHeader] string cookieID)
        {
            return new JsonResult(await _patientAccess.LogoutPatient(cookieID));
        }
    }
}
