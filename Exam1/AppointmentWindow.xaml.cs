using System;
using System.Windows;
 // EF Core
using PatientApp.Data;
using PatientApp.Models;

namespace PatientApp
{
    /// <summary>
    /// Fenêtre pour ajouter (Q1k) ou modifier (Q1m) un appointment.
    /// </summary>
    public partial class AppointmentWindow : Window
    {
        private readonly PatientData _db;
        private readonly int _patientId;
        private readonly Appointment _existingAppointment;

        // ─── Constructeur ADD ─────────────────────────────────────────────────────
        public AppointmentWindow(int patientId)
        {
            InitializeComponent();
            _db                  = new PatientData();
            _patientId           = patientId;
            _existingAppointment = null;

            btnUpdate.IsEnabled  = false;
            btnUpdate.Visibility = Visibility.Collapsed;
        }

        // ─── Constructeur EDIT ────────────────────────────────────────────────────
        public AppointmentWindow(int patientId, Appointment appointment)
        {
            InitializeComponent();
            _db                  = new PatientData();
            _patientId           = patientId;
            _existingAppointment = appointment;

            // Pré-remplissage des champs
            dpAppointmentDate.SelectedDate = appointment.AppointmentTime.Date;
            tpAppointmentTime.SelectedTime = appointment.AppointmentTime;
            txtNotes.Text                  = appointment.AppointmentNotes;

            btnAdd.IsEnabled   = false;
            btnAdd.Visibility  = Visibility.Collapsed;
        }

        // ─── Q1k: Ajouter un nouveau appointment ──────────────────────────────────
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs()) return;

            Appointment newAppt = new Appointment
            {
                PatientId        = _patientId,
                AppointmentTime  = BuildDateTime(),
                AppointmentNotes = txtNotes.Text.Trim()
            };

            _db.Appointments.Add(newAppt);
            _db.SaveChanges();

            MessageBox.Show("Appointment added successfully.", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        // ─── Q1m: Modifier un appointment existant ────────────────────────────────
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs() || _existingAppointment == null) return;

            // EF Core: Find() pour récupérer l'entité trackée
            Appointment appt = _db.Appointments.Find(_existingAppointment.AppointmentId);
            if (appt == null)
            {
                MessageBox.Show("Appointment not found.", "Error");
                return;
            }

            appt.AppointmentTime  = BuildDateTime();
            appt.AppointmentNotes = txtNotes.Text.Trim();

            _db.SaveChanges();

            MessageBox.Show("Appointment updated successfully.", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        // ─── Helpers ──────────────────────────────────────────────────────────────
        private bool ValidateInputs()
        {
            if (dpAppointmentDate.SelectedDate == null)
            {
                MessageBox.Show("Please select an appointment date.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private DateTime BuildDateTime()
        {
            DateTime date = dpAppointmentDate.SelectedDate.Value.Date;  // .Value au lieu de !.Value

            if (tpAppointmentTime.SelectedTime.HasValue)
            {
                DateTime t = tpAppointmentTime.SelectedTime.Value;
                return new DateTime(date.Year, date.Month, date.Day, t.Hour, t.Minute, 0);
            }
            return date;
        }
    }
}
