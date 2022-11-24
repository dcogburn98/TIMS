namespace TIMS.Forms
{
    partial class ItemImportFieldEditor
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
            this.acceptButton = new System.Windows.Forms.Button();
            this.itemHeaderCB = new System.Windows.Forms.ComboBox();
            this.csvHeaderTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.skipButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(190, 78);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 0;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // itemHeaderCB
            // 
            this.itemHeaderCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.itemHeaderCB.FormattingEnabled = true;
            this.itemHeaderCB.Location = new System.Drawing.Point(84, 51);
            this.itemHeaderCB.Name = "itemHeaderCB";
            this.itemHeaderCB.Size = new System.Drawing.Size(181, 21);
            this.itemHeaderCB.TabIndex = 1;
            // 
            // csvHeaderTB
            // 
            this.csvHeaderTB.Enabled = false;
            this.csvHeaderTB.Location = new System.Drawing.Point(84, 25);
            this.csvHeaderTB.Name = "csvHeaderTB";
            this.csvHeaderTB.Size = new System.Drawing.Size(181, 20);
            this.csvHeaderTB.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "CSV Column";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Item Data";
            // 
            // skipButton
            // 
            this.skipButton.Location = new System.Drawing.Point(12, 78);
            this.skipButton.Name = "skipButton";
            this.skipButton.Size = new System.Drawing.Size(75, 23);
            this.skipButton.TabIndex = 6;
            this.skipButton.Text = "Skip";
            this.skipButton.UseVisualStyleBackColor = true;
            this.skipButton.Click += new System.EventHandler(this.skipButton_Click);
            // 
            // ItemImportFieldEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 113);
            this.ControlBox = false;
            this.Controls.Add(this.skipButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.csvHeaderTB);
            this.Controls.Add(this.itemHeaderCB);
            this.Controls.Add(this.acceptButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ItemImportFieldEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ItemImportFieldEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.ComboBox itemHeaderCB;
        private System.Windows.Forms.TextBox csvHeaderTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button skipButton;
    }
}