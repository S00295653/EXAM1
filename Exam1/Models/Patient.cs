using System;
using System.Collections.Generic;

namespace PatientApp.Models
{
    /// <summary>
    /// Represents a patient in the medical system.
    /// A patient can have many appointments (One-to-Many).
    /// </summary>
    public class Patient
    {
        // Primary key - auto-incremented by Entity Framework
        public int PatientId { get; set; }

        // Personal details
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime DOB { get; set; }
        public string ContactNumber { get; set; }

        // Navigation property: One Patient → Many Appointments
        public virtual ICollection<Appointment> Appointments { get; set; }

        public Patient()
        {
            Appointments = new List<Appointment>();
        }

        // Formats the patient for ListBox display: "Surname, FirstName - ContactNumber"
        public override string ToString()
        {
            return $"{Surname}, {FirstName} - {ContactNumber}";
        }
    }
}
