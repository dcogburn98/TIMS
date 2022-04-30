
namespace TIMS
{
    partial class Login
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
            this.loginButton = new System.Windows.Forms.Button();
            this.loginGroup = new System.Windows.Forms.GroupBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.exceptionBox = new System.Windows.Forms.TextBox();
            this.badLoginLabel = new System.Windows.Forms.Label();
            this.tempKeyBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.timsLogo = new System.Windows.Forms.PictureBox();
            this.companyLogo = new System.Windows.Forms.PictureBox();
            this.badPasswordLabel = new System.Windows.Forms.Label();
            this.loginGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timsLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(273, 260);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 3;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // loginGroup
            // 
            this.loginGroup.Controls.Add(this.badPasswordLabel);
            this.loginGroup.Controls.Add(this.passwordBox);
            this.loginGroup.Controls.Add(this.exceptionBox);
            this.loginGroup.Controls.Add(this.badLoginLabel);
            this.loginGroup.Controls.Add(this.tempKeyBox);
            this.loginGroup.Controls.Add(this.label3);
            this.loginGroup.Controls.Add(this.label2);
            this.loginGroup.Controls.Add(this.label1);
            this.loginGroup.Controls.Add(this.usernameBox);
            this.loginGroup.Controls.Add(this.exitButton);
            this.loginGroup.Controls.Add(this.loginButton);
            this.loginGroup.Location = new System.Drawing.Point(320, 12);
            this.loginGroup.Name = "loginGroup";
            this.loginGroup.Size = new System.Drawing.Size(354, 289);
            this.loginGroup.TabIndex = 1;
            this.loginGroup.TabStop = false;
            this.loginGroup.Text = "Login Credentials";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(128, 90);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(220, 20);
            this.passwordBox.TabIndex = 1;
            this.passwordBox.TextChanged += new System.EventHandler(this.passwordBox_TextChanged);
            this.passwordBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordBox_KeyDown);
            // 
            // exceptionBox
            // 
            this.exceptionBox.Enabled = false;
            this.exceptionBox.Location = new System.Drawing.Point(128, 151);
            this.exceptionBox.Multiline = true;
            this.exceptionBox.Name = "exceptionBox";
            this.exceptionBox.Size = new System.Drawing.Size(220, 103);
            this.exceptionBox.TabIndex = 8;
            // 
            // badLoginLabel
            // 
            this.badLoginLabel.AutoSize = true;
            this.badLoginLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.badLoginLabel.Location = new System.Drawing.Point(125, 39);
            this.badLoginLabel.Name = "badLoginLabel";
            this.badLoginLabel.Size = new System.Drawing.Size(225, 13);
            this.badLoginLabel.TabIndex = 7;
            this.badLoginLabel.Text = "Invalid Username or Employee Number";
            // 
            // tempKeyBox
            // 
            this.tempKeyBox.Location = new System.Drawing.Point(128, 125);
            this.tempKeyBox.Name = "tempKeyBox";
            this.tempKeyBox.Size = new System.Drawing.Size(220, 20);
            this.tempKeyBox.TabIndex = 2;
            this.tempKeyBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tempKeyBox_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Temporary Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Username/Employee #";
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(128, 55);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(220, 20);
            this.usernameBox.TabIndex = 0;
            this.usernameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.usernameBox_KeyDown);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(6, 260);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 4;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // timsLogo
            // 
            this.timsLogo.BackColor = System.Drawing.SystemColors.Control;
            this.timsLogo.Image = global::TIMS.Properties.Resources.TIMS_Logo1;
            this.timsLogo.Location = new System.Drawing.Point(12, 12);
            this.timsLogo.Name = "timsLogo";
            this.timsLogo.Size = new System.Drawing.Size(302, 70);
            this.timsLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.timsLogo.TabIndex = 3;
            this.timsLogo.TabStop = false;
            // 
            // companyLogo
            // 
            this.companyLogo.BackColor = System.Drawing.SystemColors.Control;
            this.companyLogo.Image = global::TIMS.Properties.Resources._2;
            this.companyLogo.Location = new System.Drawing.Point(12, 88);
            this.companyLogo.Name = "companyLogo";
            this.companyLogo.Size = new System.Drawing.Size(302, 220);
            this.companyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.companyLogo.TabIndex = 2;
            this.companyLogo.TabStop = false;
            this.companyLogo.UseWaitCursor = true;
            // 
            // badPasswordLabel
            // 
            this.badPasswordLabel.AutoSize = true;
            this.badPasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.badPasswordLabel.ForeColor = System.Drawing.Color.Red;
            this.badPasswordLabel.Location = new System.Drawing.Point(125, 16);
            this.badPasswordLabel.Name = "badPasswordLabel";
            this.badPasswordLabel.Size = new System.Drawing.Size(120, 13);
            this.badPasswordLabel.TabIndex = 9;
            this.badPasswordLabel.Text = "Incorrect Password!";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 313);
            this.Controls.Add(this.timsLogo);
            this.Controls.Add(this.companyLogo);
            this.Controls.Add(this.loginGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Login";
            this.Text = "TIMS - Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Login_FormClosed);
            this.loginGroup.ResumeLayout(false);
            this.loginGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timsLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.GroupBox loginGroup;
        private System.Windows.Forms.TextBox tempKeyBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.PictureBox companyLogo;
        private System.Windows.Forms.PictureBox timsLogo;
        private System.Windows.Forms.Label badLoginLabel;
        private System.Windows.Forms.TextBox exceptionBox;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label badPasswordLabel;
    }
}