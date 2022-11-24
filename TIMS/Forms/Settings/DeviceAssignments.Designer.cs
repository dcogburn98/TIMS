namespace TIMS.Forms.Settings
{
    partial class DeviceAssignments
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cancelChanges = new System.Windows.Forms.Button();
            this.saveChanges = new System.Windows.Forms.Button();
            this.terminalsLB = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.deleteLineDisplay = new System.Windows.Forms.Button();
            this.addLineDisplay = new System.Windows.Forms.Button();
            this.chooseLineDisplay = new System.Windows.Forms.Button();
            this.lineDisplaysLB = new System.Windows.Forms.ListBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.deleteCardReader = new System.Windows.Forms.Button();
            this.addCardReader = new System.Windows.Forms.Button();
            this.chooseCardReader = new System.Windows.Forms.Button();
            this.cardReadersLB = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.deleteInvoicePrinter = new System.Windows.Forms.Button();
            this.addInvoicePrinter = new System.Windows.Forms.Button();
            this.chooseInvoicePrinter = new System.Windows.Forms.Button();
            this.invoicePrintersLB = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.deleteReceiptPrinter = new System.Windows.Forms.Button();
            this.addReceiptPrinter = new System.Windows.Forms.Button();
            this.chooseReceiptPrinter = new System.Windows.Forms.Button();
            this.receiptPrintersLB = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cancelChanges);
            this.groupBox1.Controls.Add(this.saveChanges);
            this.groupBox1.Controls.Add(this.terminalsLB);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 618);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Terminals";
            // 
            // cancelChanges
            // 
            this.cancelChanges.Location = new System.Drawing.Point(177, 588);
            this.cancelChanges.Name = "cancelChanges";
            this.cancelChanges.Size = new System.Drawing.Size(107, 23);
            this.cancelChanges.TabIndex = 2;
            this.cancelChanges.Text = "Cancel Changes";
            this.cancelChanges.UseVisualStyleBackColor = true;
            // 
            // saveChanges
            // 
            this.saveChanges.Location = new System.Drawing.Point(290, 588);
            this.saveChanges.Name = "saveChanges";
            this.saveChanges.Size = new System.Drawing.Size(107, 23);
            this.saveChanges.TabIndex = 1;
            this.saveChanges.Text = "Save Configuration";
            this.saveChanges.UseVisualStyleBackColor = true;
            // 
            // terminalsLB
            // 
            this.terminalsLB.FormattingEnabled = true;
            this.terminalsLB.Location = new System.Drawing.Point(6, 19);
            this.terminalsLB.Name = "terminalsLB";
            this.terminalsLB.Size = new System.Drawing.Size(391, 563);
            this.terminalsLB.TabIndex = 0;
            this.terminalsLB.SelectedIndexChanged += new System.EventHandler(this.terminalsLB_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(421, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(633, 618);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Terminal Devices";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.deleteLineDisplay);
            this.groupBox6.Controls.Add(this.addLineDisplay);
            this.groupBox6.Controls.Add(this.chooseLineDisplay);
            this.groupBox6.Controls.Add(this.lineDisplaysLB);
            this.groupBox6.Location = new System.Drawing.Point(320, 318);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(307, 294);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Line Displays";
            // 
            // deleteLineDisplay
            // 
            this.deleteLineDisplay.Location = new System.Drawing.Point(6, 260);
            this.deleteLineDisplay.Name = "deleteLineDisplay";
            this.deleteLineDisplay.Size = new System.Drawing.Size(92, 23);
            this.deleteLineDisplay.TabIndex = 5;
            this.deleteLineDisplay.Text = "Delete Device";
            this.deleteLineDisplay.UseVisualStyleBackColor = true;
            // 
            // addLineDisplay
            // 
            this.addLineDisplay.Location = new System.Drawing.Point(111, 260);
            this.addLineDisplay.Name = "addLineDisplay";
            this.addLineDisplay.Size = new System.Drawing.Size(92, 23);
            this.addLineDisplay.TabIndex = 4;
            this.addLineDisplay.Text = "Add Device";
            this.addLineDisplay.UseVisualStyleBackColor = true;
            this.addLineDisplay.Click += new System.EventHandler(this.addLineDisplay_Click);
            // 
            // chooseLineDisplay
            // 
            this.chooseLineDisplay.Location = new System.Drawing.Point(209, 260);
            this.chooseLineDisplay.Name = "chooseLineDisplay";
            this.chooseLineDisplay.Size = new System.Drawing.Size(92, 23);
            this.chooseLineDisplay.TabIndex = 3;
            this.chooseLineDisplay.Text = "Choose Device";
            this.chooseLineDisplay.UseVisualStyleBackColor = true;
            // 
            // lineDisplaysLB
            // 
            this.lineDisplaysLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lineDisplaysLB.FormattingEnabled = true;
            this.lineDisplaysLB.Location = new System.Drawing.Point(6, 16);
            this.lineDisplaysLB.Name = "lineDisplaysLB";
            this.lineDisplaysLB.Size = new System.Drawing.Size(296, 238);
            this.lineDisplaysLB.TabIndex = 2;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.deleteCardReader);
            this.groupBox5.Controls.Add(this.addCardReader);
            this.groupBox5.Controls.Add(this.chooseCardReader);
            this.groupBox5.Controls.Add(this.cardReadersLB);
            this.groupBox5.Location = new System.Drawing.Point(6, 318);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(308, 294);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Card Readers";
            // 
            // deleteCardReader
            // 
            this.deleteCardReader.Location = new System.Drawing.Point(6, 260);
            this.deleteCardReader.Name = "deleteCardReader";
            this.deleteCardReader.Size = new System.Drawing.Size(92, 23);
            this.deleteCardReader.TabIndex = 5;
            this.deleteCardReader.Text = "Delete Device";
            this.deleteCardReader.UseVisualStyleBackColor = true;
            this.deleteCardReader.Click += new System.EventHandler(this.deleteCardReader_Click);
            // 
            // addCardReader
            // 
            this.addCardReader.Location = new System.Drawing.Point(112, 260);
            this.addCardReader.Name = "addCardReader";
            this.addCardReader.Size = new System.Drawing.Size(92, 23);
            this.addCardReader.TabIndex = 4;
            this.addCardReader.Text = "Add Device";
            this.addCardReader.UseVisualStyleBackColor = true;
            this.addCardReader.Click += new System.EventHandler(this.addCardReader_Click);
            // 
            // chooseCardReader
            // 
            this.chooseCardReader.Location = new System.Drawing.Point(210, 260);
            this.chooseCardReader.Name = "chooseCardReader";
            this.chooseCardReader.Size = new System.Drawing.Size(92, 23);
            this.chooseCardReader.TabIndex = 3;
            this.chooseCardReader.Text = "Choose Device";
            this.chooseCardReader.UseVisualStyleBackColor = true;
            this.chooseCardReader.Click += new System.EventHandler(this.chooseCardReader_Click);
            // 
            // cardReadersLB
            // 
            this.cardReadersLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cardReadersLB.FormattingEnabled = true;
            this.cardReadersLB.Location = new System.Drawing.Point(6, 16);
            this.cardReadersLB.Name = "cardReadersLB";
            this.cardReadersLB.Size = new System.Drawing.Size(296, 238);
            this.cardReadersLB.TabIndex = 2;
            this.cardReadersLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cardReadersLB_DrawItem);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.deleteInvoicePrinter);
            this.groupBox4.Controls.Add(this.addInvoicePrinter);
            this.groupBox4.Controls.Add(this.chooseInvoicePrinter);
            this.groupBox4.Controls.Add(this.invoicePrintersLB);
            this.groupBox4.Location = new System.Drawing.Point(320, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(308, 293);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Invoice Printers";
            // 
            // deleteInvoicePrinter
            // 
            this.deleteInvoicePrinter.Location = new System.Drawing.Point(6, 260);
            this.deleteInvoicePrinter.Name = "deleteInvoicePrinter";
            this.deleteInvoicePrinter.Size = new System.Drawing.Size(92, 23);
            this.deleteInvoicePrinter.TabIndex = 5;
            this.deleteInvoicePrinter.Text = "Delete Device";
            this.deleteInvoicePrinter.UseVisualStyleBackColor = true;
            this.deleteInvoicePrinter.Click += new System.EventHandler(this.deleteInvoicePrinter_Click);
            // 
            // addInvoicePrinter
            // 
            this.addInvoicePrinter.Location = new System.Drawing.Point(111, 260);
            this.addInvoicePrinter.Name = "addInvoicePrinter";
            this.addInvoicePrinter.Size = new System.Drawing.Size(92, 23);
            this.addInvoicePrinter.TabIndex = 4;
            this.addInvoicePrinter.Text = "Add Device";
            this.addInvoicePrinter.UseVisualStyleBackColor = true;
            this.addInvoicePrinter.Click += new System.EventHandler(this.addInvoicePrinter_Click);
            // 
            // chooseInvoicePrinter
            // 
            this.chooseInvoicePrinter.Location = new System.Drawing.Point(209, 260);
            this.chooseInvoicePrinter.Name = "chooseInvoicePrinter";
            this.chooseInvoicePrinter.Size = new System.Drawing.Size(92, 23);
            this.chooseInvoicePrinter.TabIndex = 3;
            this.chooseInvoicePrinter.Text = "Choose Device";
            this.chooseInvoicePrinter.UseVisualStyleBackColor = true;
            this.chooseInvoicePrinter.Click += new System.EventHandler(this.chooseInvoicePrinter_Click);
            // 
            // invoicePrintersLB
            // 
            this.invoicePrintersLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.invoicePrintersLB.FormattingEnabled = true;
            this.invoicePrintersLB.Location = new System.Drawing.Point(6, 16);
            this.invoicePrintersLB.Name = "invoicePrintersLB";
            this.invoicePrintersLB.Size = new System.Drawing.Size(296, 238);
            this.invoicePrintersLB.TabIndex = 2;
            this.invoicePrintersLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.invoicePrintersLB_DrawItem);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.deleteReceiptPrinter);
            this.groupBox3.Controls.Add(this.addReceiptPrinter);
            this.groupBox3.Controls.Add(this.chooseReceiptPrinter);
            this.groupBox3.Controls.Add(this.receiptPrintersLB);
            this.groupBox3.Location = new System.Drawing.Point(6, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(308, 293);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Receipt Printers";
            // 
            // deleteReceiptPrinter
            // 
            this.deleteReceiptPrinter.Location = new System.Drawing.Point(8, 260);
            this.deleteReceiptPrinter.Name = "deleteReceiptPrinter";
            this.deleteReceiptPrinter.Size = new System.Drawing.Size(92, 23);
            this.deleteReceiptPrinter.TabIndex = 4;
            this.deleteReceiptPrinter.Text = "Delete Device";
            this.deleteReceiptPrinter.UseVisualStyleBackColor = true;
            this.deleteReceiptPrinter.Click += new System.EventHandler(this.deleteReceiptPrinter_Click);
            // 
            // addReceiptPrinter
            // 
            this.addReceiptPrinter.Location = new System.Drawing.Point(112, 260);
            this.addReceiptPrinter.Name = "addReceiptPrinter";
            this.addReceiptPrinter.Size = new System.Drawing.Size(92, 23);
            this.addReceiptPrinter.TabIndex = 3;
            this.addReceiptPrinter.Text = "Add Device";
            this.addReceiptPrinter.UseVisualStyleBackColor = true;
            this.addReceiptPrinter.Click += new System.EventHandler(this.addReceiptPrinter_Click);
            // 
            // chooseReceiptPrinter
            // 
            this.chooseReceiptPrinter.Location = new System.Drawing.Point(210, 260);
            this.chooseReceiptPrinter.Name = "chooseReceiptPrinter";
            this.chooseReceiptPrinter.Size = new System.Drawing.Size(92, 23);
            this.chooseReceiptPrinter.TabIndex = 2;
            this.chooseReceiptPrinter.Text = "Choose Device";
            this.chooseReceiptPrinter.UseVisualStyleBackColor = true;
            this.chooseReceiptPrinter.Click += new System.EventHandler(this.chooseReceiptPrinter_Click);
            // 
            // receiptPrintersLB
            // 
            this.receiptPrintersLB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.receiptPrintersLB.FormattingEnabled = true;
            this.receiptPrintersLB.Location = new System.Drawing.Point(6, 16);
            this.receiptPrintersLB.Name = "receiptPrintersLB";
            this.receiptPrintersLB.Size = new System.Drawing.Size(296, 238);
            this.receiptPrintersLB.TabIndex = 1;
            this.receiptPrintersLB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.receiptPrintersLB_DrawItem);
            // 
            // DeviceAssignments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 642);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DeviceAssignments";
            this.Text = "DeviceAssignments";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button cancelChanges;
        private System.Windows.Forms.Button saveChanges;
        private System.Windows.Forms.ListBox terminalsLB;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button chooseLineDisplay;
        private System.Windows.Forms.ListBox lineDisplaysLB;
        private System.Windows.Forms.Button chooseCardReader;
        private System.Windows.Forms.ListBox cardReadersLB;
        private System.Windows.Forms.Button chooseInvoicePrinter;
        private System.Windows.Forms.ListBox invoicePrintersLB;
        private System.Windows.Forms.Button chooseReceiptPrinter;
        private System.Windows.Forms.ListBox receiptPrintersLB;
        private System.Windows.Forms.Button addLineDisplay;
        private System.Windows.Forms.Button addCardReader;
        private System.Windows.Forms.Button addInvoicePrinter;
        private System.Windows.Forms.Button addReceiptPrinter;
        private System.Windows.Forms.Button deleteLineDisplay;
        private System.Windows.Forms.Button deleteCardReader;
        private System.Windows.Forms.Button deleteInvoicePrinter;
        private System.Windows.Forms.Button deleteReceiptPrinter;
    }
}