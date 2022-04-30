
namespace PermissionsTest
{
    partial class Form1
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
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.logoutButton = new System.Windows.Forms.Button();
            this.testButton1 = new System.Windows.Forms.Button();
            this.testButton2 = new System.Windows.Forms.Button();
            this.testButton3 = new System.Windows.Forms.Button();
            this.testButton4 = new System.Windows.Forms.Button();
            this.permissionsBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // usernameBox
            // 
            this.usernameBox.Location = new System.Drawing.Point(73, 6);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(100, 20);
            this.usernameBox.TabIndex = 2;
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(73, 32);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(100, 20);
            this.passwordBox.TabIndex = 3;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(12, 58);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 4;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // logoutButton
            // 
            this.logoutButton.Location = new System.Drawing.Point(98, 58);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(75, 23);
            this.logoutButton.TabIndex = 5;
            this.logoutButton.Text = "Logout";
            this.logoutButton.UseVisualStyleBackColor = true;
            // 
            // testButton1
            // 
            this.testButton1.Location = new System.Drawing.Point(12, 152);
            this.testButton1.Name = "testButton1";
            this.testButton1.Size = new System.Drawing.Size(75, 23);
            this.testButton1.TabIndex = 6;
            this.testButton1.Text = "Button 1";
            this.testButton1.UseVisualStyleBackColor = true;
            this.testButton1.Click += new System.EventHandler(this.testButton1_Click);
            // 
            // testButton2
            // 
            this.testButton2.Location = new System.Drawing.Point(93, 152);
            this.testButton2.Name = "testButton2";
            this.testButton2.Size = new System.Drawing.Size(75, 23);
            this.testButton2.TabIndex = 7;
            this.testButton2.Text = "Button 2";
            this.testButton2.UseVisualStyleBackColor = true;
            // 
            // testButton3
            // 
            this.testButton3.Location = new System.Drawing.Point(174, 152);
            this.testButton3.Name = "testButton3";
            this.testButton3.Size = new System.Drawing.Size(75, 23);
            this.testButton3.TabIndex = 8;
            this.testButton3.Text = "Button 3";
            this.testButton3.UseVisualStyleBackColor = true;
            // 
            // testButton4
            // 
            this.testButton4.Location = new System.Drawing.Point(255, 152);
            this.testButton4.Name = "testButton4";
            this.testButton4.Size = new System.Drawing.Size(75, 23);
            this.testButton4.TabIndex = 9;
            this.testButton4.Text = "Button 4";
            this.testButton4.UseVisualStyleBackColor = true;
            // 
            // permissionsBox
            // 
            this.permissionsBox.FormattingEnabled = true;
            this.permissionsBox.Location = new System.Drawing.Point(191, 6);
            this.permissionsBox.Name = "permissionsBox";
            this.permissionsBox.Size = new System.Drawing.Size(120, 95);
            this.permissionsBox.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 183);
            this.Controls.Add(this.permissionsBox);
            this.Controls.Add(this.testButton4);
            this.Controls.Add(this.testButton3);
            this.Controls.Add(this.testButton2);
            this.Controls.Add(this.testButton1);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.usernameBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.Button testButton1;
        private System.Windows.Forms.Button testButton2;
        private System.Windows.Forms.Button testButton3;
        private System.Windows.Forms.Button testButton4;
        private System.Windows.Forms.ListBox permissionsBox;
    }
}

