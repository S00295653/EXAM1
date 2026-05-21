using System.Data.Entity;       // EF6 — NuGet: EntityFramework
using PatientApp.Models;

namespace PatientApp.Data
{
    /// <summary>
    /// Database context (EF6, Code First).
    /// Database name: OODExam_FullName  ← remplace par ton nom complet
    /// </summary>
    public class PatientData : DbContext
    {
        // EF6: le string dans base() = nom de la connexion OU nom de la DB LocalDB
        public PatientData() : base("OODExam_FullName")
        {
        }

        public DbSet<Patient>     Patients     { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
