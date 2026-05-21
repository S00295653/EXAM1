using System;

namespace PatientApp.Models
{
    /// <summary>
    /// Represents a medical appointment linked to a Patient.
    /// </summary>
    public class Appointment
    {
        // Primary key - auto-incremented by Entity Framework
        public int AppointmentId { get; set; }

        // Appointment details
        public DateTime AppointmentTime { get; set; }
        public string AppointmentNotes { get; set; }

        // Foreign key for the One-to-Many relationship with Patient
        public int PatientId { get; set; }

        // Navigation property back to the parent Patient
        public virtual Patient Patient { get; set; }

        // Formats the appointment for ListBox display
        public override string ToString()
        {
            return $"{AppointmentTime:dd/MM/yyyy HH:mm} - {AppointmentNotes}";
        }
    }
}
