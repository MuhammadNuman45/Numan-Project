using System;
using System.Configuration;
using System.Data.SqlClient;

public static class DatabaseHelper
{
    private static string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;

    public static void SaveFileData(int patientID, string patientName, string fileName, string fileType, byte[] fileData)
    {
        // Ensure the fileData is not null
        if (fileData == null)
        {
            throw new ArgumentNullException(nameof(fileData), "File data cannot be null.");
        }

        // Use a try-catch block for error handling
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Patients (UniquePatientID, Name, FileName, FileType, FileData) " +
                               "VALUES (@UniquePatientID, @Name, @FileName, @FileType, @FileData)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters with explicit types
                    command.Parameters.AddWithValue("@UniquePatientID", patientID);
                    command.Parameters.AddWithValue("@Name", patientName);
                    command.Parameters.AddWithValue("@FileName", fileName);
                    command.Parameters.AddWithValue("@FileType", fileType);
                    command.Parameters.Add("@FileData", System.Data.SqlDbType.VarBinary).Value = fileData;

                    // Execute the command
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException sqlEx)
        {
            // Handle SQL exceptions (e.g., connection issues, syntax errors)
            Console.WriteLine($"SQL Error: {sqlEx.Message}");
        }
        catch (Exception ex)
        {
            // Handle general exceptions
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
