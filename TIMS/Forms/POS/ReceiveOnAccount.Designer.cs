namespace TIMS.Forms.POS
{
    partial class ReceiveOnAccount
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
            this.accountNumberTB = new System.Windows.Forms.TextBox();
            this.accountNameTB = new System.Windows.Forms.TextBox();
            this.searchCustomerButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clearPaymentButton = new System.Windows.Forms.Button();
            this.newAccountBalance = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.submitPaymentButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.paymentMethodCB = new System.Windows.Forms.ComboBox();
            this.discountAmountTB = new System.Windows.Forms.TextBox();
            this.paymentAmountTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lastPaymentAmount = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.currentMonthPayments = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.maxDiscount = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.dueFurther = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.totalAccountBalance = new System.Windows.Forms.TextBox();
            this.lateFee = new System.Windows.Forms.TextBox();
            this.lastPaymentDate = new System.Windows.Forms.TextBox();
            this.serviceCharge = new System.Windows.Forms.TextBox();
            this.due90 = new System.Windows.Forms.TextBox();
            this.due60 = new System.Windows.Forms.TextBox();
            this.due30 = new System.Windows.Forms.TextBox();
            this.lastStatementAmount = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.totalDue = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.addInvoiceButton = new System.Windows.Forms.Button();
            this.invoiceNumber = new System.Windows.Forms.TextBox();
            this.removeInvoiceButton = new System.Windows.Forms.Button();
            this.outstandingInvoicesButton = new System.Windows.Forms.Button();
            this.invoicesLB = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.phoneLabel = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.address1Label = new System.Windows.Forms.Label();
            this.address2Label = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.statementCode = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.terms = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.billingType = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.secondaryTaxStatus = new System.Windows.Forms.TextBox();
            this.creditLimit = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.creditAvailable = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.primaryTaxStatus = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // accountNumberTB
            // 
            this.accountNumberTB.Location = new System.Drawing.Point(12, 25);
            this.accountNumberTB.Name = "accountNumberTB";
            this.accountNumberTB.Size = new System.Drawing.Size(100, 20);
            this.accountNumberTB.TabIndex = 0;
            this.accountNumberTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.accountNumberTB_KeyDown);
            // 
            // accountNameTB
            // 
            this.accountNameTB.Location = new System.Drawing.Point(118, 25);
            this.accountNameTB.Name = "accountNameTB";
            this.accountNameTB.Size = new System.Drawing.Size(327, 20);
            this.accountNameTB.TabIndex = 1;
            // 
            // searchCustomerButton
            // 
            this.searchCustomerButton.Location = new System.Drawing.Point(451, 22);
            this.searchCustomerButton.Name = "searchCustomerButton";
            this.searchCustomerButton.Size = new System.Drawing.Size(75, 23);
            this.searchCustomerButton.TabIndex = 2;
            this.searchCustomerButton.Text = "Search";
            this.searchCustomerButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Account Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(115, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Account Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clearPaymentButton);
            this.groupBox1.Controls.Add(this.newAccountBalance);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.submitPaymentButton);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.paymentMethodCB);
            this.groupBox1.Controls.Add(this.discountAmountTB);
            this.groupBox1.Controls.Add(this.paymentAmountTB);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(641, 293);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 160);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Payment";
            // 
            // clearPaymentButton
            // 
            this.clearPaymentButton.Location = new System.Drawing.Point(9, 124);
            this.clearPaymentButton.Name = "clearPaymentButton";
            this.clearPaymentButton.Size = new System.Drawing.Size(82, 23);
            this.clearPaymentButton.TabIndex = 13;
            this.clearPaymentButton.Text = "Clear";
            this.clearPaymentButton.UseVisualStyleBackColor = true;
            // 
            // newAccountBalance
            // 
            this.newAccountBalance.Location = new System.Drawing.Point(134, 71);
            this.newAccountBalance.Name = "newAccountBalance";
            this.newAccountBalance.Size = new System.Drawing.Size(127, 20);
            this.newAccountBalance.TabIndex = 12;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 74);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(117, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "New Account Balance:";
            // 
            // submitPaymentButton
            // 
            this.submitPaymentButton.Location = new System.Drawing.Point(168, 124);
            this.submitPaymentButton.Name = "submitPaymentButton";
            this.submitPaymentButton.Size = new System.Drawing.Size(93, 23);
            this.submitPaymentButton.TabIndex = 10;
            this.submitPaymentButton.Text = "Submit Payment";
            this.submitPaymentButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Payment Method:";
            // 
            // paymentMethodCB
            // 
            this.paymentMethodCB.FormattingEnabled = true;
            this.paymentMethodCB.Location = new System.Drawing.Point(102, 97);
            this.paymentMethodCB.Name = "paymentMethodCB";
            this.paymentMethodCB.Size = new System.Drawing.Size(159, 21);
            this.paymentMethodCB.TabIndex = 8;
            // 
            // discountAmountTB
            // 
            this.discountAmountTB.Location = new System.Drawing.Point(134, 45);
            this.discountAmountTB.Name = "discountAmountTB";
            this.discountAmountTB.Size = new System.Drawing.Size(127, 20);
            this.discountAmountTB.TabIndex = 7;
            // 
            // paymentAmountTB
            // 
            this.paymentAmountTB.Location = new System.Drawing.Point(134, 19);
            this.paymentAmountTB.Name = "paymentAmountTB";
            this.paymentAmountTB.Size = new System.Drawing.Size(127, 20);
            this.paymentAmountTB.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Discount:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Payment Amount:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lastPaymentAmount);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.currentMonthPayments);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.maxDiscount);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.dueFurther);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.totalAccountBalance);
            this.groupBox2.Controls.Add(this.lateFee);
            this.groupBox2.Controls.Add(this.lastPaymentDate);
            this.groupBox2.Controls.Add(this.serviceCharge);
            this.groupBox2.Controls.Add(this.due90);
            this.groupBox2.Controls.Add(this.due60);
            this.groupBox2.Controls.Add(this.due30);
            this.groupBox2.Controls.Add(this.lastStatementAmount);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.totalDue);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(317, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(318, 399);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Statement";
            // 
            // lastPaymentAmount
            // 
            this.lastPaymentAmount.BackColor = System.Drawing.SystemColors.Window;
            this.lastPaymentAmount.Enabled = false;
            this.lastPaymentAmount.Location = new System.Drawing.Point(132, 239);
            this.lastPaymentAmount.Name = "lastPaymentAmount";
            this.lastPaymentAmount.Size = new System.Drawing.Size(180, 20);
            this.lastPaymentAmount.TabIndex = 34;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 242);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(113, 13);
            this.label20.TabIndex = 33;
            this.label20.Text = "Last Payment Amount:";
            // 
            // currentMonthPayments
            // 
            this.currentMonthPayments.BackColor = System.Drawing.SystemColors.Window;
            this.currentMonthPayments.Enabled = false;
            this.currentMonthPayments.Location = new System.Drawing.Point(132, 320);
            this.currentMonthPayments.Name = "currentMonthPayments";
            this.currentMonthPayments.Size = new System.Drawing.Size(180, 20);
            this.currentMonthPayments.TabIndex = 32;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 323);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(126, 13);
            this.label19.TabIndex = 31;
            this.label19.Text = "Current Month Payments:";
            // 
            // maxDiscount
            // 
            this.maxDiscount.BackColor = System.Drawing.SystemColors.Window;
            this.maxDiscount.Enabled = false;
            this.maxDiscount.Location = new System.Drawing.Point(132, 372);
            this.maxDiscount.Name = "maxDiscount";
            this.maxDiscount.Size = new System.Drawing.Size(180, 20);
            this.maxDiscount.TabIndex = 30;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 375);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(75, 13);
            this.label18.TabIndex = 29;
            this.label18.Text = "Max Discount:";
            // 
            // dueFurther
            // 
            this.dueFurther.BackColor = System.Drawing.SystemColors.Window;
            this.dueFurther.Enabled = false;
            this.dueFurther.Location = new System.Drawing.Point(132, 149);
            this.dueFurther.Name = "dueFurther";
            this.dueFurther.Size = new System.Drawing.Size(180, 20);
            this.dueFurther.TabIndex = 28;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 152);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(66, 13);
            this.label17.TabIndex = 27;
            this.label17.Text = "Further Due:";
            // 
            // totalAccountBalance
            // 
            this.totalAccountBalance.BackColor = System.Drawing.Color.Yellow;
            this.totalAccountBalance.Enabled = false;
            this.totalAccountBalance.Location = new System.Drawing.Point(132, 346);
            this.totalAccountBalance.Name = "totalAccountBalance";
            this.totalAccountBalance.Size = new System.Drawing.Size(180, 20);
            this.totalAccountBalance.TabIndex = 26;
            // 
            // lateFee
            // 
            this.lateFee.BackColor = System.Drawing.SystemColors.Window;
            this.lateFee.Enabled = false;
            this.lateFee.Location = new System.Drawing.Point(132, 294);
            this.lateFee.Name = "lateFee";
            this.lateFee.Size = new System.Drawing.Size(180, 20);
            this.lateFee.TabIndex = 25;
            // 
            // lastPaymentDate
            // 
            this.lastPaymentDate.BackColor = System.Drawing.SystemColors.Window;
            this.lastPaymentDate.Enabled = false;
            this.lastPaymentDate.Location = new System.Drawing.Point(132, 268);
            this.lastPaymentDate.Name = "lastPaymentDate";
            this.lastPaymentDate.Size = new System.Drawing.Size(180, 20);
            this.lastPaymentDate.TabIndex = 24;
            // 
            // serviceCharge
            // 
            this.serviceCharge.BackColor = System.Drawing.SystemColors.Window;
            this.serviceCharge.Enabled = false;
            this.serviceCharge.Location = new System.Drawing.Point(132, 175);
            this.serviceCharge.Name = "serviceCharge";
            this.serviceCharge.Size = new System.Drawing.Size(180, 20);
            this.serviceCharge.TabIndex = 23;
            // 
            // due90
            // 
            this.due90.BackColor = System.Drawing.SystemColors.Window;
            this.due90.Enabled = false;
            this.due90.Location = new System.Drawing.Point(132, 123);
            this.due90.Name = "due90";
            this.due90.Size = new System.Drawing.Size(180, 20);
            this.due90.TabIndex = 22;
            // 
            // due60
            // 
            this.due60.BackColor = System.Drawing.SystemColors.Window;
            this.due60.Enabled = false;
            this.due60.Location = new System.Drawing.Point(132, 97);
            this.due60.Name = "due60";
            this.due60.Size = new System.Drawing.Size(180, 20);
            this.due60.TabIndex = 21;
            // 
            // due30
            // 
            this.due30.BackColor = System.Drawing.SystemColors.Window;
            this.due30.Enabled = false;
            this.due30.Location = new System.Drawing.Point(132, 71);
            this.due30.Name = "due30";
            this.due30.Size = new System.Drawing.Size(180, 20);
            this.due30.TabIndex = 20;
            // 
            // lastStatementAmount
            // 
            this.lastStatementAmount.BackColor = System.Drawing.Color.Yellow;
            this.lastStatementAmount.Enabled = false;
            this.lastStatementAmount.Location = new System.Drawing.Point(132, 19);
            this.lastStatementAmount.Name = "lastStatementAmount";
            this.lastStatementAmount.Size = new System.Drawing.Size(180, 20);
            this.lastStatementAmount.TabIndex = 19;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 297);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(52, 13);
            this.label15.TabIndex = 17;
            this.label15.Text = "Late Fee:";
            // 
            // totalDue
            // 
            this.totalDue.BackColor = System.Drawing.Color.Yellow;
            this.totalDue.Enabled = false;
            this.totalDue.Location = new System.Drawing.Point(132, 45);
            this.totalDue.Name = "totalDue";
            this.totalDue.Size = new System.Drawing.Size(180, 20);
            this.totalDue.TabIndex = 16;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(57, 13);
            this.label14.TabIndex = 15;
            this.label14.Text = "Total Due:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 271);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Last Payment Date:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 178);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Service Charge:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 126);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "90 Days:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 100);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "60 Days:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "30 Days:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 349);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Total Account Balance:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Last Statement Amount:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.addInvoiceButton);
            this.groupBox3.Controls.Add(this.invoiceNumber);
            this.groupBox3.Controls.Add(this.removeInvoiceButton);
            this.groupBox3.Controls.Add(this.outstandingInvoicesButton);
            this.groupBox3.Controls.Add(this.invoicesLB);
            this.groupBox3.Location = new System.Drawing.Point(641, 54);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(267, 233);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Invoices";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 178);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 13);
            this.label16.TabIndex = 5;
            this.label16.Text = "Invoice Number:";
            // 
            // addInvoiceButton
            // 
            this.addInvoiceButton.Location = new System.Drawing.Point(105, 204);
            this.addInvoiceButton.Name = "addInvoiceButton";
            this.addInvoiceButton.Size = new System.Drawing.Size(75, 23);
            this.addInvoiceButton.TabIndex = 4;
            this.addInvoiceButton.Text = "Add Invoice";
            this.addInvoiceButton.UseVisualStyleBackColor = true;
            // 
            // invoiceNumber
            // 
            this.invoiceNumber.Location = new System.Drawing.Point(105, 175);
            this.invoiceNumber.Name = "invoiceNumber";
            this.invoiceNumber.Size = new System.Drawing.Size(156, 20);
            this.invoiceNumber.TabIndex = 3;
            // 
            // removeInvoiceButton
            // 
            this.removeInvoiceButton.Location = new System.Drawing.Point(186, 204);
            this.removeInvoiceButton.Name = "removeInvoiceButton";
            this.removeInvoiceButton.Size = new System.Drawing.Size(75, 23);
            this.removeInvoiceButton.TabIndex = 2;
            this.removeInvoiceButton.Text = "Remove";
            this.removeInvoiceButton.UseVisualStyleBackColor = true;
            // 
            // outstandingInvoicesButton
            // 
            this.outstandingInvoicesButton.Location = new System.Drawing.Point(6, 204);
            this.outstandingInvoicesButton.Name = "outstandingInvoicesButton";
            this.outstandingInvoicesButton.Size = new System.Drawing.Size(75, 23);
            this.outstandingInvoicesButton.TabIndex = 1;
            this.outstandingInvoicesButton.Text = "Outstanding";
            this.outstandingInvoicesButton.UseVisualStyleBackColor = true;
            // 
            // invoicesLB
            // 
            this.invoicesLB.FormattingEnabled = true;
            this.invoicesLB.Location = new System.Drawing.Point(6, 19);
            this.invoicesLB.Name = "invoicesLB";
            this.invoicesLB.Size = new System.Drawing.Size(255, 147);
            this.invoicesLB.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.nameLabel);
            this.groupBox4.Controls.Add(this.phoneLabel);
            this.groupBox4.Controls.Add(this.label25);
            this.groupBox4.Controls.Add(this.address1Label);
            this.groupBox4.Controls.Add(this.address2Label);
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Location = new System.Drawing.Point(12, 54);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(299, 188);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Customer Information";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(9, 39);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(41, 13);
            this.nameLabel.TabIndex = 6;
            this.nameLabel.Text = "label27";
            // 
            // phoneLabel
            // 
            this.phoneLabel.AutoSize = true;
            this.phoneLabel.Location = new System.Drawing.Point(59, 161);
            this.phoneLabel.Name = "phoneLabel";
            this.phoneLabel.Size = new System.Drawing.Size(41, 13);
            this.phoneLabel.TabIndex = 5;
            this.phoneLabel.Text = "label26";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(6, 97);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(56, 13);
            this.label25.TabIndex = 4;
            this.label25.Text = "Address:";
            // 
            // address1Label
            // 
            this.address1Label.AutoSize = true;
            this.address1Label.Location = new System.Drawing.Point(6, 116);
            this.address1Label.Name = "address1Label";
            this.address1Label.Size = new System.Drawing.Size(41, 13);
            this.address1Label.TabIndex = 3;
            this.address1Label.Text = "label24";
            // 
            // address2Label
            // 
            this.address2Label.AutoSize = true;
            this.address2Label.Location = new System.Drawing.Point(6, 135);
            this.address2Label.Name = "address2Label";
            this.address2Label.Size = new System.Drawing.Size(41, 13);
            this.address2Label.TabIndex = 2;
            this.address2Label.Text = "label23";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(6, 161);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(51, 13);
            this.label22.TabIndex = 1;
            this.label22.Text = "Phone: ";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(6, 22);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(43, 13);
            this.label21.TabIndex = 0;
            this.label21.Text = "Name:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.statementCode);
            this.groupBox5.Controls.Add(this.label34);
            this.groupBox5.Controls.Add(this.terms);
            this.groupBox5.Controls.Add(this.label33);
            this.groupBox5.Controls.Add(this.billingType);
            this.groupBox5.Controls.Add(this.label32);
            this.groupBox5.Controls.Add(this.secondaryTaxStatus);
            this.groupBox5.Controls.Add(this.creditLimit);
            this.groupBox5.Controls.Add(this.label31);
            this.groupBox5.Controls.Add(this.label30);
            this.groupBox5.Controls.Add(this.creditAvailable);
            this.groupBox5.Controls.Add(this.label29);
            this.groupBox5.Controls.Add(this.primaryTaxStatus);
            this.groupBox5.Controls.Add(this.label28);
            this.groupBox5.Location = new System.Drawing.Point(12, 248);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(299, 205);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Billing Information";
            // 
            // statementCode
            // 
            this.statementCode.BackColor = System.Drawing.SystemColors.Window;
            this.statementCode.Enabled = false;
            this.statementCode.Location = new System.Drawing.Point(160, 175);
            this.statementCode.Name = "statementCode";
            this.statementCode.Size = new System.Drawing.Size(133, 20);
            this.statementCode.TabIndex = 33;
            this.statementCode.Visible = false;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(68, 180);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(86, 13);
            this.label34.TabIndex = 32;
            this.label34.Text = "Statement Code:";
            this.label34.Visible = false;
            // 
            // terms
            // 
            this.terms.BackColor = System.Drawing.Color.Yellow;
            this.terms.Enabled = false;
            this.terms.Location = new System.Drawing.Point(160, 149);
            this.terms.Name = "terms";
            this.terms.Size = new System.Drawing.Size(133, 20);
            this.terms.TabIndex = 31;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(115, 154);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(39, 13);
            this.label33.TabIndex = 30;
            this.label33.Text = "Terms:";
            // 
            // billingType
            // 
            this.billingType.BackColor = System.Drawing.SystemColors.Window;
            this.billingType.Enabled = false;
            this.billingType.Location = new System.Drawing.Point(160, 123);
            this.billingType.Name = "billingType";
            this.billingType.Size = new System.Drawing.Size(133, 20);
            this.billingType.TabIndex = 29;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(90, 126);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(64, 13);
            this.label32.TabIndex = 28;
            this.label32.Text = "Billing Type:";
            // 
            // secondaryTaxStatus
            // 
            this.secondaryTaxStatus.BackColor = System.Drawing.SystemColors.Window;
            this.secondaryTaxStatus.Enabled = false;
            this.secondaryTaxStatus.Location = new System.Drawing.Point(160, 97);
            this.secondaryTaxStatus.Name = "secondaryTaxStatus";
            this.secondaryTaxStatus.Size = new System.Drawing.Size(133, 20);
            this.secondaryTaxStatus.TabIndex = 27;
            // 
            // creditLimit
            // 
            this.creditLimit.BackColor = System.Drawing.Color.Yellow;
            this.creditLimit.Enabled = false;
            this.creditLimit.Location = new System.Drawing.Point(160, 19);
            this.creditLimit.Name = "creditLimit";
            this.creditLimit.Size = new System.Drawing.Size(133, 20);
            this.creditLimit.TabIndex = 25;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(39, 100);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(115, 13);
            this.label31.TabIndex = 26;
            this.label31.Text = "Secondary Tax Status:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(56, 74);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(98, 13);
            this.label30.TabIndex = 24;
            this.label30.Text = "Primary Tax Status:";
            // 
            // creditAvailable
            // 
            this.creditAvailable.BackColor = System.Drawing.Color.Yellow;
            this.creditAvailable.Enabled = false;
            this.creditAvailable.Location = new System.Drawing.Point(160, 45);
            this.creditAvailable.Name = "creditAvailable";
            this.creditAvailable.Size = new System.Drawing.Size(133, 20);
            this.creditAvailable.TabIndex = 23;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(71, 48);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(83, 13);
            this.label29.TabIndex = 22;
            this.label29.Text = "Credit Available:";
            // 
            // primaryTaxStatus
            // 
            this.primaryTaxStatus.BackColor = System.Drawing.SystemColors.Window;
            this.primaryTaxStatus.Enabled = false;
            this.primaryTaxStatus.Location = new System.Drawing.Point(160, 71);
            this.primaryTaxStatus.Name = "primaryTaxStatus";
            this.primaryTaxStatus.Size = new System.Drawing.Size(133, 20);
            this.primaryTaxStatus.TabIndex = 21;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(93, 22);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(61, 13);
            this.label28.TabIndex = 20;
            this.label28.Text = "Credit Limit:";
            // 
            // ReceiveOnAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 458);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchCustomerButton);
            this.Controls.Add(this.accountNameTB);
            this.Controls.Add(this.accountNumberTB);
            this.Name = "ReceiveOnAccount";
            this.Text = "ReceiveOnAccount";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox accountNumberTB;
        private System.Windows.Forms.TextBox accountNameTB;
        private System.Windows.Forms.Button searchCustomerButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox discountAmountTB;
        private System.Windows.Forms.TextBox paymentAmountTB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox paymentMethodCB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button clearPaymentButton;
        private System.Windows.Forms.TextBox newAccountBalance;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button submitPaymentButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button removeInvoiceButton;
        private System.Windows.Forms.Button outstandingInvoicesButton;
        private System.Windows.Forms.ListBox invoicesLB;
        private System.Windows.Forms.TextBox totalAccountBalance;
        private System.Windows.Forms.TextBox lateFee;
        private System.Windows.Forms.TextBox lastPaymentDate;
        private System.Windows.Forms.TextBox serviceCharge;
        private System.Windows.Forms.TextBox due90;
        private System.Windows.Forms.TextBox due60;
        private System.Windows.Forms.TextBox due30;
        private System.Windows.Forms.TextBox lastStatementAmount;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button addInvoiceButton;
        private System.Windows.Forms.TextBox invoiceNumber;
        private System.Windows.Forms.TextBox dueFurther;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox lastPaymentAmount;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox currentMonthPayments;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox maxDiscount;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox totalDue;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label phoneLabel;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label address1Label;
        private System.Windows.Forms.Label address2Label;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox statementCode;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox terms;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox billingType;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox secondaryTaxStatus;
        private System.Windows.Forms.TextBox creditLimit;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox creditAvailable;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox primaryTaxStatus;
        private System.Windows.Forms.Label label28;
    }
}