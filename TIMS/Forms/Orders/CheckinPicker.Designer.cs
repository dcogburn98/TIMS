namespace TIMS.Forms.Orders
{
    partial class CheckinPicker
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.productLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shippedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.damagedQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchaseOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(107, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Checkin Number:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productLine,
            this.itemNumber,
            this.description,
            this.orderedQty,
            this.shippedQty,
            this.receivedQty,
            this.damagedQty,
            this.purchaseOrder});
            this.dataGridView1.Location = new System.Drawing.Point(12, 33);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1072, 566);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // productLine
            // 
            this.productLine.FillWeight = 25F;
            this.productLine.HeaderText = "Product Line";
            this.productLine.Name = "productLine";
            // 
            // itemNumber
            // 
            this.itemNumber.FillWeight = 30F;
            this.itemNumber.HeaderText = "Item Number";
            this.itemNumber.Name = "itemNumber";
            // 
            // description
            // 
            this.description.FillWeight = 120F;
            this.description.HeaderText = "Description";
            this.description.Name = "description";
            // 
            // orderedQty
            // 
            this.orderedQty.FillWeight = 20F;
            this.orderedQty.HeaderText = "Ordered";
            this.orderedQty.Name = "orderedQty";
            // 
            // shippedQty
            // 
            this.shippedQty.FillWeight = 20F;
            this.shippedQty.HeaderText = "Shipped";
            this.shippedQty.Name = "shippedQty";
            // 
            // receivedQty
            // 
            this.receivedQty.FillWeight = 20F;
            this.receivedQty.HeaderText = "Received";
            this.receivedQty.Name = "receivedQty";
            // 
            // damagedQty
            // 
            this.damagedQty.FillWeight = 20F;
            this.damagedQty.HeaderText = "Damaged";
            this.damagedQty.Name = "damagedQty";
            // 
            // purchaseOrder
            // 
            this.purchaseOrder.FillWeight = 40F;
            this.purchaseOrder.HeaderText = "Purchase Order";
            this.purchaseOrder.Name = "purchaseOrder";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(988, 605);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Post to Inventory";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(249, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Display:";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "All Items",
            "Exceptions by Shipped Quantity",
            "Exceptions by Order Quantity"});
            this.comboBox2.Location = new System.Drawing.Point(299, 6);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 5;
            // 
            // CheckinPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 640);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Name = "CheckinPicker";
            this.Text = "CheckinPicker";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderedQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn shippedQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivedQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn damagedQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchaseOrder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
    }
}