using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using OfficeOpenXml; // Ensure EPPlus is installed via NuGet

namespace Login_SignUp
{
    public partial class Patient_Register : Window
    {
        private const int PageSize = 10;
        private int _currentPage = 1;
        private int _totalPages;

      
        public ObservableCollection<Patient> Patients { get; set; }

        public Patient_Register()

        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
            LoadData();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Patients = new ObservableCollection<Patient>();
            PatientDataGrid.ItemsSource = Patients; // Ensure DataGrid is added to XAML
            LoadPatients(); // Load patients on initialization
        }
        private void LoadData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            string query = "SELECT COUNT(*) FROM Patients";

            // Get total number of records
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                int totalRecords = (int)cmd.ExecuteScalar();
                _totalPages = (int)Math.Ceiling((double)totalRecords / PageSize);
            }

            // Load the data for the current page
            LoadPageData();
        }

        private void LoadPageData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            string query = @"
    SELECT * FROM (
        SELECT ROW_NUMBER() OVER (ORDER BY SerialNumber) AS RowNum, *
        FROM Patients
    ) AS PatientsWithRowNum
    WHERE RowNum >= @StartRow AND RowNum < @EndRow
    ORDER BY SerialNumber ASC"; // Ensure sorting by SerialNumber

            using (SqlConnection conn = new SqlConnection(connectionString))
            {






                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@StartRow", (_currentPage - 1) * PageSize + 1);
                adapter.SelectCommand.Parameters.AddWithValue("@EndRow", _currentPage * PageSize + 1);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                PatientDataGrid.ItemsSource = dataTable.DefaultView;

                // Update pagination info
                PageInfoTextBlock.Text = $"Page {_currentPage} of {_totalPages}";

                // Enable/Disable buttons based on the current page
                PreviousButton.IsEnabled = _currentPage > 1;
                NextButton.IsEnabled = _currentPage < _totalPages;
            }
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;
            if (textBox != null && textBox.Tag != null)
            {
                textBox.Text = textBox.Text.Trim();
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;
            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
            }
        }
            private void PreviousButton_Click(object sender, RoutedEventArgs e)
{
    if (_currentPage > 1)
    {
        _currentPage--;
        LoadPageData();
    }
}

