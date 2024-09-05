using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace Login_SignUp
{
    public partial class ForgotPasswordWindow : Window
    {
        public ForgotPasswordWindow()
        {
            InitializeComponent();
        }

        private void SendResetLinkButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter your email address.");
                return;
            }

            string token = GenerateResetToken();
            if (StoreResetToken(email, token))
            {
                SendResetEmail(email, token);
                MessageBox.Show("Password reset link has been sent to your email.");
            }
            else
            {
                MessageBox.Show("Email address not found.");
            }
        }

        // Other methods as previously written

        private string GenerateResetToken()
        {
            // Generate a unique token
            return Guid.NewGuid().ToString();
        }

        private bool StoreResetToken(string email, string token)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["UserDBConnectionString"].ConnectionString;
            string query = "UPDATE Users SET ResetToken = @Token, ResetTokenExpiry = @Expiry WHERE Email = @Email";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Token", token);
                cmd.Parameters.AddWithValue("@Expiry", DateTime.Now.AddHours(1)); // Token expires in 1 hour
                cmd.Parameters.AddWithValue("@Email", email);

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
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window when the button is clicked
        }

        private void SendResetEmail(string email, string token)
        {
            string resetLink = $"http://yourwebsite.com/resetpassword?token={token}";
            string subject = "Password Reset Request";
            string body = $"Please click the following link to reset your password: {resetLink}";

            MailMessage mail = new MailMessage("legendusertesting@gmail.com", email)
            {
                Subject = subject,
                Body = body
            };

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("legendusertesting@gmail.com", "your-app-password"), // Use the App Password here
                EnableSsl = true,
            };

            try
            {
                smtpClient.Send(mail);
            }
            catch (SmtpException smtpEx)
            {
                MessageBox.Show($"SMTP Error: {smtpEx.StatusCode} - {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send email: {ex.Message}");
            }
        }

    }
}
