namespace TIMS.Forms.Customers
{
    partial class PricingProfileEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.department = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subDepartment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceSheet = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Margin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.beginDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveProfileButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.deleteElementButton = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.priorityDownButton = new System.Windows.Forms.Button();
            this.priorityUpButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(237, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(342, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(161, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Profile Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Profile ID:";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(65, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(90, 20);
            this.textBox2.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Silver;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groupCode,
            this.department,
            this.subDepartment,
            this.ProductLine,
            this.ItemNumber,
            this.PriceSheet,
            this.Margin,
            this.beginDate,
            this.endDate});
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridView1.Location = new System.Drawing.Point(12, 38);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(936, 559);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
            // 
            // groupCode
            // 
            this.groupCode.HeaderText = "Group Code";
            this.groupCode.Name = "groupCode";
            // 
            // department
            // 
            this.department.HeaderText = "Department";
            this.department.Name = "department";
            // 
            // subDepartment
            // 
            this.subDepartment.HeaderText = "Sub Department";
            this.subDepartment.Name = "subDepartment";
            // 
            // ProductLine
            // 
            this.ProductLine.HeaderText = "Product Line";
            this.ProductLine.Name = "ProductLine";
            // 
            // ItemNumber
            // 
            this.ItemNumber.HeaderText = "Item Number";
            this.ItemNumber.Name = "ItemNumber";
            // 
            // PriceSheet
            // 
            this.PriceSheet.HeaderText = "Price Sheet";
            this.PriceSheet.Name = "PriceSheet";
            // 
            // Margin
            // 
            this.Margin.HeaderText = "Margin (%)";
            this.Margin.Name = "Margin";
            // 
            // beginDate
            // 
            this.beginDate.HeaderText = "Begin Date";
            this.beginDate.Name = "beginDate";
            // 
            // endDate
            // 
            this.endDate.HeaderText = "End Date";
            this.endDate.Name = "endDate";
            // 
            // saveProfileButton
            // 
            this.saveProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveProfileButton.Location = new System.Drawing.Point(93, 603);
            this.saveProfileButton.Name = "saveProfileButton";
            this.saveProfileButton.Size = new System.Drawing.Size(75, 23);
            this.saveProfileButton.TabIndex = 5;
            this.saveProfileButton.Text = "Save";
            this.saveProfileButton.UseVisualStyleBackColor = true;
            this.saveProfileButton.Click += new System.EventHandler(this.saveProfileButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.Location = new System.Drawing.Point(12, 603);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // deleteElementButton
            // 
            this.deleteElementButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteElementButton.Enabled = false;
            this.deleteElementButton.Location = new System.Drawing.Point(827, 603);
            this.deleteElementButton.Name = "deleteElementButton";
            this.deleteElementButton.Size = new System.Drawing.Size(88, 23);
            this.deleteElementButton.TabIndex = 8;
            this.deleteElementButton.Text = "Delete Element";
            this.deleteElementButton.UseVisualStyleBackColor = true;
            this.deleteElementButton.Click += new System.EventHandler(this.deleteElementButton_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(921, 603);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Add Element";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // priorityDownButton
            // 
            this.priorityDownButton.Enabled = false;
            this.priorityDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.priorityDownButton.Location = new System.Drawing.Point(961, 314);
            this.priorityDownButton.Name = "priorityDownButton";
            this.priorityDownButton.Size = new System.Drawing.Size(35, 45);
            this.priorityDownButton.TabIndex = 10;
            this.priorityDownButton.Text = "↓";
            this.priorityDownButton.UseVisualStyleBackColor = true;
            // 
            // priorityUpButton
            // 
            this.priorityUpButton.Enabled = false;
            this.priorityUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.priorityUpButton.Location = new System.Drawing.Point(961, 263);
            this.priorityUpButton.Name = "priorityUpButton";
            this.priorityUpButton.Size = new System.Drawing.Size(35, 45);
            this.priorityUpButton.TabIndex = 9;
            this.priorityUpButton.Text = "↑";
            this.priorityUpButton.UseVisualStyleBackColor = true;
            this.priorityUpButton.Click += new System.EventHandler(this.priorityUpButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(952, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Arrange";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(954, 246);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Priority";
            // 
            // PricingProfileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 638);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.priorityDownButton);
            this.Controls.Add(this.priorityUpButton);
            this.Controls.Add(this.deleteElementButton);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveProfileButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "PricingProfileEditor";
            this.Text = "PricingProfiles";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PricingProfileEditor_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button saveProfileButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button deleteElementButton;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button priorityDownButton;
        private System.Windows.Forms.Button priorityUpButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn department;
        private System.Windows.Forms.DataGridViewTextBoxColumn subDepartment;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemNumber;
        private System.Windows.Forms.DataGridViewComboBoxColumn PriceSheet;
        private System.Windows.Forms.DataGridViewTextBoxColumn Margin;
        private System.Windows.Forms.DataGridViewTextBoxColumn beginDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn endDate;
    }
}