namespace TIMS.Forms.POS
{
    partial class SavedInvoicesPicker
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
            this.SequenceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AttentionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmployeeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TerminalColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PhoneNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SequenceColumn,
            this.CustomerColumn,
            this.AttentionColumn,
            this.POColumn,
            this.EmployeeColumn,
            this.TerminalColumn,
            this.PhoneNumberColumn,
            this.DateColumn,
            this.TimeColumn,
            this.ItemCountColumn});
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(993, 533);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // SequenceColumn
            // 
            this.SequenceColumn.HeaderText = "Seq #";
            this.SequenceColumn.Name = "SequenceColumn";
            // 
            // CustomerColumn
            // 
            this.CustomerColumn.HeaderText = "Customer";
            this.CustomerColumn.Name = "CustomerColumn";
            // 
            // AttentionColumn
            // 
            this.AttentionColumn.HeaderText = "Attention";
            this.AttentionColumn.Name = "AttentionColumn";
            // 
            // POColumn
            // 
            this.POColumn.HeaderText = "PO";
            this.POColumn.Name = "POColumn";
            // 
            // EmployeeColumn
            // 
            this.EmployeeColumn.HeaderText = "Employee";
            this.EmployeeColumn.Name = "EmployeeColumn";
            // 
            // TerminalColumn
            // 
            this.TerminalColumn.HeaderText = "Terminal";
            this.TerminalColumn.Name = "TerminalColumn";
            // 
            // PhoneNumberColumn
            // 
            this.PhoneNumberColumn.HeaderText = "Phone Number";
            this.PhoneNumberColumn.Name = "PhoneNumberColumn";
            // 
            // DateColumn
            // 
            this.DateColumn.HeaderText = "Date";
            this.DateColumn.Name = "DateColumn";
            // 
            // TimeColumn
            // 
            this.TimeColumn.HeaderText = "Time";
            this.TimeColumn.Name = "TimeColumn";
            // 
            // ItemCountColumn
            // 
            this.ItemCountColumn.HeaderText = "# Of Items";
            this.ItemCountColumn.Name = "ItemCountColumn";
            // 
            // SavedInvoicesPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 605);
            this.Controls.Add(this.dataGridView1);
            this.Name = "SavedInvoicesPicker";
            this.Text = "SavedInvoicesPicker";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SequenceColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AttentionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn POColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployeeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TerminalColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PhoneNumberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemCountColumn;
    }
}