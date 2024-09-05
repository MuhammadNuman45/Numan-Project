using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Login_SignUp
{
    public partial class EditPatientFileWindow : Window
    {
        private PatientFile _patientFile;
        private Action _onSaveCallback; // Delegate for callback

        public EditPatientFileWindow(PatientFile patientFile, Action onSaveCallback)
        {
            InitializeComponent();
            _patientFile = patientFile;
            _onSaveCallback = onSaveCallback;
            InitializeFields(patientFile);
        }

        private void InitializeFields(PatientFile patientFile)
        {
            MRNTextBox.Text = patientFile.FileNumber;
            PatientNameTextBox.Text = patientFile.Name;
            PhoneNumberTextBox.Text = patientFile.PhoneNumber;

            // Ensure FileTypeComboBox is empty before setting ItemsSource
            FileTypeComboBox.Items.Clear();

            // Populate ComboBox with predefined items
            FileTypeComboBox.ItemsSource = new[] { "Inpatient", "Outpatient" };

            // Set the selected item
            FileTypeComboBox.SelectedItem = patientFile.FileType;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string mrn = MRNTextBox.Text;
            string patientName = PatientNameTextBox.Text;
            string phoneNumber = PhoneNumberTextBox.Text;
            string fileType = FileTypeComboBox.SelectedItem as string;

            if (_patientFile == null)
            {
                MessageBox.Show("Patient file is not available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string uniquePatientID = _patientFile.UniquePatientID;

            UpdatePatientFileInDatabase(uniquePatientID, mrn, patientName, phoneNumber, fileType);

            // Invoke the callback to notify changes
            _onSaveCallback?.Invoke();

            MessageBox.Show("Changes saved successfully.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void UpdatePatientFileInDatabase(string uniquePatientID, string fileNumber, string name, string phoneNumber, string fileType)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE [dbo].[Patients] SET Name = @Name, PhoneNumber = @PhoneNumber, FileType = @FileType, FileNumber = @FileNumber WHERE UniquePatientID = @UniquePatientID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@FileType", fileType);
                        command.Parameters.AddWithValue("@FileNumber", fileNumber);
                        command.Parameters.AddWithValue("@UniquePatientID", uniquePatientID);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the patient file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
