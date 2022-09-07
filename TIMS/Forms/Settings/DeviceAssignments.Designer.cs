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
            this.addLineDisplay = new System.Windows.Forms.Button();
            this.chooseLineDisplay = new System.Windows.Forms.Button();
            this.lineDisplaysLB = new System.Windows.Forms.ListBox();
            this.lineDisplayUSB = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.addCardReader = new System.Windows.Forms.Button();
            this.chooseCardReader = new System.Windows.Forms.Button();
            this.cardReadersLB = new System.Windows.Forms.ListBox();
            this.cardReaderUSB = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.addInvoicePrinter = new System.Windows.Forms.Button();
            this.chooseInvoicePrinter = new System.Windows.Forms.Button();
            this.invoicePrintersLB = new System.Windows.Forms.ListBox();
            this.invoicePrinterUSB = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.addReceiptPrinter = new System.Windows.Forms.Button();
            this.chooseReceiptPrinter = new System.Windows.Forms.Button();
            this.receiptPrintersLB = new System.Windows.Forms.ListBox();
            this.receiptPrinterUSB = new System.Windows.Forms.CheckBox();
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
            this.groupBox6.Controls.Add(this.addLineDisplay);
            this.groupBox6.Controls.Add(this.chooseLineDisplay);
            this.groupBox6.Controls.Add(this.lineDisplaysLB);
            this.groupBox6.Controls.Add(this.lineDisplayUSB);
            this.groupBox6.Location = new System.Drawing.Point(320, 318);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(307, 294);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Line Displays";
            // 
            // addLineDisplay
            // 
            this.addLineDisplay.Location = new System.Drawing.Point(111, 260);
            this.addLineDisplay.Name = "addLineDisplay";
            this.addLineDisplay.Size = new System.Drawing.Size(92, 23);
            this.addLineDisplay.TabIndex = 4;
            this.addLineDisplay.Text = "Add Device";
            this.addLineDisplay.UseVisualStyleBackColor = true;
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
            this.lineDisplaysLB.FormattingEnabled = true;
            this.lineDisplaysLB.Location = new System.Drawing.Point(6, 42);
            this.lineDisplaysLB.Name = "lineDisplaysLB";
            this.lineDisplaysLB.Size = new System.Drawing.Size(296, 212);
            this.lineDisplaysLB.TabIndex = 2;
            // 
            // lineDisplayUSB
            // 
            this.lineDisplayUSB.AutoSize = true;
            this.lineDisplayUSB.Location = new System.Drawing.Point(6, 19);
            this.lineDisplayUSB.Name = "lineDisplayUSB";
            this.lineDisplayUSB.Size = new System.Drawing.Size(94, 17);
            this.lineDisplayUSB.TabIndex = 1;
            this.lineDisplayUSB.Text = "USB Attached";
            this.lineDisplayUSB.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.addCardReader);
            this.groupBox5.Controls.Add(this.chooseCardReader);
            this.groupBox5.Controls.Add(this.cardReadersLB);
            this.groupBox5.Controls.Add(this.cardReaderUSB);
            this.groupBox5.Location = new System.Drawing.Point(6, 318);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(308, 294);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Card Readers";
            // 
            // addCardReader
            // 
            this.addCardReader.Location = new System.Drawing.Point(112, 260);
            this.addCardReader.Name = "addCardReader";
            this.addCardReader.Size = new System.Drawing.Size(92, 23);
            this.addCardReader.TabIndex = 4;
            this.addCardReader.Text = "Add Device";
            this.addCardReader.UseVisualStyleBackColor = true;
            // 
            // chooseCardReader
            // 
            this.chooseCardReader.Location = new System.Drawing.Point(210, 260);
            this.chooseCardReader.Name = "chooseCardReader";
            this.chooseCardReader.Size = new System.Drawing.Size(92, 23);
            this.chooseCardReader.TabIndex = 3;
            this.chooseCardReader.Text = "Choose Device";
            this.chooseCardReader.UseVisualStyleBackColor = true;
            // 
            // cardReadersLB
            // 
            this.cardReadersLB.FormattingEnabled = true;
            this.cardReadersLB.Location = new System.Drawing.Point(6, 42);
            this.cardReadersLB.Name = "cardReadersLB";
            this.cardReadersLB.Size = new System.Drawing.Size(296, 212);
            this.cardReadersLB.TabIndex = 2;
            // 
            // cardReaderUSB
            // 
            this.cardReaderUSB.AutoSize = true;
            this.cardReaderUSB.Location = new System.Drawing.Point(6, 19);
            this.cardReaderUSB.Name = "cardReaderUSB";
            this.cardReaderUSB.Size = new System.Drawing.Size(94, 17);
            this.cardReaderUSB.TabIndex = 1;
            this.cardReaderUSB.Text = "USB Attached";
            this.cardReaderUSB.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.addInvoicePrinter);
            this.groupBox4.Controls.Add(this.chooseInvoicePrinter);
            this.groupBox4.Controls.Add(this.invoicePrintersLB);
            this.groupBox4.Controls.Add(this.invoicePrinterUSB);
            this.groupBox4.Location = new System.Drawing.Point(320, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(308, 293);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Invoice Printers";
            // 
            // addInvoicePrinter
            // 
            this.addInvoicePrinter.Location = new System.Drawing.Point(111, 260);
            this.addInvoicePrinter.Name = "addInvoicePrinter";
            this.addInvoicePrinter.Size = new System.Drawing.Size(92, 23);
            this.addInvoicePrinter.TabIndex = 4;
            this.addInvoicePrinter.Text = "Add Device";
            this.addInvoicePrinter.UseVisualStyleBackColor = true;
            // 
            // chooseInvoicePrinter
            // 
            this.chooseInvoicePrinter.Location = new System.Drawing.Point(209, 260);
            this.chooseInvoicePrinter.Name = "chooseInvoicePrinter";
            this.chooseInvoicePrinter.Size = new System.Drawing.Size(92, 23);
            this.chooseInvoicePrinter.TabIndex = 3;
            this.chooseInvoicePrinter.Text = "Choose Device";
            this.chooseInvoicePrinter.UseVisualStyleBackColor = true;
            // 
            // invoicePrintersLB
            // 
            this.invoicePrintersLB.FormattingEnabled = true;
            this.invoicePrintersLB.Location = new System.Drawing.Point(6, 42);
            this.invoicePrintersLB.Name = "invoicePrintersLB";
            this.invoicePrintersLB.Size = new System.Drawing.Size(296, 212);
            this.invoicePrintersLB.TabIndex = 2;
            // 
            // invoicePrinterUSB
            // 
            this.invoicePrinterUSB.AutoSize = true;
            this.invoicePrinterUSB.Location = new System.Drawing.Point(6, 19);
            this.invoicePrinterUSB.Name = "invoicePrinterUSB";
            this.invoicePrinterUSB.Size = new System.Drawing.Size(94, 17);
            this.invoicePrinterUSB.TabIndex = 1;
            this.invoicePrinterUSB.Text = "USB Attached";
            this.invoicePrinterUSB.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.addReceiptPrinter);
            this.groupBox3.Controls.Add(this.chooseReceiptPrinter);
            this.groupBox3.Controls.Add(this.receiptPrintersLB);
            this.groupBox3.Controls.Add(this.receiptPrinterUSB);
            this.groupBox3.Location = new System.Drawing.Point(6, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(308, 293);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Receipt Printers";
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
            // 
            // receiptPrintersLB
            // 
            this.receiptPrintersLB.FormattingEnabled = true;
            this.receiptPrintersLB.Location = new System.Drawing.Point(6, 42);
            this.receiptPrintersLB.Name = "receiptPrintersLB";
            this.receiptPrintersLB.Size = new System.Drawing.Size(296, 212);
            this.receiptPrintersLB.TabIndex = 1;
            // 
            // receiptPrinterUSB
            // 
            this.receiptPrinterUSB.AutoSize = true;
            this.receiptPrinterUSB.Location = new System.Drawing.Point(6, 19);
            this.receiptPrinterUSB.Name = "receiptPrinterUSB";
            this.receiptPrinterUSB.Size = new System.Drawing.Size(94, 17);
            this.receiptPrinterUSB.TabIndex = 0;
            this.receiptPrinterUSB.Text = "USB Attached";
            this.receiptPrinterUSB.UseVisualStyleBackColor = true;
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
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
        private System.Windows.Forms.CheckBox lineDisplayUSB;
        private System.Windows.Forms.Button chooseCardReader;
        private System.Windows.Forms.ListBox cardReadersLB;
        private System.Windows.Forms.CheckBox cardReaderUSB;
        private System.Windows.Forms.Button chooseInvoicePrinter;
        private System.Windows.Forms.ListBox invoicePrintersLB;
        private System.Windows.Forms.CheckBox invoicePrinterUSB;
        private System.Windows.Forms.Button chooseReceiptPrinter;
        private System.Windows.Forms.ListBox receiptPrintersLB;
        private System.Windows.Forms.CheckBox receiptPrinterUSB;
        private System.Windows.Forms.Button addLineDisplay;
        private System.Windows.Forms.Button addCardReader;
        private System.Windows.Forms.Button addInvoicePrinter;
        private System.Windows.Forms.Button addReceiptPrinter;
    }
}