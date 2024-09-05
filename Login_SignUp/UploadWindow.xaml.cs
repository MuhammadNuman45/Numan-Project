using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace Login_SignUp
{
    public partial class UploadWindow : Window
    {

        // Properties to hold the file details
        public string SelectedFileName { get; private set; }
        public string SelectedFileType { get; private set; }
        public byte[] SelectedFileData { get; private set; }

        public UploadWindow()
        {
            InitializeComponent();
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "All files (*.*)|*.*",
                Title = "Select a File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedFileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                SelectedFileType = System.IO.Path.GetExtension(openFileDialog.FileName).TrimStart('.');
                SelectedFilePathTextBox.Text = openFileDialog.FileName;

                // Read file data into a byte array
                SelectedFileData = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate the manual file name input

            string manualFileName = SelectedFilePathTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(manualFileName))
            {
                MessageBox.Show("Please enter a file name.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update the properties with the manual file name if provided
            if (string.IsNullOrWhiteSpace(SelectedFileName))
            {
                SelectedFileName = manualFileName;
            }
            else
            {
                // If the file was selected, use the selected file's name
                SelectedFileName = manualFileName;
            }

            // Validate file type selection
            if (!InPatientRadioButton.IsChecked.GetValueOrDefault() && !OutPatientRadioButton.IsChecked.GetValueOrDefault())
            {
                MessageBox.Show("Please select a file type.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SelectedFileType = InPatientRadioButton.IsChecked.GetValueOrDefault() ? "In Patient" : "Out Patient";

            // Set dialog result to true to indicate successful saving
            DialogResult = true;
            Close();
        }
    }
}
