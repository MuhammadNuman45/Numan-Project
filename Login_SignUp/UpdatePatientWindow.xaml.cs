using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace Login_SignUp
{
    public partial class UpdatePatientWindow : Window
    {
        private string _patientId;
        public PatientFile UpdatedPatient { get; private set; }

        public UpdatePatientWindow(string patientId, string name, string phoneNumber, string fileNumber, string fileName, string fileType)
        {
            InitializeComponent();
            _patientId = patientId;
            SetPatientDetails(name, phoneNumber, fileNumber, fileName, fileType);
        }

        private void SetPatientDetails(string name, string phoneNumber, string fileNumber, string fileName, string fileType)
        {
            PatientIDTextBox.Text = _patientId;
            NameTextBox.Text = name;
            PhoneNumberTextBox.Text = phoneNumber;
            FileNumberTextBox.Text = fileNumber; // Ensure this is a valid property if needed
            FileNameTextBox.Text = fileName;

            // Set the selected value for FileTypeComboBox
            foreach (ComboBoxItem item in FileTypeComboBox.Items)
            {
                if (item.Content.ToString() == fileType)
                {
                    FileTypeComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve updated values from text boxes and combo box
            string name = NameTextBox.Text;
            string phoneNumber = PhoneNumberTextBox.Text;
            string fileNumber = FileNumberTextBox.Text; // Ensure this is handled correctly
            string fileName = FileNameTextBox.Text;
            string fileType = (FileTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(); // Get FileType

            // Validate input
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(fileNumber) ||
                string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(fileType))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Update patient details in the database
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            string query = @"
                UPDATE Patients
                SET Name = @Name,
                    PhoneNumber = @PhoneNumber,
                    FileNumber = @FileNumber,
                    FileName = @FileName,
                    FileType = @FileType
                WHERE UniquePatientID = @PatientID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@FileNumber", fileNumber); // Ensure this matches your database schema
                cmd.Parameters.AddWithValue("@FileName", fileName);
                cmd.Parameters.AddWithValue("@FileType", fileType);
                cmd.Parameters.AddWithValue("@PatientID", _patientId);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Set UpdatedPatient property
                        UpdatedPatient = new PatientFile
                        {
                            SerialNumber = _patientId, // Assuming SerialNumber is same as PatientID
                            Name = name,
                            UniquePatientID = _patientId,
                            PhoneNumber = phoneNumber,
                            FileName = fileName,
                            FileType = fileType
                        };

                        MessageBox.Show("Patient details updated successfully.");
                        this.DialogResult = true; // Close the window and indicate success
                    }
                    else
                    {
                        MessageBox.Show("No patient found with the given ID.");
                    }
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show($"SQL Error: {sqlEx.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}
