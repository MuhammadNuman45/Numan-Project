using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace Login_SignUp
{
    public partial class AddPatientFileWindow : Window
    {
        private ObservableCollection<PatientFile> _patientFiles;
        private Action _refreshCallback;

        // Constructor for adding patient files
        public AddPatientFileWindow(ObservableCollection<PatientFile> patientFiles)
        {
            InitializeComponent();
            _patientFiles = patientFiles;
            SetSerialNumbers(_patientFiles.ToList()); 
           PatientFilesDataGrid.ItemsSource = _patientFiles;
              this.WindowState = WindowState.Maximized;
        }

        // Constructor for editing patient files
        public AddPatientFileWindow(Action refreshCallback)
        {
            InitializeComponent();
            _refreshCallback = refreshCallback;
            _patientFiles = new ObservableCollection<PatientFile>();
            PatientFilesDataGrid.ItemsSource = _patientFiles;
        }

        // Handle the window closed event to call the refresh callback
        private void AddPatientFileWindow_Closed(object sender, EventArgs e)
        {
            _refreshCallback?.Invoke();
        }

        private void PatientFilesDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (PatientFilesDataGrid.Items.Count > 0)
            {
                PatientFilesDataGrid.SelectedIndex = 0;
                //UpdateDisplayWithSelectedPatient();
            }
        }
        private void PatientFileDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDisplayWithSelectedPatient();
        }

        private void UpdateDisplayWithSelectedPatient()
        {
            var selectedPatient = PatientFilesDataGrid.SelectedItem as PatientFile;
            if (selectedPatient != null)
            {
                // Update the UI elements with the selected patient's data
                MRNDisplayTextBlock.Text = selectedPatient.FileNumber;
                NameDisplayTextBlock.Text = selectedPatient.Name;
                PatientIDDisplayTextBlock.Text = selectedPatient.UniquePatientID;
                PhoneNumberDisplayTextBlock.Text = selectedPatient.PhoneNumber;
            }
        }
        private void PopulatePatientFilesDataGrid()
        {
            // Code to populate the DataGrid goes here
            // Example: PatientFilesDataGrid.ItemsSource = GetPatientFilesFromDatabase();
        }
        private void UpdatePatientDataGrid()
        {
            // Assuming you have a method to populate the DataGrid
            PopulatePatientFilesDataGrid();

            // Automatically select the first item
            if (PatientFilesDataGrid.Items.Count > 0)
            {
                PatientFilesDataGrid.SelectedIndex = 0;
                UpdateDisplayWithSelectedPatient();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Select the first item in the DataGrid by default
            if (PatientFilesDataGrid.Items.Count > 0)
            {
                PatientFilesDataGrid.SelectedIndex = 0;
                UpdateDisplayWithSelectedPatient();
            }
        }
        //private void UpdateDisplayWithSelectedPatient()
        //{
        //    var selectedPatient = PatientFilesDataGrid.SelectedItem as PatientFile;
        //    if (selectedPatient != null)
        //    {
        //        MRNDisplayTextBlock.Text = selectedPatient.FileNumber;
        //        NameDisplayTextBlock.Text = selectedPatient.Name;
        //        PatientIDDisplayTextBlock.Text = selectedPatient.UniquePatientID;
        //        PhoneNumberDisplayTextBlock.Text = selectedPatient.PhoneNumber;
        //    }
        //}

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = PatientFilesDataGrid.SelectedItem as PatientFile;

            if (selectedItem != null)
            {
                string patientID = selectedItem.UniquePatientID;
                string patientName = selectedItem.Name;
                string fileName = selectedItem.FileName;
                string fileType = selectedItem.FileType;
                string filePath = GetFilePathFromDatabase(fileName);

                Action refreshDataGridCallback = () =>
                {
                    // Refresh logic if needed
                };

                ViewDataWindow viewDataWindow = new ViewDataWindow(patientID, patientName, fileName, fileType, filePath, refreshDataGridCallback);
                viewDataWindow.ShowDialog();
            }
        }

        private string GetFilePathFromDatabase(string fileName)
        {
            string directoryPath = @"C:\Users\bk\Downloads\";
            return Path.Combine(directoryPath, fileName);
        }

        private void CloseDetailsGrid_Click(object sender, RoutedEventArgs e)
        {
            var selectedPatient = PatientFilesDataGrid.SelectedItem as PatientFile;

            if (selectedPatient != null)
            {
                string uniquePatientId = selectedPatient.UniquePatientID;

                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to close the details?",
                    "Confirm Close",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    UpdateFileStatus("Complete", uniquePatientId);
                }
                else if (result == MessageBoxResult.No)
                {
                    UpdateFileStatus("Pending", uniquePatientId);
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a patient first.", "No Patient Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void UpdateFileStatus(string status, string uniquePatientId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Patients SET FileStatus = @status WHERE UniquePatientID = @uniquePatientId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@uniquePatientId", uniquePatientId);
                    command.ExecuteNonQuery();
                }
            }
        }

        //private void PatientFileDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    UpdateDisplayWithSelectedPatient();
        //}

        //private void UpdateDisplayWithSelectedPatient()
        //{
        //    var selectedPatient = PatientFilesDataGrid.SelectedItem as PatientFile;
        //    if (selectedPatient != null)
        //    {
        //        // Update the UI elements with the selected patient's data
        //        MRNDisplayTextBlock.Text = selectedPatient.FileNumber;
        //        NameDisplayTextBlock.Text = selectedPatient.Name;
        //        PatientIDDisplayTextBlock.Text = selectedPatient.UniquePatientID;
        //        PhoneNumberDisplayTextBlock.Text = selectedPatient.PhoneNumber;
        //    }
        //}

        private void NewScanButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPatient = (PatientFile)PatientFilesDataGrid.SelectedItem;

            if (selectedPatient != null)
            {
                NewScanWindow scanWindow = new NewScanWindow(selectedPatient);
                scanWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a patient first.", "No Patient Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPatient = (PatientFile)PatientFilesDataGrid.SelectedItem;

            if (selectedPatient != null)
            {
                UploadWindow uploadWindow = new UploadWindow();
                bool? result = uploadWindow.ShowDialog();

                if (result == true)
                {
                    string fileName = uploadWindow.SelectedFileName;
                    string fileType = uploadWindow.SelectedFileType;
                    byte[] fileData = uploadWindow.SelectedFileData;

                    SaveFileToDatabase(selectedPatient, fileName, fileType, fileData);

                    MessageBox.Show($"File: {fileName}\nType: {fileType}",
                                    "File Uploaded", MessageBoxButton.OK, MessageBoxImage.Information);

                    selectedPatient.FileName = fileName;
                    selectedPatient.FileType = fileType;
                    PatientFilesDataGrid.Items.Refresh();

                    UpdatePatientWindow updateWindow = new UpdatePatientWindow(
                        selectedPatient.SerialNumber,
                        selectedPatient.Name,
                        selectedPatient.UniquePatientID,
                        selectedPatient.PhoneNumber,
                        selectedPatient.FileName,
                        selectedPatient.FileType
                    );
                }
            }
            else
            {
                MessageBox.Show("Please select a patient first.", "No Patient Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveFileToDatabase(PatientFile patient, string fileName, string fileType, byte[] fileData)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "IF EXISTS (SELECT 1 FROM [dbo].[Patients] WHERE UniquePatientID = @UniquePatientID) " +
                                   "BEGIN " +
                                   "   UPDATE [dbo].[Patients] SET FileName = @FileName, FileType = @FileType, FileData = @FileData WHERE UniquePatientID = @UniquePatientID " +
                                   "END " +
                                   "ELSE " +
                                   "BEGIN " +
                                   "   INSERT INTO [dbo].[Patients] (FileName, FileType, FileData, UniquePatientID) VALUES (@FileName, @FileType, @FileData, @UniquePatientID) " +
                                   "END";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FileName", fileName);
                        command.Parameters.AddWithValue("@FileType", fileType);
                        command.Parameters.AddWithValue("@FileData", fileData);
                        command.Parameters.AddWithValue("@UniquePatientID", patient.UniquePatientID);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("No records updated or inserted. Please try again.", "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button updateButton)
            {
                if (FindVisualParent<DataGridRow>(updateButton) is DataGridRow row &&
                    row.DataContext is PatientFile patient)
                {
                    var uploadWindow = new UploadWindow();

                    // Open the UploadWindow as a dialog
                    if (uploadWindow.ShowDialog() == true)
                    {
                        // Handle any logic after the UploadWindow is closed
                        MessageBox.Show("File uploaded successfully.", "Upload Complete", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Optional: If you need to update the patient data grid or database, you can do that here
                        // RefreshPatientDataGrid(patient);
                    }
                    else
                    {
                        MessageBox.Show("File upload was cancelled.", "Upload Cancelled", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }


        private void RefreshPatientDataGrid(PatientFile updatedPatient)
        {
            SetSerialNumbers(_patientFiles.ToList());
            PatientFilesDataGrid.Items.Refresh();
        }

        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T parent)
                {
                    return parent;
                }
                child = VisualTreeHelper.GetParent(child);
            }
            return null;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = PatientFilesDataGrid.SelectedItem as PatientFile;

            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to delete this patient file?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    DeletePatientFileFromDatabase(selectedItem);

                    _patientFiles.Remove(selectedItem);
                    PatientFilesDataGrid.Items.Refresh();

                    MessageBox.Show("Patient file deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a patient file to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeletePatientFileFromDatabase(PatientFile patientFile)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM [dbo].[Patients] WHERE UniquePatientID = @UniquePatientID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UniquePatientID", patientFile.UniquePatientID);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("No records were deleted. Please try again.", "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetSerialNumbers(List<PatientFile> patientFiles)
        {
            int serial = 1;
            foreach (var patient in patientFiles)
            {
                patient.SerialNumber = serial.ToString(); // Convert serial number to string
                serial++;
            }
        }

        private void UpdatePatientInDatabase(PatientFile updatedPatient)
        {
            // Logic to update the patient in the database
        }
    }
}
