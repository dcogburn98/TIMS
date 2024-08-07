﻿namespace TIMS.Forms
{
    partial class Checkout
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
            this.changeDueLbl = new System.Windows.Forms.Label();
            this.cancelPaymentBtn = new System.Windows.Forms.Button();
            this.finalizeBtn = new System.Windows.Forms.Button();
            this.acceptPaymentBtn = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.checkNameTB = new System.Windows.Forms.TextBox();
            this.checkNumberTB = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.paymentAmountTB = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.poTB = new System.Windows.Forms.TextBox();
            this.attentionTB = new System.Windows.Forms.TextBox();
            this.saveInvoiceBtn = new System.Windows.Forms.Button();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.customerBirthdaySelector = new System.Windows.Forms.DateTimePicker();
            this.ageRestrictedTB = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.itemCountTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.taxRateTB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.totalTB = new System.Windows.Forms.TextBox();
            this.taxAmtTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.taxableTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.subtotalTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.paymentIndexTB = new System.Windows.Forms.TextBox();
            this.deletePaymentBtn = new System.Windows.Forms.Button();
            this.remainingBalanceTB = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.paymentsLB = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.paymentTypeLB = new System.Windows.Forms.ListBox();
            this.returnInvoiceBtn = new System.Windows.Forms.Button();
            this.printBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.changeDueLbl);
            this.groupBox1.Controls.Add(this.cancelPaymentBtn);
            this.groupBox1.Controls.Add(this.finalizeBtn);
            this.groupBox1.Controls.Add(this.acceptPaymentBtn);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.checkNameTB);
            this.groupBox1.Controls.Add(this.checkNumberTB);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.paymentAmountTB);
            this.groupBox1.Location = new System.Drawing.Point(364, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 384);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Finalize";
            // 
            // changeDueLbl
            // 
            this.changeDueLbl.AutoSize = true;
            this.changeDueLbl.Location = new System.Drawing.Point(87, 360);
            this.changeDueLbl.Name = "changeDueLbl";
            this.changeDueLbl.Size = new System.Drawing.Size(70, 13);
            this.changeDueLbl.TabIndex = 9;
            this.changeDueLbl.Text = "Change Due:";
            // 
            // cancelPaymentBtn
            // 
            this.cancelPaymentBtn.Location = new System.Drawing.Point(11, 181);
            this.cancelPaymentBtn.Name = "cancelPaymentBtn";
            this.cancelPaymentBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelPaymentBtn.TabIndex = 8;
            this.cancelPaymentBtn.Text = "Cancel";
            this.cancelPaymentBtn.UseVisualStyleBackColor = true;
            this.cancelPaymentBtn.Click += new System.EventHandler(this.cancelPaymentBtn_Click);
            // 
            // finalizeBtn
            // 
            this.finalizeBtn.Enabled = false;
            this.finalizeBtn.Location = new System.Drawing.Point(6, 355);
            this.finalizeBtn.Name = "finalizeBtn";
            this.finalizeBtn.Size = new System.Drawing.Size(75, 23);
            this.finalizeBtn.TabIndex = 7;
            this.finalizeBtn.Text = "Finalize";
            this.finalizeBtn.UseVisualStyleBackColor = true;
            this.finalizeBtn.Click += new System.EventHandler(this.finalizeBtn_Click);
            // 
            // acceptPaymentBtn
            // 
            this.acceptPaymentBtn.Location = new System.Drawing.Point(217, 181);
            this.acceptPaymentBtn.Name = "acceptPaymentBtn";
            this.acceptPaymentBtn.Size = new System.Drawing.Size(75, 23);
            this.acceptPaymentBtn.TabIndex = 12;
            this.acceptPaymentBtn.Text = "Accept";
            this.acceptPaymentBtn.UseVisualStyleBackColor = true;
            this.acceptPaymentBtn.Click += new System.EventHandler(this.acceptPaymentBtn_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 157);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(89, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Name On Check:";
            // 
            // checkNameTB
            // 
            this.checkNameTB.Location = new System.Drawing.Point(103, 154);
            this.checkNameTB.Name = "checkNameTB";
            this.checkNameTB.Size = new System.Drawing.Size(189, 20);
            this.checkNameTB.TabIndex = 11;
            this.checkNameTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.checkNameTB_KeyDown);
            // 
            // checkNumberTB
            // 
            this.checkNumberTB.Location = new System.Drawing.Point(103, 128);
            this.checkNumberTB.Name = "checkNumberTB";
            this.checkNumberTB.Size = new System.Drawing.Size(189, 20);
            this.checkNumberTB.TabIndex = 10;
            this.checkNumberTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.checkNumberTB_KeyDown);
            this.checkNumberTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.checkNumberTB_KeyPress);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 131);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Check Number:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 105);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(90, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Payment Amount;";
            // 
            // paymentAmountTB
            // 
            this.paymentAmountTB.Location = new System.Drawing.Point(103, 102);
            this.paymentAmountTB.Name = "paymentAmountTB";
            this.paymentAmountTB.Size = new System.Drawing.Size(189, 20);
            this.paymentAmountTB.TabIndex = 9;
            this.paymentAmountTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.paymentAmountTB_KeyDown);
            this.paymentAmountTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.paymentAmountTB_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.poTB);
            this.groupBox2.Controls.Add(this.attentionTB);
            this.groupBox2.Controls.Add(this.saveInvoiceBtn);
            this.groupBox2.Controls.Add(this.acceptBtn);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.customerBirthdaySelector);
            this.groupBox2.Controls.Add(this.ageRestrictedTB);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.itemCountTB);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.taxRateTB);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.totalTB);
            this.groupBox2.Controls.Add(this.taxAmtTB);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.taxableTB);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.subtotalTB);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(13, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(345, 231);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Review";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(7, 180);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 21;
            this.label16.Text = "PO #:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(7, 153);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(52, 13);
            this.label15.TabIndex = 20;
            this.label15.Text = "Attention:";
            // 
            // poTB
            // 
            this.poTB.Location = new System.Drawing.Point(70, 177);
            this.poTB.Name = "poTB";
            this.poTB.Size = new System.Drawing.Size(222, 20);
            this.poTB.TabIndex = 19;
            // 
            // attentionTB
            // 
            this.attentionTB.Location = new System.Drawing.Point(70, 150);
            this.attentionTB.Name = "attentionTB";
            this.attentionTB.Size = new System.Drawing.Size(222, 20);
            this.attentionTB.TabIndex = 18;
            // 
            // saveInvoiceBtn
            // 
            this.saveInvoiceBtn.Location = new System.Drawing.Point(111, 202);
            this.saveInvoiceBtn.Name = "saveInvoiceBtn";
            this.saveInvoiceBtn.Size = new System.Drawing.Size(98, 23);
            this.saveInvoiceBtn.TabIndex = 17;
            this.saveInvoiceBtn.Text = "Save Invoice";
            this.saveInvoiceBtn.UseVisualStyleBackColor = true;
            this.saveInvoiceBtn.Click += new System.EventHandler(this.saveInvoiceBtn_Click);
            // 
            // acceptBtn
            // 
            this.acceptBtn.Location = new System.Drawing.Point(217, 202);
            this.acceptBtn.Name = "acceptBtn";
            this.acceptBtn.Size = new System.Drawing.Size(75, 23);
            this.acceptBtn.TabIndex = 16;
            this.acceptBtn.Text = "Accept";
            this.acceptBtn.UseVisualStyleBackColor = true;
            this.acceptBtn.Click += new System.EventHandler(this.acceptBtn_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Birth Date:";
            // 
            // customerBirthdaySelector
            // 
            this.customerBirthdaySelector.CalendarForeColor = System.Drawing.Color.Black;
            this.customerBirthdaySelector.CalendarMonthBackground = System.Drawing.SystemColors.Menu;
            this.customerBirthdaySelector.CalendarTitleForeColor = System.Drawing.Color.Black;
            this.customerBirthdaySelector.Enabled = false;
            this.customerBirthdaySelector.Location = new System.Drawing.Point(70, 124);
            this.customerBirthdaySelector.Name = "customerBirthdaySelector";
            this.customerBirthdaySelector.Size = new System.Drawing.Size(222, 20);
            this.customerBirthdaySelector.TabIndex = 14;
            // 
            // ageRestrictedTB
            // 
            this.ageRestrictedTB.BackColor = System.Drawing.Color.Yellow;
            this.ageRestrictedTB.Enabled = false;
            this.ageRestrictedTB.Location = new System.Drawing.Point(250, 98);
            this.ageRestrictedTB.Name = "ageRestrictedTB";
            this.ageRestrictedTB.Size = new System.Drawing.Size(84, 20);
            this.ageRestrictedTB.TabIndex = 13;
            this.ageRestrictedTB.Text = "False";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(80, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(164, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Purchasing Age Restricted Items:";
            // 
            // itemCountTB
            // 
            this.itemCountTB.BackColor = System.Drawing.Color.Yellow;
            this.itemCountTB.Enabled = false;
            this.itemCountTB.Location = new System.Drawing.Point(250, 71);
            this.itemCountTB.Name = "itemCountTB";
            this.itemCountTB.Size = new System.Drawing.Size(55, 20);
            this.itemCountTB.TabIndex = 11;
            this.itemCountTB.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(169, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Total Items:";
            // 
            // taxRateTB
            // 
            this.taxRateTB.BackColor = System.Drawing.Color.Yellow;
            this.taxRateTB.Enabled = false;
            this.taxRateTB.Location = new System.Drawing.Point(250, 45);
            this.taxRateTB.Name = "taxRateTB";
            this.taxRateTB.Size = new System.Drawing.Size(55, 20);
            this.taxRateTB.TabIndex = 9;
            this.taxRateTB.Text = "10.25%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(169, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Tax Rate:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Total:";
            // 
            // totalTB
            // 
            this.totalTB.BackColor = System.Drawing.Color.Yellow;
            this.totalTB.Enabled = false;
            this.totalTB.Location = new System.Drawing.Point(62, 71);
            this.totalTB.Name = "totalTB";
            this.totalTB.Size = new System.Drawing.Size(100, 20);
            this.totalTB.TabIndex = 6;
            this.totalTB.Text = "$0.00";
            // 
            // taxAmtTB
            // 
            this.taxAmtTB.BackColor = System.Drawing.Color.Yellow;
            this.taxAmtTB.Enabled = false;
            this.taxAmtTB.Location = new System.Drawing.Point(62, 45);
            this.taxAmtTB.Name = "taxAmtTB";
            this.taxAmtTB.Size = new System.Drawing.Size(100, 20);
            this.taxAmtTB.TabIndex = 5;
            this.taxAmtTB.Text = "$0.00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tax:";
            // 
            // taxableTB
            // 
            this.taxableTB.BackColor = System.Drawing.Color.Yellow;
            this.taxableTB.Enabled = false;
            this.taxableTB.Location = new System.Drawing.Point(250, 17);
            this.taxableTB.Name = "taxableTB";
            this.taxableTB.Size = new System.Drawing.Size(84, 20);
            this.taxableTB.TabIndex = 3;
            this.taxableTB.Text = "True";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(169, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Taxable Total:";
            // 
            // subtotalTB
            // 
            this.subtotalTB.BackColor = System.Drawing.Color.Yellow;
            this.subtotalTB.Enabled = false;
            this.subtotalTB.Location = new System.Drawing.Point(62, 17);
            this.subtotalTB.Name = "subtotalTB";
            this.subtotalTB.Size = new System.Drawing.Size(100, 20);
            this.subtotalTB.TabIndex = 1;
            this.subtotalTB.Text = "$0.00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Subtotal:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.paymentIndexTB);
            this.groupBox3.Controls.Add(this.deletePaymentBtn);
            this.groupBox3.Controls.Add(this.remainingBalanceTB);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.paymentsLB);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.paymentTypeLB);
            this.groupBox3.Location = new System.Drawing.Point(13, 250);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(345, 188);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Payment Method";
            // 
            // paymentIndexTB
            // 
            this.paymentIndexTB.Location = new System.Drawing.Point(6, 132);
            this.paymentIndexTB.Name = "paymentIndexTB";
            this.paymentIndexTB.Size = new System.Drawing.Size(29, 20);
            this.paymentIndexTB.TabIndex = 8;
            this.paymentIndexTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.paymentIndexTB_KeyDown);
            this.paymentIndexTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.paymentIndexTB_KeyPress);
            // 
            // deletePaymentBtn
            // 
            this.deletePaymentBtn.Enabled = false;
            this.deletePaymentBtn.Location = new System.Drawing.Point(215, 130);
            this.deletePaymentBtn.Name = "deletePaymentBtn";
            this.deletePaymentBtn.Size = new System.Drawing.Size(75, 23);
            this.deletePaymentBtn.TabIndex = 7;
            this.deletePaymentBtn.Text = "Delete";
            this.deletePaymentBtn.UseVisualStyleBackColor = true;
            this.deletePaymentBtn.Click += new System.EventHandler(this.deletePaymentBtn_Click);
            // 
            // remainingBalanceTB
            // 
            this.remainingBalanceTB.Enabled = false;
            this.remainingBalanceTB.Location = new System.Drawing.Point(111, 162);
            this.remainingBalanceTB.Name = "remainingBalanceTB";
            this.remainingBalanceTB.Size = new System.Drawing.Size(181, 20);
            this.remainingBalanceTB.TabIndex = 6;
            this.remainingBalanceTB.Text = "$0.00";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 165);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(102, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Remaining Balance:";
            // 
            // paymentsLB
            // 
            this.paymentsLB.FormattingEnabled = true;
            this.paymentsLB.Items.AddRange(new object[] {
            "Cash: $34.00",
            "Payment Card: $28.41"});
            this.paymentsLB.Location = new System.Drawing.Point(154, 32);
            this.paymentsLB.Name = "paymentsLB";
            this.paymentsLB.Size = new System.Drawing.Size(138, 95);
            this.paymentsLB.TabIndex = 3;
            this.paymentsLB.SelectedIndexChanged += new System.EventHandler(this.paymentsLB_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(169, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Tendered Payments";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(37, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Payment Type";
            // 
            // paymentTypeLB
            // 
            this.paymentTypeLB.Enabled = false;
            this.paymentTypeLB.FormattingEnabled = true;
            this.paymentTypeLB.Items.AddRange(new object[] {
            "Cash",
            "Check",
            "Payment Card",
            "Charge"});
            this.paymentTypeLB.Location = new System.Drawing.Point(6, 32);
            this.paymentTypeLB.Name = "paymentTypeLB";
            this.paymentTypeLB.Size = new System.Drawing.Size(142, 95);
            this.paymentTypeLB.TabIndex = 0;
            this.paymentTypeLB.DoubleClick += new System.EventHandler(this.paymentTypeLB_DoubleClick);
            // 
            // returnInvoiceBtn
            // 
            this.returnInvoiceBtn.Location = new System.Drawing.Point(364, 414);
            this.returnInvoiceBtn.Name = "returnInvoiceBtn";
            this.returnInvoiceBtn.Size = new System.Drawing.Size(98, 23);
            this.returnInvoiceBtn.TabIndex = 3;
            this.returnInvoiceBtn.Text = "Return to Invoice";
            this.returnInvoiceBtn.UseVisualStyleBackColor = true;
            this.returnInvoiceBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // printBtn
            // 
            this.printBtn.Enabled = false;
            this.printBtn.Location = new System.Drawing.Point(468, 414);
            this.printBtn.Name = "printBtn";
            this.printBtn.Size = new System.Drawing.Size(99, 23);
            this.printBtn.TabIndex = 4;
            this.printBtn.Text = "Print";
            this.printBtn.UseVisualStyleBackColor = true;
            // 
            // closeBtn
            // 
            this.closeBtn.Enabled = false;
            this.closeBtn.Location = new System.Drawing.Point(573, 414);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(88, 23);
            this.closeBtn.TabIndex = 5;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // Checkout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 450);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.printBtn);
            this.Controls.Add(this.returnInvoiceBtn);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Checkout";
            this.Text = "Checkout";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button acceptBtn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker customerBirthdaySelector;
        private System.Windows.Forms.TextBox ageRestrictedTB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox itemCountTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox taxRateTB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox totalTB;
        private System.Windows.Forms.TextBox taxAmtTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox taxableTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox subtotalTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox remainingBalanceTB;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListBox paymentsLB;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox paymentTypeLB;
        private System.Windows.Forms.Button returnInvoiceBtn;
        private System.Windows.Forms.Button printBtn;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Button finalizeBtn;
        private System.Windows.Forms.Button acceptPaymentBtn;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox checkNameTB;
        private System.Windows.Forms.TextBox checkNumberTB;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox paymentAmountTB;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox poTB;
        private System.Windows.Forms.TextBox attentionTB;
        private System.Windows.Forms.Button saveInvoiceBtn;
        private System.Windows.Forms.Button deletePaymentBtn;
        private System.Windows.Forms.TextBox paymentIndexTB;
        private System.Windows.Forms.Button cancelPaymentBtn;
        private System.Windows.Forms.Label changeDueLbl;
    }
}