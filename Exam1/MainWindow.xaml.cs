using System;
using System.Data.Entity;       // EF6 — NuGet: EntityFramework
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PatientApp.Data;
using PatientApp.Models;

namespace PatientApp
{
    public partial class MainWindow : Window
    {
        private readonly PatientData _db;

        public MainWindow()
        {
            InitializeComponent();
            _db = new PatientData();

            DatabaseScript.CreateAndSeedDatabase();
            LoadPatients();
            ResetPatientFields();
        }

        // ─── Q1i: Tous les patients triés par nom ─────────────────────────────────
        private void LoadPatients()
        {
            var patients = _db.Patients
                              .OrderBy(p => p.Surname)
                              .ThenBy(p => p.FirstName)
                              .ToList();

            lstPatients.ItemsSource = patients;
        }

        // ─── Q1i/l: Sélection → charge les appointments du patient ───────────────
        private void lstPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstPatients.SelectedItem is Patient selected)
                LoadAppointments(selected.PatientId);
        }

        // ─── Q1l: Appointments triés par date ────────────────────────────────────
        private void LoadAppointments(int patientId)
        {
            var appts = _db.Appointments
                           .Where(a => a.PatientId == patientId)
                           .OrderBy(a => a.AppointmentTime)
                           .ToList();

            if (appts.Count == 0)
                lstAppointments.ItemsSource = new[] { "No appointments" };
            else
                lstAppointments.ItemsSource = appts;
        }

        // ─── Q1j: Ajouter un nouveau patient ──────────────────────────────────────
        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string surname   = txtSurname.Text.Trim();
            string phone     = txtPhone.Text.Trim();

            if (firstName == (string)txtFirstName.Tag ||
                surname   == (string)txtSurname.Tag   ||
                string.IsNullOrWhiteSpace(firstName)  ||
                string.IsNullOrWhiteSpace(surname))
            {
                MessageBox.Show("Please enter at least a first name and surname.",
                                "Validation Error", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            Patient newPatient = new Patient
            {
                FirstName     = firstName,
                Surname       = surname,
                ContactNumber = phone == (string)txtPhone.Tag ? string.Empty : phone,
                DOB           = dpDOB.SelectedDate ?? DateTime.Now
            };

            _db.Patients.Add(newPatient);
            _db.SaveChanges();

            LoadPatients();
            ResetPatientFields();
        }

        // ─── Q1k: Ouvre AppointmentWindow — mode Ajout ───────────────────────────
        private void btnAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (lstPatients.SelectedItem is Patient selected)
            {
                new AppointmentWindow(selected.PatientId).ShowDialog();
                LoadAppointments(selected.PatientId);
            }
            else
                MessageBox.Show("Please select a patient first.", "No Patient Selected");
        }

        // ─── Q1m: Ouvre AppointmentWindow — mode Édition ─────────────────────────
        private void btnEditAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (lstPatients.SelectedItem  is Patient     pat  &&
                lstAppointments.SelectedItem is Appointment appt)
            {
                new AppointmentWindow(pat.PatientId, appt).ShowDialog();
                LoadAppointments(pat.PatientId);
            }
            else
                MessageBox.Show("Please select a patient and an appointment.",
                                "Selection Required");
        }

        // ─── Hint text ────────────────────────────────────────────────────────────
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == (string)tb.Tag)
                tb.Text = string.Empty;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
                tb.Text = (string)tb.Tag;
        }

        private void ResetPatientFields()
        {
            txtFirstName.Text  = (string)txtFirstName.Tag;
            txtSurname.Text    = (string)txtSurname.Tag;
            txtPhone.Text      = (string)txtPhone.Tag;
            dpDOB.SelectedDate = null;
        }
    }
}
