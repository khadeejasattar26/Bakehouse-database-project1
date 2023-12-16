using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sign_in_Application
{
    public partial class Form1 : Form
    {
        string name;
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection conn=new SqlConnection(@"Data Source=DESKTOP-SQLM2FG\SQLEXPRESS;Initial Catalog=Bakehouse;Integrated Security=True");
        private Registeration_form form2;
        private Form3 form3;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string username = txt_username.Text;
            string user_password = txt_password.Text;

            try
            {
                // Query to check if the email, username, and password match a record in the database
                string query = "SELECT * FROM Register WHERE email = @Email AND username = @Username AND password = @Password";

                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-SQLM2FG\SQLEXPRESS;Initial Catalog=Bakehouse;Integrated Security=True"))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", user_password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                name = username;
                                // Login successful
                                MessageBox.Show("Login successful.");
                               // Registeration_form form = new Registeration_form();
                                Form3 form3 = new Form3(name);
                                form3.Show();
                                this.Hide();
                                // Navigate to the next page or perform necessary actions here
                                // For example, open a new form or display some content.
                            }
                            else
                            {
                                // Invalid login credentials
                                MessageBox.Show("Invalid login details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        private void button_clear_Click(object sender, EventArgs e)
        {
            txt_username.Clear ();
            txt_password.Clear ();
            txt_username.Focus();
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            DialogResult res;
            res=MessageBox.Show("Do you want to exit","Exit",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(res == DialogResult.Yes)
            {
                Application.Exit();

            }
            else
            {
                this.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registeration_form form2 = new Registeration_form();
            form2.Show();
            this.Hide();
        

    }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
