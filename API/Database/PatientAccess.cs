using API.Configuration;
using API.Database.Abstractions;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Database
{
    public class PatientAccess : IPatientAccess
    {

        public async Task<bool> RegisterPatient(string Username = "", string Password = "")
        {
            string username = Username.Trim();
            string password = Password.Trim();

            if (username.Length == 0 || password.Length == 0)
            {
                return false;
            }

            using (var db = new DatabaseConfiguration())
            {

                int count = await db.Patients.CountAsync();

                Patient patient = null;

                if (count != 0)
                {
                    patient = await db.Patients.FirstAsync(p => p.Username == username);

                    if (patient != null)
                    {
                        return false;
                    }
                }

                patient = new Patient();
                patient.Username = username;
                patient.Password = password;

                try
                {
                    await db.Patients.AddAsync(patient);
                    await db.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    // TODO
                    Console.WriteLine(ex);
                    return false;
                }
            }
        }

        public async Task<string> LoginPatient(string Username = "", string Password = "")
        {
            string username = Username.Trim();
            string password = Password.Trim();

            // Checks if either username or password is empty
            if (username.Length == 0 || password.Length == 0)
            {
                return "invalid";
            }
            using (var db = new DatabaseConfiguration())
            {
                Patient patient;
                try
                {
                    patient = await db.Patients.SingleAsync(p => p.Username == username);
                }
                catch (Exception ex)
                {
                    // TODO
                    Console.WriteLine(ex);
                    return "invalid";
                }

                // We should hash the password and verify if the hashed password match
                // the current password in database - The password hashing is not implemented
                // here yet due to limit time
                if (patient.Password == password)
                {
                    // The system should implement a cookie system for patient to hold
                    // For now we will use patient ID
                    return "success," + patient.PatientID.ToString();
                }

                return "invalid";
            }
        }

        public async Task<bool> LogoutPatient(string cookieID)
        {
            // Not implemented yet but we should remove the cookie (authorization)
            // from database and inform that patient is logged out
            using (var db = new DatabaseConfiguration())
            {
                // TODO
                await db.Patients.CountAsync();
            }

            return true;
        }
    }
}
