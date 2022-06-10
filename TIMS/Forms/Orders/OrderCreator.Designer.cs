namespace TIMS.Forms.Orders
{
    partial class OrderCreator
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
            this.itemNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.retail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extRetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deleteItemButton = new System.Windows.Forms.Button();
            this.saveOrderButton = new System.Windows.Forms.Button();
            this.finalizeButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.itemNumberTB = new System.Windows.Forms.TextBox();
            this.productLineCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.qtyTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.addItemButton = new System.Windows.Forms.Button();
            this.clearItemButton = new System.Windows.Forms.Button();
            this.supplierLabel = new System.Windows.Forms.Label();
            this.criteriaLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemNumber,
            this.productLine,
            this.description,
            this.qty,
            this.cost,
            this.retail,
            this.extCost,
            this.extRetail});
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(12, 52);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(803, 357);
            this.dataGridView1.TabIndex = 0;
            // 
            // itemNumber
            // 
            this.itemNumber.HeaderText = "Item Number";
            this.itemNumber.Name = "itemNumber";
            // 
            // productLine
            // 
            this.productLine.HeaderText = "Product Line";
            this.productLine.Name = "productLine";
            // 
            // description
            // 
            this.description.HeaderText = "Description";
            this.description.Name = "description";
            // 
            // qty
            // 
            this.qty.HeaderText = "Quantity";
            this.qty.Name = "qty";
            // 
            // cost
            // 
            this.cost.HeaderText = "Cost";
            this.cost.Name = "cost";
            // 
            // retail
            // 
            this.retail.HeaderText = "Retail Price";
            this.retail.Name = "retail";
            // 
            // extCost
            // 
            this.extCost.HeaderText = "Ext. Cost";
            this.extCost.Name = "extCost";
            // 
            // extRetail
            // 
            this.extRetail.HeaderText = "Ext. Retail";
            this.extRetail.Name = "extRetail";
            // 
            // deleteItemButton
            // 
            this.deleteItemButton.Enabled = false;
            this.deleteItemButton.Location = new System.Drawing.Point(578, 415);
            this.deleteItemButton.Name = "deleteItemButton";
            this.deleteItemButton.Size = new System.Drawing.Size(75, 23);
            this.deleteItemButton.TabIndex = 8;
            this.deleteItemButton.Text = "Delete Item";
            this.deleteItemButton.UseVisualStyleBackColor = true;
            // 
            // saveOrderButton
            // 
            this.saveOrderButton.Enabled = false;
            this.saveOrderButton.Location = new System.Drawing.Point(740, 415);
            this.saveOrderButton.Name = "saveOrderButton";
            this.saveOrderButton.Size = new System.Drawing.Size(75, 23);
            this.saveOrderButton.TabIndex = 6;
            this.saveOrderButton.Text = "Save Order";
            this.saveOrderButton.UseVisualStyleBackColor = true;
            // 
            // finalizeButton
            // 
            this.finalizeButton.Enabled = false;
            this.finalizeButton.Location = new System.Drawing.Point(659, 415);
            this.finalizeButton.Name = "finalizeButton";
            this.finalizeButton.Size = new System.Drawing.Size(75, 23);
            this.finalizeButton.TabIndex = 7;
            this.finalizeButton.Text = "Finalize";
            this.finalizeButton.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 415);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Exit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // itemNumberTB
            // 
            this.itemNumberTB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.itemNumberTB.Location = new System.Drawing.Point(9, 25);
            this.itemNumberTB.Name = "itemNumberTB";
            this.itemNumberTB.Size = new System.Drawing.Size(112, 20);
            this.itemNumberTB.TabIndex = 1;
            this.itemNumberTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // productLineCB
            // 
            this.productLineCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.productLineCB.FormattingEnabled = true;
            this.productLineCB.Location = new System.Drawing.Point(127, 24);
            this.productLineCB.Name = "productLineCB";
            this.productLineCB.Size = new System.Drawing.Size(67, 21);
            this.productLineCB.TabIndex = 2;
            this.productLineCB.Enter += new System.EventHandler(this.productLineCB_Enter);
            this.productLineCB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.productLineCB_KeyDown);
            this.productLineCB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.productLineCB_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Product Line";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Item Number/Barcode";
            // 
            // qtyTB
            // 
            this.qtyTB.Location = new System.Drawing.Point(200, 25);
            this.qtyTB.Name = "qtyTB";
            this.qtyTB.Size = new System.Drawing.Size(46, 20);
            this.qtyTB.TabIndex = 3;
            this.qtyTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.qtyTB_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(200, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Quantity";
            // 
            // addItemButton
            // 
            this.addItemButton.Location = new System.Drawing.Point(252, 23);
            this.addItemButton.Name = "addItemButton";
            this.addItemButton.Size = new System.Drawing.Size(75, 23);
            this.addItemButton.TabIndex = 4;
            this.addItemButton.Text = "Add Item";
            this.addItemButton.UseVisualStyleBackColor = true;
            this.addItemButton.Click += new System.EventHandler(this.addItemButton_Click);
            // 
            // clearItemButton
            // 
            this.clearItemButton.Location = new System.Drawing.Point(333, 23);
            this.clearItemButton.Name = "clearItemButton";
            this.clearItemButton.Size = new System.Drawing.Size(75, 23);
            this.clearItemButton.TabIndex = 5;
            this.clearItemButton.Text = "Clear Item";
            this.clearItemButton.UseVisualStyleBackColor = true;
            // 
            // supplierLabel
            // 
            this.supplierLabel.AutoSize = true;
            this.supplierLabel.Location = new System.Drawing.Point(575, 8);
            this.supplierLabel.Name = "supplierLabel";
            this.supplierLabel.Size = new System.Drawing.Size(48, 13);
            this.supplierLabel.TabIndex = 13;
            this.supplierLabel.Text = "Supplier:";
            // 
            // criteriaLabel
            // 
            this.criteriaLabel.AutoSize = true;
            this.criteriaLabel.Location = new System.Drawing.Point(575, 27);
            this.criteriaLabel.Name = "criteriaLabel";
            this.criteriaLabel.Size = new System.Drawing.Size(76, 13);
            this.criteriaLabel.TabIndex = 14;
            this.criteriaLabel.Text = "Pre-Fill Criteria:";
            // 
            // OrderCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 450);
            this.Controls.Add(this.criteriaLabel);
            this.Controls.Add(this.supplierLabel);
            this.Controls.Add(this.clearItemButton);
            this.Controls.Add(this.addItemButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.qtyTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.productLineCB);
            this.Controls.Add(this.itemNumberTB);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.finalizeButton);
            this.Controls.Add(this.saveOrderButton);
            this.Controls.Add(this.deleteItemButton);
            this.Controls.Add(this.dataGridView1);
            this.Name = "OrderCreator";
            this.Text = "OrderCreator";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn productLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn retail;
        private System.Windows.Forms.DataGridViewTextBoxColumn extCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn extRetail;
        private System.Windows.Forms.Button deleteItemButton;
        private System.Windows.Forms.Button saveOrderButton;
        private System.Windows.Forms.Button finalizeButton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox itemNumberTB;
        private System.Windows.Forms.ComboBox productLineCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox qtyTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button addItemButton;
        private System.Windows.Forms.Button clearItemButton;
        private System.Windows.Forms.Label supplierLabel;
        private System.Windows.Forms.Label criteriaLabel;
    }
}