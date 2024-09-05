using System.Windows;
using System.Windows.Controls;

namespace Login_SignUp
{
    public partial class UpdatePatientDetailsWindow : Window
    {
        public PatientFile Patient { get; set; }

        public UpdatePatientDetailsWindow(PatientFile patient)
        {
            InitializeComponent();

            // Initialize the fields with the patient's current details
            Patient = patient;
            PatientNameTextBox.Text = patient.Name;
            MRNTextBox.Text = patient.FileNumber;
            PhoneNumberTextBox.Text = patient.PhoneNumber;
            FileTypeComboBox.SelectedItem = patient.FileType;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Update the Patient object with the new values
            Patient.Name = PatientNameTextBox.Text;
            Patient.SerialNumber = MRNTextBox.Text;
            Patient.PhoneNumber = PhoneNumberTextBox.Text;
            Patient.FileType = (FileTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Perform validation if needed

            // Return the updated patient data
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Simply close the window
            DialogResult = false;
            Close();
        }
    }
}
