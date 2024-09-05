using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Login_SignUp
{
    public partial class ResetPasswordWindow : Window
    {
        private string _token;

        public ResetPasswordWindow(string token)
        {
            InitializeComponent();
            _token = token;
        }

        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            if (UpdatePassword(_token, newPassword))
            {
                MessageBox.Show("Password has been reset successfully.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Error resetting password.");
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window when the button is clicked
        }
        private bool UpdatePassword(string token, string newPassword)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            string query = "UPDATE Users SET PasswordHash = @PasswordHash, ResetToken = NULL, ResetTokenExpiry = NULL WHERE ResetToken = @Token AND ResetTokenExpiry > @Now";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(newPassword));
                cmd.Parameters.AddWithValue("@Token", token);
                cmd.Parameters.AddWithValue("@Now", DateTime.Now);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                    return false;
                }
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
