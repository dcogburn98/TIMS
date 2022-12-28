namespace TIMS.Forms.Maintenance
{
    partial class BarcodeMaintenance
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
            this.barcodeTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.qtyTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.itemNumberTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.productLineTB = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // barcodeTB
            // 
            this.barcodeTB.Location = new System.Drawing.Point(70, 12);
            this.barcodeTB.Name = "barcodeTB";
            this.barcodeTB.Size = new System.Drawing.Size(215, 20);
            this.barcodeTB.TabIndex = 0;
            this.barcodeTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.barcodeTB_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Barcode:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.closeButton);
            this.groupBox1.Controls.Add(this.saveButton);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.qtyTB);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.itemNumberTB);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.productLineTB);
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(283, 157);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Barcode Information";
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(6, 118);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 9;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(202, 118);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(78, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Qty:";
            // 
            // qtyTB
            // 
            this.qtyTB.Location = new System.Drawing.Point(110, 81);
            this.qtyTB.Name = "qtyTB";
            this.qtyTB.Size = new System.Drawing.Size(123, 20);
            this.qtyTB.TabIndex = 6;
            this.qtyTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.qtyTB_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Item Number:";
            // 
            // itemNumberTB
            // 
            this.itemNumberTB.Location = new System.Drawing.Point(110, 55);
            this.itemNumberTB.Name = "itemNumberTB";
            this.itemNumberTB.Size = new System.Drawing.Size(123, 20);
            this.itemNumberTB.TabIndex = 4;
            this.itemNumberTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.itemNumberTB_KeyDown);
            this.itemNumberTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.itemNumberTB_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Product Line:";
            // 
            // productLineTB
            // 
            this.productLineTB.Location = new System.Drawing.Point(110, 29);
            this.productLineTB.Name = "productLineTB";
            this.productLineTB.Size = new System.Drawing.Size(123, 20);
            this.productLineTB.TabIndex = 2;
            this.productLineTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.productLineTB_KeyDown);
            this.productLineTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.productLineTB_KeyPress);
            // 
            // BarcodeMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 206);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.barcodeTB);
            this.Name = "BarcodeMaintenance";
            this.Text = "BarcodeMaintenance";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox barcodeTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox qtyTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox itemNumberTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox productLineTB;
    }
}