private void NextButton_Click(object sender, RoutedEventArgs e)
{
    if (_currentPage < _totalPages)
    {
        _currentPage++;
        LoadPageData();
    }
}

        private void CloseDetailsGrid_Click(object sender, RoutedEventArgs e)
        {
            // Show a confirmation message box
           
            

            // Check the result of the message box
           
                // Open the File_Features window
                File_Features file_Features = new File_Features();
                file_Features.Show();

                // Close the current window
                this.Close();
            }
            // No action needed if the user selects "No"
        


        private void DownloadTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Template");

                // Define the template columns with SerialNumber first
                worksheet.Cells["A1"].Value = "SerialNumber";
                worksheet.Cells["B1"].Value = "Name";
                worksheet.Cells["C1"].Value = "UniquePatientID";
                worksheet.Cells["D1"].Value = "PhoneNumber";
                worksheet.Cells["E1"].Value = "FileNumber";

                // AutoFit columns for better readability
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Define the file path
                var filePath = System.IO.Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
                    "PatientTemplate.xlsx");

                try
                {
                    // Delete the file if it already exists
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    // Save the Excel package
                    System.IO.File.WriteAllBytes(filePath, package.GetAsByteArray());
                    MessageBox.Show($"Template downloaded successfully at {filePath}");
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Error accessing file: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unexpected error: {ex.Message}");
                }
            }
        }






      
        private void SerialNumberButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var patient = button?.DataContext as Patient;

            if (patient != null)
            {
                // Define the template columns
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Name");
                dataTable.Columns.Add("UniquePatientID");
                dataTable.Columns.Add("PhoneNumber");
                dataTable.Columns.Add("FileNumber");

                // Add data to DataTable based on selected patient
                dataTable.Rows.Add(patient.Name, patient.UniquePatientID, patient.PhoneNumber, patient.FileNumber);

                // Create a new Excel package
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Template");
                    worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                    // Save the file to desktop
                    var filePath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), $"PatientTemplate_{patient.SerialNumber}.xlsx");
                    System.IO.File.WriteAllBytes(filePath, package.GetAsByteArray());

                    MessageBox.Show($"Template downloaded successfully at {filePath}");
                }
            }
        }


        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string patientName = PatientNameTextBox.Text.Trim();
            string patientId = PatientIDTextBox.Text.Trim();
            string phoneNumber = PatientPhoneNumberTextBox.Text.Trim();
            string fileNumber = PatientFileNumberTextBox.Text.Trim();
            //string fileName = FileNameTextBox.Text.Trim();
            string fileType = (FileTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Validate input
            if (string.IsNullOrWhiteSpace(patientName) ||
                string.IsNullOrWhiteSpace(patientId) ||
                string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(fileNumber) ||
                //string.IsNullOrWhiteSpace(fileName) ||
                string.IsNullOrWhiteSpace(fileType))
            {
                MessageBox.Show("All fields must be filled.");
                return;
            }

            // Database connection
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            string query = @"INSERT INTO Patients (Name, UniquePatientID, PhoneNumber, FileNumber,  FileType)
                     VALUES (@Name, @UniquePatientID, @PhoneNumber, @FileNumber,  @FileType)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", patientName);
                cmd.Parameters.AddWithValue("@UniquePatientID", patientId);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@FileNumber", fileNumber);
                //cmd.Parameters.AddWithValue("@FileName", fileName);
                cmd.Parameters.AddWithValue("@FileType", fileType);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Patient registered successfully.");
                    // Optionally refresh the DataGrid here

                    RefreshDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
        private void RefreshDataGrid()
        {
            LoadPatients(); // Reload the data into the DataGrid
            LoadData(); // Reload pagination information
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var patient = button?.DataContext as Patient;

            if (patient != null)
            {
                // Populate the text fields with the selected patient data
                PatientNameTextBox.Text = patient.Name;
                PatientIDTextBox.Text = patient.UniquePatientID;
                PatientPhoneNumberTextBox.Text = patient.PhoneNumber;
                PatientFileNumberTextBox.Text = patient.FileNumber;

                // Ensure there's only one confirm button
                StackPanelButtons.Children.Clear();

                // Add the confirm button
                var confirmButton = new Button
                {
                    Content = "Confirm Update",
                    Margin = new Thickness(5)
                };
                confirmButton.Click += ConfirmUpdate_Click;
                StackPanelButtons.Children.Add(confirmButton);
            }
        }


        private void ConfirmUpdate_Click(object sender, RoutedEventArgs e)
        {
            string updateQuery = "UPDATE Patients SET Name = @Name, PhoneNumber = @PhoneNumber, FileNumber = @FileNumber WHERE UniquePatientID = @PatientID";

            SavePatientData(updateQuery);

            // Refresh the DataGrid
            LoadPatients();

            // Remove the confirm button after use
            StackPanelButtons.Children.Clear();
        }

        private void SavePatientData(string query)
        {
            string name = PatientNameTextBox.Text;
            string patientId = PatientIDTextBox.Text;
            string phoneNumber = PatientPhoneNumberTextBox.Text;
            string fileNumber = PatientFileNumberTextBox.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(patientId) ||
                string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(fileNumber))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@PatientID", patientId);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@FileNumber", fileNumber);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Operation successful.");

                    // Clear input fields after operation
                    PatientNameTextBox.Clear();
                    PatientIDTextBox.Clear();
                    PatientPhoneNumberTextBox.Clear();
                    PatientFileNumberTextBox.Clear();
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var patient = button?.DataContext as Patient;

            if (patient != null)
            {
                var patientId = patient.UniquePatientID;
                if (string.IsNullOrWhiteSpace(patientId))
                {
                    MessageBox.Show("Patient ID is missing.");
                    return;
                }

                string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
                string query = "DELETE FROM Patients WHERE UniquePatientID = @PatientID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PatientID", patientId);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Patient deleted successfully.");
                            LoadPatients(); // Refresh the DataGrid
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

        private void ExportToExcelButton_Click(object sender, RoutedEventArgs e)
        {
            // Export patient data to an Excel file
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            string query = "SELECT SerialNumber, Name, UniquePatientID, PhoneNumber, FileNumber FROM Patients";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Patients");
                    worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                    var filePath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "PatientsData.xlsx");

                    // Ensure file is not already in use
                    if (System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            System.IO.File.Delete(filePath); // Delete the file if it exists
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show($"File is in use and cannot be deleted. Please close any applications that might be using the file. Error: {ex.Message}");
                            return;
                        }
                    }

                    try
                    {
                        System.IO.File.WriteAllBytes(filePath, package.GetAsByteArray());
                        MessageBox.Show($"Data exported to Excel successfully at {filePath}");
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"Error writing to file: {ex.Message}");
                    }
                }
            }
        }



        private void LoadPatients()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            string query = "SELECT Name, UniquePatientID, PhoneNumber, FileNumber FROM Patients";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Patients.Clear(); // Clear existing items
                    int serialNumber = 1; // Start serial numbers from 1

                    while (reader.Read())
                    {
                        Patients.Add(new Patient
                        {
                            SerialNumber = serialNumber++, // Assign serial number
                            Name = reader["Name"].ToString(),
                            UniquePatientID = reader["UniquePatientID"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            FileNumber = reader["FileNumber"].ToString()
                        });
                    }
                }
            }
        }


        private void UploadTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a file dialog to select the Excel file
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Select an Excel File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                // Process the selected Excel file
                try
                {
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        // Load the first worksheet
                        var worksheet = package.Workbook.Worksheets[0];

                        // Iterate through the rows in the worksheet
                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {
                            var name = worksheet.Cells[row, 2].Text;
                            var patientId = worksheet.Cells[row, 3].Text;
                            var phoneNumber = worksheet.Cells[row, 4].Text;
                            var fileNumber = worksheet.Cells[row, 5].Text;

                            // Insert or update patient data
                            SavePatientData("INSERT INTO Patients (Name, UniquePatientID, PhoneNumber, FileNumber) VALUES (@Name, @PatientID, @PhoneNumber, @FileNumber)",
                                            name, patientId, phoneNumber, fileNumber);
                        }

                        MessageBox.Show("Data uploaded and saved successfully.");
                        LoadPatients(); // Refresh the DataGrid
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing file: {ex.Message}");
                }
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            PatientNameTextBox.Clear();
            PatientIDTextBox.Clear();
            PatientPhoneNumberTextBox.Clear();
            PatientFileNumberTextBox.Clear();

            // Reload patients from the database
            
            LoadPatients(); // Reload patients from the database
            LoadData(); // Reload pagination information
        }



        private void SavePatientData(string query, string name = null, string patientId = null, string phoneNumber = null, string fileNumber = null)
        {
            // If parameters are provided, use them; otherwise, use text box values
            name = name ?? PatientNameTextBox.Text;
            patientId = patientId ?? PatientIDTextBox.Text;
            phoneNumber = phoneNumber ?? PatientPhoneNumberTextBox.Text;
            fileNumber = fileNumber ?? PatientFileNumberTextBox.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(patientId) ||
                string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(fileNumber))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@PatientID", patientId);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@FileNumber", fileNumber);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Operation successful.");

                    // Clear input fields after operation
                    PatientNameTextBox.Clear();
                    PatientIDTextBox.Clear();
                    PatientPhoneNumberTextBox.Clear();
                    PatientFileNumberTextBox.Clear();
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

        private void PatientDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PatientIDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
