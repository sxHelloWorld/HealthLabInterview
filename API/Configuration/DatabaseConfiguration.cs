using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Configuration
{
    public class DatabaseConfiguration : DbContext
    {
        //public DbSet<WeatherForecast> Forecasts { get; set; }

        public DbSet<Questionnaire> Questionnaires { get; set; }

        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionnaireTrack> QuestionnaireTracks { get; set; }

        public DbSet<PatientAgreement> PatientAgreements { get; set; }

        public DbSet<Patient> Patients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=api.db");
    }
}
