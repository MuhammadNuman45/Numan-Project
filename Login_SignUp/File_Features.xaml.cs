using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Login_SignUp
{
    public partial class File_Features : Window
    {
        private int _currentPage = 1;
        private int _pageSize = 10;
        private ObservableCollection<PatientFile> _patientFiles;

        public File_Features()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            LoadPatientFiles();
        }

        private void LoadPatientFiles()
        {
            // Calculate the offset for pagination
            int offset = (_currentPage - 1) * _pageSize;

            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Patients ORDER BY SerialNumber OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Offset", offset);
                command.Parameters.AddWithValue("@PageSize", _pageSize);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                PatientFilesDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private IEnumerable<PatientFile> GetPatientFilesFromDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            string query = "SELECT * FROM Patients";

            var patientFiles = new List<PatientFile>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                int serialNumber = 1; // Initialize serial number

                while (reader.Read())
                {
                    var patientFile = new PatientFile
                    {
                        SerialNumber = serialNumber.ToString(), // Set the serial number
                        UniquePatientID = reader["UniquePatientID"].ToString(),
                        Name = reader["Name"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        FileName = reader["FileName"].ToString(),
                        FileType = reader["FileType"].ToString(),
                        FileNumber = reader["FileNumber"].ToString(),
                        FileStatus = reader["FileStatus"].ToString()
                        // Add other fields as needed
                    };
                    patientFiles.Add(patientFile);

                    serialNumber++; // Increment serial number
                }
            }
            return patientFiles;
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadPatientFiles();
            }
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPatientFiles();
        }


        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            // You might want to check if there are more pages available
            _currentPage++;
            LoadPatientFiles();
        }

        private void AddPatientButton_Click(object sender, RoutedEventArgs e)
        {
            Patient_Register patient_Register = new Patient_Register();
            patient_Register.Show();
        }

        private void SearchPatientButton_Click(object sender, RoutedEventArgs e)
        {
            string mrn = MRNTextBox.Text.Trim();
            string patientID = PatientIDTextBox.Text.Trim();
            string phoneNumber = PhoneNumberTextBox.Text.Trim();

            var queryBuilder = new StringBuilder("SELECT * FROM Patients WHERE 1=1");
            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(mrn))
            {
                queryBuilder.Append(" AND FileNumber LIKE @MRN");
                parameters.Add(new SqlParameter("@MRN", $"%{mrn}%"));
            }
            if (!string.IsNullOrWhiteSpace(patientID))
            {
                queryBuilder.Append(" AND UniquePatientID LIKE @PatientID");
                parameters.Add(new SqlParameter("@PatientID", $"%{patientID}%"));
            }
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                queryBuilder.Append(" AND PhoneNumber LIKE @PhoneNumber");
                parameters.Add(new SqlParameter("@PhoneNumber", $"%{phoneNumber}%"));
            }

            string query = queryBuilder.ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddRange(parameters.ToArray());

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    SearchResultsWindow resultsWindow = new SearchResultsWindow(dt);
                    resultsWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching for patients: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            UploadWindow uploadWindow = new UploadWindow();
            uploadWindow.ShowDialog();
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

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientFilesDataGrid.SelectedItem is PatientFile selectedPatient)
            {
                // Open UpdatePatientWindow with selected patient's details
                UpdatePatientWindow updateWindow = new UpdatePatientWindow(
                    selectedPatient.UniquePatientID,
                    selectedPatient.Name,
                    selectedPatient.PhoneNumber,
                    selectedPatient.FileNumber,
                    selectedPatient.FileName,
                    selectedPatient.FileType
                );

                bool? result = updateWindow.ShowDialog();

                // Check if the update was successful
                if (result == true)
                {
                    // Update the selected item in the DataGrid
                    selectedPatient.Name = updateWindow.UpdatedPatient.Name;
                    selectedPatient.PhoneNumber = updateWindow.UpdatedPatient.PhoneNumber;
                    selectedPatient.FileNumber = updateWindow.UpdatedPatient.FileNumber;
                    selectedPatient.FileName = updateWindow.UpdatedPatient.FileName;
                    selectedPatient.FileType = updateWindow.UpdatedPatient.FileType;

                    // Refresh the DataGrid to reflect changes
                    PatientFilesDataGrid.Items.Refresh();
                }
            }
        }


        private void OpenAddPatientFileWindow_Click(object sender, RoutedEventArgs e)
        {
            AddPatientFileWindow addPatientFileWindow = new AddPatientFileWindow(RefreshPatientFiles);
            addPatientFileWindow.ShowDialog(); // Use ShowDialog to block until the window is closed
        }

        public void RefreshPatientFiles()
        {
            LoadPatientFiles(); // Reloads data from the database
        }

        private void UpdatePatientFilePath(string patientId, string filePath)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            string query = "UPDATE Patients SET ScanFilePath = @FilePath WHERE UniquePatientID = @PatientID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PatientID", patientId);
                cmd.Parameters.AddWithValue("@FilePath", filePath);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Successfully updated
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }

    public class PatientFile
    {
        public string SerialNumber { get; set; }
        public string UniquePatientID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string FileNumber { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileData { get; set; }
        public string FileStatus { get; set; }
    }
}
