namespace Sign_in_Application
{
    partial class Registeration_form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.textCnic = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtConfpass = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.Exitbtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.loginLbl = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Required = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Email";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "CNIC";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(57, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "City";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(57, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Username";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(57, 248);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Password";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(57, 290);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 16);
            this.label7.TabIndex = 6;
            this.label7.Text = "Confirm Password";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(186, 71);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(249, 22);
            this.txtName.TabIndex = 7;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(186, 110);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(249, 22);
            this.txtEmail.TabIndex = 8;
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(186, 180);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(249, 22);
            this.txtCity.TabIndex = 9;
            this.txtCity.TextChanged += new System.EventHandler(this.txtCity_TextChanged);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(186, 213);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(249, 22);
            this.txtUser.TabIndex = 10;
            this.txtUser.TextChanged += new System.EventHandler(this.txtCnic_TextChanged);
            // 
            // textCnic
            // 
            this.textCnic.Location = new System.Drawing.Point(186, 145);
            this.textCnic.Name = "textCnic";
            this.textCnic.Size = new System.Drawing.Size(249, 22);
            this.textCnic.TabIndex = 11;
            this.textCnic.TextChanged += new System.EventHandler(this.txtPass_TextChanged);
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(186, 245);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(249, 22);
            this.txtPass.TabIndex = 12;
            this.txtPass.TextChanged += new System.EventHandler(this.txtPass_TextChanged_1);
            // 
            // txtConfpass
            // 
            this.txtConfpass.Location = new System.Drawing.Point(186, 287);
            this.txtConfpass.Name = "txtConfpass";
            this.txtConfpass.PasswordChar = '*';
            this.txtConfpass.Size = new System.Drawing.Size(249, 22);
            this.txtConfpass.TabIndex = 13;
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(454, 353);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(90, 34);
            this.btnRegister.TabIndex = 14;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // Exitbtn
            // 
            this.Exitbtn.Location = new System.Drawing.Point(70, 353);
            this.Exitbtn.Name = "Exitbtn";
            this.Exitbtn.Size = new System.Drawing.Size(102, 40);
            this.Exitbtn.TabIndex = 15;
            this.Exitbtn.Text = "Exit";
            this.Exitbtn.UseVisualStyleBackColor = true;
            this.Exitbtn.Click += new System.EventHandler(this.Exitbtn_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(155, 385);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(16, 8);
            this.button2.TabIndex = 16;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // loginLbl
            // 
            this.loginLbl.AutoSize = true;
            this.loginLbl.Location = new System.Drawing.Point(314, 412);
            this.loginLbl.Name = "loginLbl";
            this.loginLbl.Size = new System.Drawing.Size(222, 16);
            this.loginLbl.TabIndex = 17;
            this.loginLbl.Text = "Already have an account?Login now";
            this.loginLbl.Click += new System.EventHandler(this.loginLbl_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label8.Location = new System.Drawing.Point(451, 151);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 16);
            this.label8.TabIndex = 18;
            this.label8.Text = "Required";
            // 
            // Required
            // 
            this.Required.AutoSize = true;
            this.Required.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.Required.Location = new System.Drawing.Point(451, 219);
            this.Required.Name = "Required";
            this.Required.Size = new System.Drawing.Size(63, 16);
            this.Required.TabIndex = 19;
            this.Required.Text = "Required";
            this.Required.Click += new System.EventHandler(this.Required_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label10.Location = new System.Drawing.Point(451, 251);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 16);
            this.label10.TabIndex = 20;
            this.label10.Text = "Required";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label9.Location = new System.Drawing.Point(451, 113);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 16);
            this.label9.TabIndex = 21;
            this.label9.Text = "Required";
            // 
            // Registeration_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.Required);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.loginLbl);
            this.Controls.Add(this.textCnic);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Exitbtn);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.txtConfpass);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Registeration_form";
            this.Text = "Registeration_form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox  txtUser;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.TextBox  textCnic;
        private System.Windows.Forms.TextBox txtConfpass;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button Exitbtn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label loginLbl;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label Required;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
    }
}