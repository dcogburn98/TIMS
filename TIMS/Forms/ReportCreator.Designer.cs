namespace TIMS.Forms
{
    partial class ReportCreator
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataSourceCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.reportNameTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.previewReportButton = new System.Windows.Forms.Button();
            this.saveReportButton = new System.Windows.Forms.Button();
            this.conditionsLB = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.conditionLeftComparatorCB = new System.Windows.Forms.ComboBox();
            this.conditionOperatorCB = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fieldsLB = new System.Windows.Forms.ListBox();
            this.addFieldButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.fieldsCB = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.totalLB = new System.Windows.Forms.ListBox();
            this.addTotalButton = new System.Windows.Forms.Button();
            this.totalCB = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.conditionRightComparatorCB = new System.Windows.Forms.ComboBox();
            this.addConditionButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.viewQueryButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(12, 357);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(698, 265);
            this.dataGridView1.TabIndex = 0;
            // 
            // dataSourceCB
            // 
            this.dataSourceCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataSourceCB.FormattingEnabled = true;
            this.dataSourceCB.Location = new System.Drawing.Point(90, 45);
            this.dataSourceCB.Name = "dataSourceCB";
            this.dataSourceCB.Size = new System.Drawing.Size(176, 21);
            this.dataSourceCB.TabIndex = 1;
            this.dataSourceCB.Leave += new System.EventHandler(this.dataSourceCB_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(524, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Report Creator";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Report Name:";
            // 
            // reportNameTB
            // 
            this.reportNameTB.Location = new System.Drawing.Point(90, 19);
            this.reportNameTB.Name = "reportNameTB";
            this.reportNameTB.Size = new System.Drawing.Size(293, 20);
            this.reportNameTB.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Data Source:";
            // 
            // previewReportButton
            // 
            this.previewReportButton.Enabled = false;
            this.previewReportButton.Location = new System.Drawing.Point(12, 328);
            this.previewReportButton.Name = "previewReportButton";
            this.previewReportButton.Size = new System.Drawing.Size(75, 23);
            this.previewReportButton.TabIndex = 6;
            this.previewReportButton.Text = "Preview";
            this.previewReportButton.UseVisualStyleBackColor = true;
            this.previewReportButton.Click += new System.EventHandler(this.previewReportButton_Click);
            // 
            // saveReportButton
            // 
            this.saveReportButton.Location = new System.Drawing.Point(635, 328);
            this.saveReportButton.Name = "saveReportButton";
            this.saveReportButton.Size = new System.Drawing.Size(75, 23);
            this.saveReportButton.TabIndex = 7;
            this.saveReportButton.Text = "Save Report";
            this.saveReportButton.UseVisualStyleBackColor = true;
            // 
            // conditionsLB
            // 
            this.conditionsLB.FormattingEnabled = true;
            this.conditionsLB.Location = new System.Drawing.Point(90, 72);
            this.conditionsLB.Name = "conditionsLB";
            this.conditionsLB.Size = new System.Drawing.Size(292, 95);
            this.conditionsLB.TabIndex = 8;
            this.conditionsLB.DoubleClick += new System.EventHandler(this.conditionsLB_DoubleClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Conditions:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Add Condition:";
            // 
            // conditionLeftComparatorCB
            // 
            this.conditionLeftComparatorCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionLeftComparatorCB.FormattingEnabled = true;
            this.conditionLeftComparatorCB.Location = new System.Drawing.Point(90, 173);
            this.conditionLeftComparatorCB.Name = "conditionLeftComparatorCB";
            this.conditionLeftComparatorCB.Size = new System.Drawing.Size(115, 21);
            this.conditionLeftComparatorCB.TabIndex = 11;
            // 
            // conditionOperatorCB
            // 
            this.conditionOperatorCB.FormattingEnabled = true;
            this.conditionOperatorCB.Items.AddRange(new object[] {
            ">",
            ">=",
            "<",
            "<=",
            "==",
            "!=",
            "CONTAINS"});
            this.conditionOperatorCB.Location = new System.Drawing.Point(211, 173);
            this.conditionOperatorCB.Name = "conditionOperatorCB";
            this.conditionOperatorCB.Size = new System.Drawing.Size(50, 21);
            this.conditionOperatorCB.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.addConditionButton);
            this.groupBox1.Controls.Add(this.conditionRightComparatorCB);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dataSourceCB);
            this.groupBox1.Controls.Add(this.conditionOperatorCB);
            this.groupBox1.Controls.Add(this.reportNameTB);
            this.groupBox1.Controls.Add(this.conditionLeftComparatorCB);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.conditionsLB);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(389, 281);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Data:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fieldsLB);
            this.groupBox2.Controls.Add(this.addFieldButton);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.fieldsCB);
            this.groupBox2.Location = new System.Drawing.Point(407, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(303, 154);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Report Fields:";
            // 
            // fieldsLB
            // 
            this.fieldsLB.FormattingEnabled = true;
            this.fieldsLB.Location = new System.Drawing.Point(44, 46);
            this.fieldsLB.Name = "fieldsLB";
            this.fieldsLB.Size = new System.Drawing.Size(172, 95);
            this.fieldsLB.TabIndex = 3;
            this.fieldsLB.DoubleClick += new System.EventHandler(this.fieldsLB_DoubleClick);
            // 
            // addFieldButton
            // 
            this.addFieldButton.Location = new System.Drawing.Point(222, 19);
            this.addFieldButton.Name = "addFieldButton";
            this.addFieldButton.Size = new System.Drawing.Size(75, 23);
            this.addFieldButton.TabIndex = 2;
            this.addFieldButton.Text = "Add Field";
            this.addFieldButton.UseVisualStyleBackColor = true;
            this.addFieldButton.Click += new System.EventHandler(this.addFieldButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Field:";
            // 
            // fieldsCB
            // 
            this.fieldsCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fieldsCB.FormattingEnabled = true;
            this.fieldsCB.Location = new System.Drawing.Point(44, 19);
            this.fieldsCB.Name = "fieldsCB";
            this.fieldsCB.Size = new System.Drawing.Size(172, 21);
            this.fieldsCB.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.totalLB);
            this.groupBox3.Controls.Add(this.addTotalButton);
            this.groupBox3.Controls.Add(this.totalCB);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(407, 201);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(303, 121);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Totals";
            // 
            // totalLB
            // 
            this.totalLB.FormattingEnabled = true;
            this.totalLB.Location = new System.Drawing.Point(46, 46);
            this.totalLB.Name = "totalLB";
            this.totalLB.Size = new System.Drawing.Size(170, 69);
            this.totalLB.TabIndex = 3;
            // 
            // addTotalButton
            // 
            this.addTotalButton.Location = new System.Drawing.Point(222, 17);
            this.addTotalButton.Name = "addTotalButton";
            this.addTotalButton.Size = new System.Drawing.Size(75, 23);
            this.addTotalButton.TabIndex = 2;
            this.addTotalButton.Text = "Add Total";
            this.addTotalButton.UseVisualStyleBackColor = true;
            this.addTotalButton.Click += new System.EventHandler(this.addTotalButton_Click);
            // 
            // totalCB
            // 
            this.totalCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.totalCB.FormattingEnabled = true;
            this.totalCB.Location = new System.Drawing.Point(46, 19);
            this.totalCB.Name = "totalCB";
            this.totalCB.Size = new System.Drawing.Size(170, 21);
            this.totalCB.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Total:";
            // 
            // conditionRightComparatorCB
            // 
            this.conditionRightComparatorCB.FormattingEnabled = true;
            this.conditionRightComparatorCB.Location = new System.Drawing.Point(267, 173);
            this.conditionRightComparatorCB.Name = "conditionRightComparatorCB";
            this.conditionRightComparatorCB.Size = new System.Drawing.Size(115, 21);
            this.conditionRightComparatorCB.TabIndex = 4;
            // 
            // addConditionButton
            // 
            this.addConditionButton.Location = new System.Drawing.Point(200, 200);
            this.addConditionButton.Name = "addConditionButton";
            this.addConditionButton.Size = new System.Drawing.Size(75, 23);
            this.addConditionButton.TabIndex = 13;
            this.addConditionButton.Text = "Add";
            this.addConditionButton.UseVisualStyleBackColor = true;
            this.addConditionButton.Click += new System.EventHandler(this.addConditionButton_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(87, 242);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(225, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Double click to remove items from lists";
            // 
            // viewQueryButton
            // 
            this.viewQueryButton.Enabled = false;
            this.viewQueryButton.Location = new System.Drawing.Point(93, 328);
            this.viewQueryButton.Name = "viewQueryButton";
            this.viewQueryButton.Size = new System.Drawing.Size(75, 23);
            this.viewQueryButton.TabIndex = 17;
            this.viewQueryButton.Text = "View Query";
            this.viewQueryButton.UseVisualStyleBackColor = true;
            this.viewQueryButton.Click += new System.EventHandler(this.viewQueryButton_Click);
            // 
            // ReportCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 634);
            this.Controls.Add(this.viewQueryButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.saveReportButton);
            this.Controls.Add(this.previewReportButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ReportCreator";
            this.Text = "ReportCreator";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox dataSourceCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox reportNameTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button previewReportButton;
        private System.Windows.Forms.Button saveReportButton;
        private System.Windows.Forms.ListBox conditionsLB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox conditionLeftComparatorCB;
        private System.Windows.Forms.ComboBox conditionOperatorCB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox fieldsLB;
        private System.Windows.Forms.Button addFieldButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox fieldsCB;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox totalLB;
        private System.Windows.Forms.Button addTotalButton;
        private System.Windows.Forms.ComboBox totalCB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button addConditionButton;
        private System.Windows.Forms.ComboBox conditionRightComparatorCB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button viewQueryButton;
    }
}