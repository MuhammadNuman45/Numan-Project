using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Login_SignUp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
        }

        private void CloseDetailsGrid_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Authenticate user
            if (AuthenticateUser(username, password))
            {
                // Credentials are valid
                File_Features fileFeaturesWindow = new File_Features();
                fileFeaturesWindow.Show();
                this.Close(); // Close the login window
            }
            else
            {
                // Invalid credentials
                MessageBox.Show("Invalid credentials. Please try again.");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                try
                {
                    conn.Open();
                    string storedHash = cmd.ExecuteScalar() as string;

                    if (storedHash != null)
                    {
                        // Verify the password
                        return VerifyPassword(password, storedHash);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            return false;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Hash the input password
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hash = Convert.ToBase64String(hashBytes);

                return hash == storedHash;
            }
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
            forgotPasswordWindow.Show();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.Show();
        }
    }
}
