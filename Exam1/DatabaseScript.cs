using System;
using System.Linq;
using System.Data.Entity;       // EF6 — NuGet: EntityFramework
using PatientApp.Data;
using PatientApp.Models;

namespace PatientApp
{
    /// <summary>
    /// Crée la base de données et insère les données initiales (Q1g).
    /// </summary>
    public static class DatabaseScript
    {
        public static void CreateAndSeedDatabase()
        {
            using (PatientData db = new PatientData())
            {
                // EF6: CreateIfNotExists() crée la DB si elle n'existe pas encore
                db.Database.CreateIfNotExists();

                if (!db.Patients.Any())
                {
                    // ── Seed Patients ──────────────────────────────────────────
                    Patient p1 = new Patient
                    {
                        FirstName     = "Mark",
                        Surname       = "Allen",
                        DOB           = new DateTime(1980, 5, 15),
                        ContactNumber = "086 123 9876"
                    };
                    Patient p2 = new Patient
                    {
                        FirstName     = "Tim",
                        Surname       = "Mahon",
                        DOB           = new DateTime(1975, 3, 22),
                        ContactNumber = "087 564 1489"
                    };
                    Patient p3 = new Patient
                    {
                        FirstName     = "Keith",
                        Surname       = "McManus",
                        DOB           = new DateTime(1990, 8, 1),
                        ContactNumber = "086 123 4567"
                    };
                    Patient p4 = new Patient
                    {
                        FirstName     = "Mary",
                        Surname       = "Smith",
                        DOB           = new DateTime(1985, 11, 30),
                        ContactNumber = "086 333 4444"
                    };
                    Patient p5 = new Patient
                    {
                        FirstName     = "John",
                        Surname       = "Smith",
                        DOB           = new DateTime(1992, 2, 14),
                        ContactNumber = "086 123 4567"
                    };

                    db.Patients.AddRange(new[] { p1, p2, p3, p4, p5 });

                    // ── Seed Appointments ──────────────────────────────────────
                    db.Appointments.AddRange(new[]
                    {
                        new Appointment
                        {
                            Patient          = p1,
                            AppointmentTime  = new DateTime(2025, 6, 10, 9, 30, 0),
                            AppointmentNotes = "Annual check-up"
                        },
                        new Appointment
                        {
                            Patient          = p1,
                            AppointmentTime  = new DateTime(2025, 7, 5, 14, 0, 0),
                            AppointmentNotes = "Follow-up consultation"
                        },
                        new Appointment
                        {
                            Patient          = p3,
                            AppointmentTime  = new DateTime(2025, 6, 15, 11, 0, 0),
                            AppointmentNotes = "Blood pressure review"
                        }
                    });

                    db.SaveChanges();
                }
            }
        }
    }
}
