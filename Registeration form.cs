/*using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sign_in_Application
{
    public partial class Registeration_form : Form
    {
        public Registeration_form()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        /*{
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-SQLM2FG\SQLEXPRESS;Initial Catalog=Bakehouse;Integrated Security=True");
            SqlCommand cmd = new SqlCommand(@"INSERT INTO [dbo].[Register]
           ([Name]
           ,[Email]
           ,[CNIC]
           ,[City]
           ,[Username]
           ,[Password]
           ,[ConfirmPassword])*/
/*
{
    try
    {
                string connectionString = @"Data Source=DESKTOP-SQLM2FG\SQLEXPRESS;Initial Catalog=Bakehouse;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "INSERT INTO [dbo].[Register] " +
                                   "([Name], [Email], [CNIC], [City], [Username], [Password], [ConfirmPassword]) " +
                                   "VALUES (@Name, @Email, @CNIC, @City, @Username, @Password, @ConfirmPassword)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = txtName.Text;
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;
                        cmd.Parameters.Add("@CNIC", SqlDbType.NVarChar).Value = txtCnic.Text;
                        cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = txtCity.Text;
                        cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = txtUser.Text;
                        cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = txtPass.Text;
                        cmd.Parameters.Add("@ConfirmPassword", SqlDbType.NVarChar).Value = txtConfpass.Text;

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data has been successfully inserted.");
                        }
                        else
                        {
                            MessageBox.Show("Data insertion failed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            VALUES
            ('" + txtName.Text + "','" + txtEmail.Text + "','" + txtCnic.Text + "','" + txtCity.Text + "','" + txtUser.Text + "','" + txtPass.Text + "','" + txtConfpass.Text + "')", con);
                            con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Register");

                    }
                }
            }
            
        }
    }
}

using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Sign_in_Application
{
    public partial class Registeration_form : Form
    {
        public Registeration_form()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (textCnic.Text != "" && txtEmail.Text != "" && txtPass.Text != "" && txtUser.Text != "")
            {
                try
                {
                    string connectionString = @"Data Source=DESKTOP-SQLM2FG\SQLEXPRESS;Initial Catalog=Bakehouse;Integrated Security=True";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string emailCheckQuery = "SELECT COUNT(*) FROM Register WHERE Email = @Email";
                        using (SqlCommand emailCheckCmd = new SqlCommand(emailCheckQuery, con))
                        {
                            emailCheckCmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;

                            int emailCount = (int)emailCheckCmd.ExecuteScalar();
                            if (emailCount > 0)
                            {
                                MessageBox.Show("This email is already registered. Please use a different email.");
                                return;
                            }
                        }
                        string query = "INSERT INTO [dbo].[Register] " +
                                       "([Name], [Email], [CNIC], [City], [Username], [Password], [ConfirmPassword]) " +
                                       "VALUES (@Name, @Email, @CNIC, @City, @Username, @Password, @ConfirmPassword)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = txtName.Text;
                            cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;
                            cmd.Parameters.Add("@CNIC", SqlDbType.NVarChar).Value = textCnic.Text;
                            cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = txtCity.Text;
                            cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = txtUser.Text;
                            cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = txtPass.Text;
                            cmd.Parameters.Add("@ConfirmPassword", SqlDbType.NVarChar).Value = txtConfpass.Text;

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Data has been successfully inserted.");
                            }
                            else
                            {
                                MessageBox.Show("Data insertion failed.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            else
            {
                MessageBox.Show("Please fill the required fields");
            }


            {
                if (textCnic.Text != "" && txtEmail.Text != "" && txtPass.Text != "" && txtUser.Text != "")
    {
        try
        {
            // Validate email format
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Invalid email format.");
                return;
            }

            // Validate CNIC format (assuming CNIC is a 13-character string with dashes)
            if (!IsValidCNIC(textCnic.Text))
            {
                MessageBox.Show("Invalid CNIC Format");
                return;
            }

            // Validate password complexity (at least 8 characters with letters and special characters)
            if (!IsValidPassword(txtPass.Text))
            {
                MessageBox.Show("Password must be at least 8 characters long and include letters and special characters.");
                return;
            }

            string connectionString = @"Data Source=DESKTOP-SQLM2FG\SQLEXPRESS;Initial Catalog=Bakehouse;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Check if email is already registered
                if (IsEmailAlreadyRegistered(con, txtEmail.Text))
                {
                    MessageBox.Show("This email is already registered. Please use a different email.");
                    return;
                }

                // Hash the password before storing it
                string hashedPassword = HashPassword(txtPass.Text);

                // Insert data into the database
                string query = "INSERT INTO [dbo].[Register] " +
                               "([Name], [Email], [CNIC], [City], [Username], [Password], [ConfirmPassword]) " +
                               "VALUES (@Name, @Email, @CNIC, @City, @Username, @Password, @ConfirmPassword)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = txtName.Text;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;
                    cmd.Parameters.Add("@CNIC", SqlDbType.NVarChar).Value = textCnic.Text;
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = txtCity.Text;
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = txtUser.Text;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = hashedPassword;
                    cmd.Parameters.Add("@ConfirmPassword", SqlDbType.NVarChar).Value = txtConfpass.Text;

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data has been successfully inserted.");
                    }
                    else
                    {
                        MessageBox.Show("Data insertion failed.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred: " + ex.Message);
        }
    }
    else
    {
        MessageBox.Show("Please fill the required fields");
    }
}


            // Validate email format
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Invalid email format.");
                return;
            }

            // Validate CNIC format (assuming CNIC is a 13-character string with dashes)
            if (!IsValidCNIC(textCnic.Text))
            {
                MessageBox.Show("Invalid CNIC Format");
                return;
            }

            // Validate password complexity (at least 8 characters with letters and special characters)
            if (!IsValidPassword(txtPass.Text))
            {
                MessageBox.Show("Password must be at least 8 characters long and include letters and special characters.");
                //"Password must be at least 8 characters long and include letters and special characters.");
                return;
            }

            // Proceed with database insertion
            
        }

        // Email format validation
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // CNIC format validation
        private bool IsValidCNIC(string cnic)
        {
            
            Console.Write(cnic);
            // Trim leading and trailing spaces from the CNIC string
            cnic = cnic.Trim();

            // Use the regular expression to validate the CNIC
            return Regex.IsMatch(cnic, @"^\d{5}-\d{7}-\d{1}$");
        }



        // Password complexity validation
        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$");
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            {
                DialogResult res;
                res = MessageBox.Show("Do you want to exit", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    Application.Exit();

                }
                else
                {
                    this.Show();
                }
            }
        }

        private void txtCnic_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCity_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPass_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void loginLbl_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void Required_Click(object sender, EventArgs e)
        {

        }
    }
}*/
using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace Sign_in_Application
{
    public partial class Registeration_form : Form
    {
        public Registeration_form()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (textCnic.Text != "" && txtEmail.Text != "" && txtPass.Text != "" && txtUser.Text != "")
            {
                try
                {
                    string connectionString = @"Data Source=DESKTOP-SQLM2FG\SQLEXPRESS;Initial Catalog=Bakehouse;Integrated Security=True";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string emailCheckQuery = "SELECT COUNT(*) FROM Register WHERE Email = @Email";
                        using (SqlCommand emailCheckCmd = new SqlCommand(emailCheckQuery, con))
                        {
                            emailCheckCmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;

                            int emailCount = (int)emailCheckCmd.ExecuteScalar();
                            if (emailCount > 0)
                            {
                                MessageBox.Show("This email is already registered. Please use a different email.");
                                return;
                            }
                        }
                        string query = "INSERT INTO [dbo].[Register] " +
                                       "([Name], [Email], [CNIC], [City], [Username], [Password], [ConfirmPassword]) " +
                                       "VALUES (@Name, @Email, @CNIC, @City, @Username, @Password, @ConfirmPassword)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = txtName.Text;
                            cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;
                            cmd.Parameters.Add("@CNIC", SqlDbType.NVarChar).Value = textCnic.Text;
                            cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = txtCity.Text;
                            cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = txtUser.Text;
                            cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = txtPass.Text;
                            cmd.Parameters.Add("@ConfirmPassword", SqlDbType.NVarChar).Value = txtConfpass.Text;

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Data has been successfully inserted.");
                            }
                            else
                            {
                                MessageBox.Show("Data insertion failed.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            else
            {
                MessageBox.Show("Please fill the required fields");
            }

            // Validate email format
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Invalid email format.");
                return;
            }

            // Validate CNIC format (assuming CNIC is a 13-character string with dashes)
            if (!IsValidCNIC(textCnic.Text))
            {
                MessageBox.Show("Invalid CNIC Format");
                return;
            }

            // Validate password complexity (at least 8 characters with letters and special characters)
            if (!IsValidPassword(txtPass.Text))
            {
                MessageBox.Show("Password must be at least 8 characters long and include letters and special characters.");
                //"Password must be at least 8 characters long and include letters and special characters.");
                return;
            }

            // Proceed with database insertion

        }

        // Email format validation
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // CNIC format validation
        private bool IsValidCNIC(string cnic)
        {

            Console.Write(cnic);
            // Trim leading and trailing spaces from the CNIC string
            cnic = cnic.Trim();

            // Use the regular expression to validate the CNIC
            return Regex.IsMatch(cnic, @"^\d{5}-\d{7}-\d{1}$");
        }



        // Password complexity validation
        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$");
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            {
                DialogResult res;
                res = MessageBox.Show("Do you want to exit", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    Application.Exit();

                }
                else
                {
                    this.Show();
                }
            }
        }
       

        private void txtCnic_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCity_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPass_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void loginLbl_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void Required_Click(object sender, EventArgs e)
        {

        }
    }
}
