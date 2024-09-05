using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Login_SignUp
{
    public partial class SearchResultsWindow : Window
    {
        public SearchResultsWindow(DataTable searchResults)
        {
            InitializeComponent();
            SearchResultsDataGrid.ItemsSource = searchResults.DefaultView;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
           
            // Handle the update button click event here
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle the delete button click event here
        }


        private void SearchResultsDataGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SearchResultsDataGrid.SelectedItem != null)
            {
                DataRowView rowView = SearchResultsDataGrid.SelectedItem as DataRowView;
                if (rowView != null)
                {
                    PatientFile selectedPatient = new PatientFile
                    {
                        FileNumber = rowView["FileNumber"].ToString(),
                        UniquePatientID = rowView["UniquePatientID"].ToString(),
                        PhoneNumber = rowView["PhoneNumber"].ToString(),
                        Name = rowView["Name"].ToString(),
                        FileName = rowView["FileName"].ToString(),
                        FileType = rowView["FileType"].ToString(),
                    };

                    // Convert List<PatientFile> to ObservableCollection<PatientFile>
                    ObservableCollection<PatientFile> patientFileCollection = new ObservableCollection<PatientFile> { selectedPatient };

                    // Pass ObservableCollection<PatientFile> to AddPatientFileWindow
                    AddPatientFileWindow addPatientFileWindow = new AddPatientFileWindow(patientFileCollection);
                    addPatientFileWindow.Show();
                    this.Close(); // Optionally close the SearchResultsWindow
                }
            }
        }

    }
}
