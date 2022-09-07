namespace TIMS.Forms.Settings
{
    partial class AddDevice
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
            this.nicknameField = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ipAddressField = new System.Windows.Forms.TextBox();
            this.portField = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.testButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.deviceTypeField = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Device Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nickname:";
            // 
            // nicknameField
            // 
            this.nicknameField.Location = new System.Drawing.Point(89, 33);
            this.nicknameField.Name = "nicknameField";
            this.nicknameField.Size = new System.Drawing.Size(234, 20);
            this.nicknameField.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "IP Address:";
            // 
            // ipAddressField
            // 
            this.ipAddressField.Location = new System.Drawing.Point(89, 59);
            this.ipAddressField.Name = "ipAddressField";
            this.ipAddressField.Size = new System.Drawing.Size(121, 20);
            this.ipAddressField.TabIndex = 9;
            // 
            // portField
            // 
            this.portField.Location = new System.Drawing.Point(89, 85);
            this.portField.Name = "portField";
            this.portField.Size = new System.Drawing.Size(121, 20);
            this.portField.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Port:";
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(165, 114);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 23);
            this.testButton.TabIndex = 12;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(12, 114);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(246, 114);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 14;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // deviceTypeField
            // 
            this.deviceTypeField.Enabled = false;
            this.deviceTypeField.Location = new System.Drawing.Point(89, 6);
            this.deviceTypeField.Name = "deviceTypeField";
            this.deviceTypeField.Size = new System.Drawing.Size(121, 20);
            this.deviceTypeField.TabIndex = 15;
            // 
            // AddDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 145);
            this.Controls.Add(this.deviceTypeField);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.portField);
            this.Controls.Add(this.ipAddressField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nicknameField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddDevice";
            this.Text = "AddDevice";
            this.Load += new System.EventHandler(this.AddDevice_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nicknameField;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ipAddressField;
        private System.Windows.Forms.TextBox portField;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox deviceTypeField;
    }
}