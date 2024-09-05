using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32; // For OpenFileDialog

namespace Login_SignUp
{
    public partial class ViewDataWindow : Window
    {
        private string filePath;
        private string patientName;
        private string fileName;
        private string fileType;
        private int patientID;
        private Action refreshDataGridCallback;

        public ViewDataWindow(string patientID, string patientName, string fileName, string fileType, string filePath, Action refreshDataGridCallback)
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

            // Initialize fields
            this.patientID = int.Parse(patientID);
            this.patientName = patientName;
            this.fileName = fileName;
            this.fileType = fileType;
            this.filePath = filePath;
            this.refreshDataGridCallback = refreshDataGridCallback;

            // Load and display the file
            LoadImage(filePath);
        }

        private void LoadImage(string filePath)
        {
            try
            {
                // Debugging: Show the file path being used
                Console.WriteLine($"Loading image from: {filePath}");

                // Check if file exists
                if (File.Exists(filePath))
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = fileStream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        ImageDisplay.Source = bitmap;
                    }
                }
                else
                {
                    MessageBox.Show($"File not found at: {filePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}\nFile path: {filePath}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void UploadFile_Click(object sender, RoutedEventArgs e)
        //{
        //    // Open file dialog to select a file
        //    OpenFileDialog openFileDialog = new OpenFileDialog
        //    {
        //        Filter = "Image Files (*.PNG;*.JPG;*.JPEG)|*.PNG;*.JPG;*.JPEG|All files (*.*)|*.*"
        //    };

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        filePath = openFileDialog.FileName;
        //        fileName = Path.GetFileName(filePath); // Update fileName
        //        fileType = Path.GetExtension(filePath).TrimStart('.').ToUpper(); // Update fileType
        //        LoadImage(filePath); // Display the selected image
        //    }
        //}

        //private void SaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(filePath))
        //    {
        //        // Convert the image to a byte array
        //        byte[] imageData = GetImageData(filePath);

        //        // Call method to save data to the database, including patientID
        //        bool success = SaveDataToDatabase(patientID, patientName, fileName, fileType, imageData);

        //        if (success)
        //        {
        //            MessageBox.Show("File data saved successfully!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

        //            // Notify parent window to refresh DataGrid
        //            refreshDataGridCallback?.Invoke();

        //            // Close the ViewDataWindow
        //            this.Close();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Failed to save file data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("No file selected to save.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void RescanButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Create a PatientFile object with relevant data
        //    PatientFile patientFile = new PatientFile
        //    {
        //        SerialNumber = "SampleSerialNumber", // Replace with actual values if available
        //        FileNumber = "SampleFileNumber",
        //        Name = patientName,
        //        UniquePatientID = patientID.ToString(), // Ensure patientID is properly converted to string
        //        PhoneNumber = "SamplePhoneNumber",
        //        FileName = fileName,
        //        FileType = fileType
        //    };

        //    // Pass the PatientFile object to NewScanWindow
        //    NewScanWindow newScanWindow = new NewScanWindow(patientFile);
        //    newScanWindow.Show();

        //    // Notify parent window to refresh DataGrid after rescanning
        //    refreshDataGridCallback?.Invoke();
        //}

        private byte[] GetImageData(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        private bool SaveDataToDatabase(int patientID, string patientName, string fileName, string fileType, byte[] imageData)
        {
            try
            {
                // Save data to the database
                DatabaseHelper.SaveFileData(patientID, patientName, fileName, fileType, imageData);
                return true; // Return true if save operation is successful
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false; // Return false if an error occurs
            }
        }
    }
}
