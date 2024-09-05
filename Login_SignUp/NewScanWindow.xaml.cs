using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace Login_SignUp
{
    public partial class NewScanWindow : Window
    {
        private string _selectedFilePath;
        private readonly PatientFile _patientFile;
        private bool _isFileSelected = false;
        private bool _isRadioButtonSelected = false;

        public NewScanWindow(PatientFile patientFile)
        {
            InitializeComponent();
            _patientFile = patientFile;
            PatientIdTextBox.Text = patientFile.UniquePatientID;
            // Populate other fields as needed
        }

        private void ScanDetailsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ScanDetailsPlaceholder.Visibility = string.IsNullOrWhiteSpace(ScanDetailsTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|PDF Files (*.pdf)|*.pdf",
                Title = "Select a File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedFilePath = openFileDialog.FileName;
                SelectedFilePathTextBox.Text = _selectedFilePath;
                _isFileSelected = true;
                UpdateSaveButtonState();
            }
        }

        // This method should be connected to each RadioButton's Checked event
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _isRadioButtonSelected = true;
            UpdateSaveButtonState();
        }

        private void SaveScanButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isFileSelected || !_isRadioButtonSelected)
            {
                MessageBox.Show("Please select a file and a radio button before saving.");
                return;
            }

            string destinationDirectory = @"C:\Scans"; // Adjust path if needed
            string destinationFilePath = Path.Combine(destinationDirectory, Path.GetFileName(_selectedFilePath));

            try
            {
                Directory.CreateDirectory(destinationDirectory);
                File.Copy(_selectedFilePath, destinationFilePath, true);

                // Save file path to the database
                SaveFilePathToDatabase(_patientFile.UniquePatientID, destinationFilePath);

                MessageBox.Show("File saved successfully.");
                this.Close(); // Close the window after saving
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}");
            }
        }

        private void SaveFilePathToDatabase(string patientId, string filePath)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            string query = "UPDATE Patients SET FileName = @FileName WHERE UniquePatientID = @PatientID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FileName", filePath);
                cmd.Parameters.AddWithValue("@PatientID", patientId);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("No records updated. Please check the Patient ID and try again.", "Update Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show($"SQL Error: {sqlEx.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window when the button is clicked
        }

        private void UpdateSaveButtonState()
        {
            // Enable SaveScanButton only if both a file is selected and a radio button is checked
            SaveScanButton.IsEnabled = _isFileSelected && _isRadioButtonSelected;
        }
    }
}
