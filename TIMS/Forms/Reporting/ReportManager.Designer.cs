namespace TIMS.Forms.Reporting
{
    partial class ReportManager
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
            this.reportPickerCB = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.conditionsLB = new System.Windows.Forms.ListBox();
            this.printButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.printTotalsCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(372, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "Report Manager";
            // 
            // reportPickerCB
            // 
            this.reportPickerCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reportPickerCB.FormattingEnabled = true;
            this.reportPickerCB.Location = new System.Drawing.Point(60, 6);
            this.reportPickerCB.Name = "reportPickerCB";
            this.reportPickerCB.Size = new System.Drawing.Size(302, 21);
            this.reportPickerCB.TabIndex = 4;
            this.reportPickerCB.SelectedIndexChanged += new System.EventHandler(this.reportPickerCB_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Report:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.resetButton);
            this.groupBox1.Controls.Add(this.conditionsLB);
            this.groupBox1.Location = new System.Drawing.Point(15, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 405);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variables";
            // 
            // conditionsLB
            // 
            this.conditionsLB.FormattingEnabled = true;
            this.conditionsLB.Location = new System.Drawing.Point(6, 19);
            this.conditionsLB.Name = "conditionsLB";
            this.conditionsLB.Size = new System.Drawing.Size(335, 342);
            this.conditionsLB.TabIndex = 0;
            this.conditionsLB.DoubleClick += new System.EventHandler(this.conditionsLB_DoubleClick);
            // 
            // printButton
            // 
            this.printButton.Enabled = false;
            this.printButton.Location = new System.Drawing.Point(498, 415);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(75, 23);
            this.printButton.TabIndex = 7;
            this.printButton.Text = "Print Report";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Enabled = false;
            this.resetButton.Location = new System.Drawing.Point(6, 367);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(99, 23);
            this.resetButton.TabIndex = 1;
            this.resetButton.Text = "Reset Conditions";
            this.resetButton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.printTotalsCheckbox);
            this.groupBox2.Location = new System.Drawing.Point(368, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(205, 368);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // printTotalsCheckbox
            // 
            this.printTotalsCheckbox.AutoSize = true;
            this.printTotalsCheckbox.Location = new System.Drawing.Point(9, 19);
            this.printTotalsCheckbox.Name = "printTotalsCheckbox";
            this.printTotalsCheckbox.Size = new System.Drawing.Size(104, 17);
            this.printTotalsCheckbox.TabIndex = 0;
            this.printTotalsCheckbox.Text = "Print Total Rows";
            this.printTotalsCheckbox.UseVisualStyleBackColor = true;
            // 
            // ReportManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.reportPickerCB);
            this.Controls.Add(this.label1);
            this.Name = "ReportManager";
            this.Text = "ReportManager";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox reportPickerCB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox conditionsLB;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox printTotalsCheckbox;
    }
}