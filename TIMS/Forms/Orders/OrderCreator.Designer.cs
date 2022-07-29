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
            this.min = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.max = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.onHandQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.totalItemsTB = new System.Windows.Forms.TextBox();
            this.totalCostTB = new System.Windows.Forms.TextBox();
            this.totalRetailTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.potentialProfitTB = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.averageMarginTB = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.shippingCostTB = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemNumber,
            this.productLine,
            this.description,
            this.qty,
            this.min,
            this.max,
            this.onHandQty,
            this.cost,
            this.retail,
            this.extCost,
            this.extRetail});
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridView1.Location = new System.Drawing.Point(12, 52);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(953, 357);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
            this.dataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
            this.dataGridView1.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView1_RowsRemoved);
            // 
            // itemNumber
            // 
            this.itemNumber.FillWeight = 70.91371F;
            this.itemNumber.HeaderText = "Item Number";
            this.itemNumber.Name = "itemNumber";
            // 
            // productLine
            // 
            this.productLine.FillWeight = 70.91371F;
            this.productLine.HeaderText = "Product Line";
            this.productLine.Name = "productLine";
            // 
            // description
            // 
            this.description.FillWeight = 390.8629F;
            this.description.HeaderText = "Description";
            this.description.Name = "description";
            // 
            // qty
            // 
            this.qty.FillWeight = 70.91371F;
            this.qty.HeaderText = "Quantity";
            this.qty.Name = "qty";
            // 
            // min
            // 
            this.min.FillWeight = 70.91371F;
            this.min.HeaderText = "Min";
            this.min.Name = "min";
            // 
            // max
            // 
            this.max.FillWeight = 70.91371F;
            this.max.HeaderText = "Max";
            this.max.Name = "max";
            // 
            // onHandQty
            // 
            this.onHandQty.FillWeight = 70.91371F;
            this.onHandQty.HeaderText = "On Hand";
            this.onHandQty.Name = "onHandQty";
            // 
            // cost
            // 
            this.cost.FillWeight = 70.91371F;
            this.cost.HeaderText = "Cost";
            this.cost.Name = "cost";
            // 
            // retail
            // 
            this.retail.FillWeight = 70.91371F;
            this.retail.HeaderText = "Retail Price";
            this.retail.Name = "retail";
            // 
            // extCost
            // 
            this.extCost.FillWeight = 70.91371F;
            this.extCost.HeaderText = "Ext. Cost";
            this.extCost.Name = "extCost";
            // 
            // extRetail
            // 
            this.extRetail.FillWeight = 70.91371F;
            this.extRetail.HeaderText = "Ext. Retail";
            this.extRetail.Name = "extRetail";
            // 
            // deleteItemButton
            // 
            this.deleteItemButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteItemButton.Enabled = false;
            this.deleteItemButton.Location = new System.Drawing.Point(728, 476);
            this.deleteItemButton.Name = "deleteItemButton";
            this.deleteItemButton.Size = new System.Drawing.Size(75, 23);
            this.deleteItemButton.TabIndex = 8;
            this.deleteItemButton.Text = "Delete Item";
            this.deleteItemButton.UseVisualStyleBackColor = true;
            this.deleteItemButton.Click += new System.EventHandler(this.deleteItemButton_Click);
            // 
            // saveOrderButton
            // 
            this.saveOrderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveOrderButton.Enabled = false;
            this.saveOrderButton.Location = new System.Drawing.Point(890, 476);
            this.saveOrderButton.Name = "saveOrderButton";
            this.saveOrderButton.Size = new System.Drawing.Size(75, 23);
            this.saveOrderButton.TabIndex = 6;
            this.saveOrderButton.Text = "Save Order";
            this.saveOrderButton.UseVisualStyleBackColor = true;
            this.saveOrderButton.Click += new System.EventHandler(this.saveOrderButton_Click);
            // 
            // finalizeButton
            // 
            this.finalizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.finalizeButton.Enabled = false;
            this.finalizeButton.Location = new System.Drawing.Point(809, 476);
            this.finalizeButton.Name = "finalizeButton";
            this.finalizeButton.Size = new System.Drawing.Size(75, 23);
            this.finalizeButton.TabIndex = 7;
            this.finalizeButton.Text = "Finalize";
            this.finalizeButton.UseVisualStyleBackColor = true;
            this.finalizeButton.Click += new System.EventHandler(this.finalizeButton_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(12, 476);
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
            this.qtyTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.qtyTB_KeyDown);
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
            this.clearItemButton.Click += new System.EventHandler(this.clearItemButton_Click);
            // 
            // supplierLabel
            // 
            this.supplierLabel.AutoSize = true;
            this.supplierLabel.Location = new System.Drawing.Point(455, 8);
            this.supplierLabel.Name = "supplierLabel";
            this.supplierLabel.Size = new System.Drawing.Size(48, 13);
            this.supplierLabel.TabIndex = 13;
            this.supplierLabel.Text = "Supplier:";
            // 
            // criteriaLabel
            // 
            this.criteriaLabel.AutoSize = true;
            this.criteriaLabel.Location = new System.Drawing.Point(427, 27);
            this.criteriaLabel.Name = "criteriaLabel";
            this.criteriaLabel.Size = new System.Drawing.Size(76, 13);
            this.criteriaLabel.TabIndex = 14;
            this.criteriaLabel.Text = "Pre-Fill Criteria:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 415);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Total Items:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(188, 415);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Total Cost:";
            // 
            // totalItemsTB
            // 
            this.totalItemsTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.totalItemsTB.Enabled = false;
            this.totalItemsTB.Location = new System.Drawing.Point(80, 412);
            this.totalItemsTB.Name = "totalItemsTB";
            this.totalItemsTB.Size = new System.Drawing.Size(100, 20);
            this.totalItemsTB.TabIndex = 17;
            // 
            // totalCostTB
            // 
            this.totalCostTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.totalCostTB.Enabled = false;
            this.totalCostTB.Location = new System.Drawing.Point(252, 412);
            this.totalCostTB.Name = "totalCostTB";
            this.totalCostTB.Size = new System.Drawing.Size(100, 20);
            this.totalCostTB.TabIndex = 18;
            // 
            // totalRetailTB
            // 
            this.totalRetailTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.totalRetailTB.Enabled = false;
            this.totalRetailTB.Location = new System.Drawing.Point(422, 412);
            this.totalRetailTB.Name = "totalRetailTB";
            this.totalRetailTB.Size = new System.Drawing.Size(100, 20);
            this.totalRetailTB.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(358, 415);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Total Retail:";
            // 
            // potentialProfitTB
            // 
            this.potentialProfitTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.potentialProfitTB.Enabled = false;
            this.potentialProfitTB.Location = new System.Drawing.Point(612, 412);
            this.potentialProfitTB.Name = "potentialProfitTB";
            this.potentialProfitTB.Size = new System.Drawing.Size(100, 20);
            this.potentialProfitTB.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(528, 415);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Potential Profit:";
            // 
            // averageMarginTB
            // 
            this.averageMarginTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.averageMarginTB.Enabled = false;
            this.averageMarginTB.Location = new System.Drawing.Point(802, 412);
            this.averageMarginTB.Name = "averageMarginTB";
            this.averageMarginTB.Size = new System.Drawing.Size(100, 20);
            this.averageMarginTB.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(718, 415);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Average Margin:";
            // 
            // shippingCostTB
            // 
            this.shippingCostTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.shippingCostTB.Location = new System.Drawing.Point(612, 478);
            this.shippingCostTB.Name = "shippingCostTB";
            this.shippingCostTB.Size = new System.Drawing.Size(100, 20);
            this.shippingCostTB.TabIndex = 26;
            this.shippingCostTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(531, 481);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Shipping Cost:";
            // 
            // OrderCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 511);
            this.Controls.Add(this.shippingCostTB);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.averageMarginTB);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.potentialProfitTB);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.totalRetailTB);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.totalCostTB);
            this.Controls.Add(this.totalItemsTB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn itemNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn productLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn min;
        private System.Windows.Forms.DataGridViewTextBoxColumn max;
        private System.Windows.Forms.DataGridViewTextBoxColumn onHandQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn retail;
        private System.Windows.Forms.DataGridViewTextBoxColumn extCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn extRetail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox totalItemsTB;
        private System.Windows.Forms.TextBox totalCostTB;
        private System.Windows.Forms.TextBox totalRetailTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox potentialProfitTB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox averageMarginTB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox shippingCostTB;
        private System.Windows.Forms.Label label9;
    }
}