
namespace TIMS.Forms.POS
{
    partial class ReviewInvoices
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
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.customerNoAllCB = new System.Windows.Forms.CheckBox();
            this.invoiceNoAllCB = new System.Windows.Forms.CheckBox();
            this.dateAllCB = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.productLineAllCB = new System.Windows.Forms.CheckBox();
            this.selectProductLineBtn = new System.Windows.Forms.Button();
            this.productLineTB = new System.Windows.Forms.TextBox();
            this.productLineLB = new System.Windows.Forms.ListBox();
            this.customerNoTo = new System.Windows.Forms.TextBox();
            this.customerNoFrom = new System.Windows.Forms.TextBox();
            this.invNoTo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.invNoFrom = new System.Windows.Forms.TextBox();
            this.dateTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dateFrom = new System.Windows.Forms.DateTimePicker();
            this.formatPicker = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.itemNumberTB = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.itemNumberPrefixTB = new System.Windows.Forms.TextBox();
            this.itemNumberAllCB = new System.Windows.Forms.CheckBox();
            this.employeeNumberAllCB = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.employeeNoTo = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.employeeNoFrom = new System.Windows.Forms.TextBox();
            this.POAllCB = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.PONumberTo = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.PONumberFrom = new System.Windows.Forms.TextBox();
            this.totalAmountAllCB = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.totalAmountTo = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.totalAmountFrom = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.alternateFunctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invoicingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoiceNoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(449, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(371, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Review/Change Transactions";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.totalAmountAllCB);
            this.tabPage1.Controls.Add(this.label16);
            this.tabPage1.Controls.Add(this.totalAmountTo);
            this.tabPage1.Controls.Add(this.label17);
            this.tabPage1.Controls.Add(this.totalAmountFrom);
            this.tabPage1.Controls.Add(this.POAllCB);
            this.tabPage1.Controls.Add(this.label14);
            this.tabPage1.Controls.Add(this.PONumberTo);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.PONumberFrom);
            this.tabPage1.Controls.Add(this.employeeNumberAllCB);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.employeeNoTo);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.employeeNoFrom);
            this.tabPage1.Controls.Add(this.itemNumberAllCB);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.itemNumberPrefixTB);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.itemNumberTB);
            this.tabPage1.Controls.Add(this.customerNoAllCB);
            this.tabPage1.Controls.Add(this.invoiceNoAllCB);
            this.tabPage1.Controls.Add(this.dateAllCB);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.customerNoTo);
            this.tabPage1.Controls.Add(this.customerNoFrom);
            this.tabPage1.Controls.Add(this.invNoTo);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.invNoFrom);
            this.tabPage1.Controls.Add(this.dateTo);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.dateFrom);
            this.tabPage1.Controls.Add(this.formatPicker);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(804, 587);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Selection Criteria";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // customerNoAllCB
            // 
            this.customerNoAllCB.AutoSize = true;
            this.customerNoAllCB.Checked = true;
            this.customerNoAllCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.customerNoAllCB.Location = new System.Drawing.Point(584, 92);
            this.customerNoAllCB.Name = "customerNoAllCB";
            this.customerNoAllCB.Size = new System.Drawing.Size(37, 17);
            this.customerNoAllCB.TabIndex = 19;
            this.customerNoAllCB.Text = "All";
            this.customerNoAllCB.UseVisualStyleBackColor = true;
            this.customerNoAllCB.CheckedChanged += new System.EventHandler(this.customerNoAllCB_CheckedChanged);
            // 
            // invoiceNoAllCB
            // 
            this.invoiceNoAllCB.AutoSize = true;
            this.invoiceNoAllCB.Checked = true;
            this.invoiceNoAllCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.invoiceNoAllCB.Location = new System.Drawing.Point(584, 66);
            this.invoiceNoAllCB.Name = "invoiceNoAllCB";
            this.invoiceNoAllCB.Size = new System.Drawing.Size(37, 17);
            this.invoiceNoAllCB.TabIndex = 18;
            this.invoiceNoAllCB.Text = "All";
            this.invoiceNoAllCB.UseVisualStyleBackColor = true;
            this.invoiceNoAllCB.CheckedChanged += new System.EventHandler(this.invoiceNoAllCB_CheckedChanged);
            // 
            // dateAllCB
            // 
            this.dateAllCB.AutoSize = true;
            this.dateAllCB.Location = new System.Drawing.Point(584, 40);
            this.dateAllCB.Name = "dateAllCB";
            this.dateAllCB.Size = new System.Drawing.Size(37, 17);
            this.dateAllCB.TabIndex = 17;
            this.dateAllCB.Text = "All";
            this.dateAllCB.UseVisualStyleBackColor = true;
            this.dateAllCB.CheckedChanged += new System.EventHandler(this.dateAllCB_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(173, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(129, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Customer Number Range:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(182, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Invoice Number Range:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(435, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "to";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Menu;
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.productLineAllCB);
            this.panel1.Controls.Add(this.selectProductLineBtn);
            this.panel1.Controls.Add(this.productLineTB);
            this.panel1.Controls.Add(this.productLineLB);
            this.panel1.Location = new System.Drawing.Point(6, 116);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(792, 115);
            this.panel1.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(214, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Product Line(s):";
            // 
            // productLineAllCB
            // 
            this.productLineAllCB.AutoSize = true;
            this.productLineAllCB.Checked = true;
            this.productLineAllCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.productLineAllCB.Location = new System.Drawing.Point(407, 41);
            this.productLineAllCB.Name = "productLineAllCB";
            this.productLineAllCB.Size = new System.Drawing.Size(37, 17);
            this.productLineAllCB.TabIndex = 20;
            this.productLineAllCB.Text = "All";
            this.productLineAllCB.UseVisualStyleBackColor = true;
            // 
            // selectProductLineBtn
            // 
            this.selectProductLineBtn.Location = new System.Drawing.Point(404, 13);
            this.selectProductLineBtn.Name = "selectProductLineBtn";
            this.selectProductLineBtn.Size = new System.Drawing.Size(75, 23);
            this.selectProductLineBtn.TabIndex = 13;
            this.selectProductLineBtn.Text = "Select";
            this.selectProductLineBtn.UseVisualStyleBackColor = true;
            // 
            // productLineTB
            // 
            this.productLineTB.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.productLineTB.Location = new System.Drawing.Point(301, 15);
            this.productLineTB.Name = "productLineTB";
            this.productLineTB.Size = new System.Drawing.Size(97, 20);
            this.productLineTB.TabIndex = 11;
            this.productLineTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.productLineTB_KeyDown);
            this.productLineTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.productLineTB_KeyPress);
            this.productLineTB.Leave += new System.EventHandler(this.productLineTB_Leave);
            // 
            // productLineLB
            // 
            this.productLineLB.FormattingEnabled = true;
            this.productLineLB.Location = new System.Drawing.Point(485, 15);
            this.productLineLB.Name = "productLineLB";
            this.productLineLB.Size = new System.Drawing.Size(129, 82);
            this.productLineLB.TabIndex = 12;
            // 
            // customerNoTo
            // 
            this.customerNoTo.Location = new System.Drawing.Point(457, 90);
            this.customerNoTo.Name = "customerNoTo";
            this.customerNoTo.Size = new System.Drawing.Size(121, 20);
            this.customerNoTo.TabIndex = 10;
            this.customerNoTo.Enter += new System.EventHandler(this.customerNoTo_Enter);
            // 
            // customerNoFrom
            // 
            this.customerNoFrom.Location = new System.Drawing.Point(308, 90);
            this.customerNoFrom.Name = "customerNoFrom";
            this.customerNoFrom.Size = new System.Drawing.Size(121, 20);
            this.customerNoFrom.TabIndex = 9;
            this.customerNoFrom.TextChanged += new System.EventHandler(this.customerNoFrom_TextChanged);
            // 
            // invNoTo
            // 
            this.invNoTo.Location = new System.Drawing.Point(457, 64);
            this.invNoTo.Name = "invNoTo";
            this.invNoTo.Size = new System.Drawing.Size(121, 20);
            this.invNoTo.TabIndex = 8;
            this.invNoTo.Enter += new System.EventHandler(this.invNoTo_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(435, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "to";
            // 
            // invNoFrom
            // 
            this.invNoFrom.Location = new System.Drawing.Point(308, 64);
            this.invNoFrom.Name = "invNoFrom";
            this.invNoFrom.Size = new System.Drawing.Size(121, 20);
            this.invNoFrom.TabIndex = 6;
            this.invNoFrom.TextChanged += new System.EventHandler(this.invNoFrom_TextChanged);
            // 
            // dateTo
            // 
            this.dateTo.CustomFormat = "";
            this.dateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTo.Location = new System.Drawing.Point(457, 38);
            this.dateTo.Name = "dateTo";
            this.dateTo.Size = new System.Drawing.Size(121, 20);
            this.dateTo.TabIndex = 5;
            this.dateTo.Value = new System.DateTime(2022, 5, 22, 0, 0, 0, 0);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(435, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "to";
            // 
            // dateFrom
            // 
            this.dateFrom.CustomFormat = "";
            this.dateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateFrom.Location = new System.Drawing.Point(308, 38);
            this.dateFrom.Name = "dateFrom";
            this.dateFrom.Size = new System.Drawing.Size(121, 20);
            this.dateFrom.TabIndex = 3;
            this.dateFrom.Value = new System.DateTime(2022, 5, 22, 0, 0, 0, 0);
            // 
            // formatPicker
            // 
            this.formatPicker.FormattingEnabled = true;
            this.formatPicker.Items.AddRange(new object[] {
            "Invoice",
            "List"});
            this.formatPicker.Location = new System.Drawing.Point(308, 11);
            this.formatPicker.Name = "formatPicker";
            this.formatPicker.Size = new System.Drawing.Size(121, 21);
            this.formatPicker.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(234, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Date Range:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Invoice/List Result Format:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 58);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(812, 613);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(804, 587);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Invoice List";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dateColumn,
            this.invoiceNoColumn,
            this.customerColumn,
            this.employeeColumn,
            this.totalColumn,
            this.paymentType});
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(6, 6);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(792, 575);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDoubleClick);
            // 
            // itemNumberTB
            // 
            this.itemNumberTB.Location = new System.Drawing.Point(308, 237);
            this.itemNumberTB.Name = "itemNumberTB";
            this.itemNumberTB.Size = new System.Drawing.Size(270, 20);
            this.itemNumberTB.TabIndex = 20;
            this.itemNumberTB.TextChanged += new System.EventHandler(this.itemNumberTB_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(232, 240);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Item Number:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(266, 266);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Prefix:";
            // 
            // itemNumberPrefixTB
            // 
            this.itemNumberPrefixTB.Location = new System.Drawing.Point(308, 263);
            this.itemNumberPrefixTB.Name = "itemNumberPrefixTB";
            this.itemNumberPrefixTB.Size = new System.Drawing.Size(270, 20);
            this.itemNumberPrefixTB.TabIndex = 22;
            this.itemNumberPrefixTB.TextChanged += new System.EventHandler(this.itemNumberPrefixTB_TextChanged);
            // 
            // itemNumberAllCB
            // 
            this.itemNumberAllCB.AutoSize = true;
            this.itemNumberAllCB.Checked = true;
            this.itemNumberAllCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.itemNumberAllCB.Location = new System.Drawing.Point(584, 239);
            this.itemNumberAllCB.Name = "itemNumberAllCB";
            this.itemNumberAllCB.Size = new System.Drawing.Size(37, 17);
            this.itemNumberAllCB.TabIndex = 24;
            this.itemNumberAllCB.Text = "All";
            this.itemNumberAllCB.UseVisualStyleBackColor = true;
            this.itemNumberAllCB.CheckedChanged += new System.EventHandler(this.itemNumberAllCB_CheckedChanged);
            // 
            // employeeNumberAllCB
            // 
            this.employeeNumberAllCB.AutoSize = true;
            this.employeeNumberAllCB.Checked = true;
            this.employeeNumberAllCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.employeeNumberAllCB.Location = new System.Drawing.Point(584, 291);
            this.employeeNumberAllCB.Name = "employeeNumberAllCB";
            this.employeeNumberAllCB.Size = new System.Drawing.Size(37, 17);
            this.employeeNumberAllCB.TabIndex = 29;
            this.employeeNumberAllCB.Text = "All";
            this.employeeNumberAllCB.UseVisualStyleBackColor = true;
            this.employeeNumberAllCB.CheckedChanged += new System.EventHandler(this.employeeNumberAllCB_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(170, 292);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(131, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Employee Number Range:";
            // 
            // employeeNoTo
            // 
            this.employeeNoTo.Location = new System.Drawing.Point(457, 289);
            this.employeeNoTo.Name = "employeeNoTo";
            this.employeeNoTo.Size = new System.Drawing.Size(121, 20);
            this.employeeNoTo.TabIndex = 27;
            this.employeeNoTo.Enter += new System.EventHandler(this.employeeNoTo_Enter);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(435, 292);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(16, 13);
            this.label13.TabIndex = 26;
            this.label13.Text = "to";
            // 
            // employeeNoFrom
            // 
            this.employeeNoFrom.Location = new System.Drawing.Point(308, 289);
            this.employeeNoFrom.Name = "employeeNoFrom";
            this.employeeNoFrom.Size = new System.Drawing.Size(121, 20);
            this.employeeNoFrom.TabIndex = 25;
            this.employeeNoFrom.TextChanged += new System.EventHandler(this.employeeNoFrom_TextChanged);
            // 
            // POAllCB
            // 
            this.POAllCB.AutoSize = true;
            this.POAllCB.Checked = true;
            this.POAllCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.POAllCB.Location = new System.Drawing.Point(584, 316);
            this.POAllCB.Name = "POAllCB";
            this.POAllCB.Size = new System.Drawing.Size(37, 17);
            this.POAllCB.TabIndex = 34;
            this.POAllCB.Text = "All";
            this.POAllCB.UseVisualStyleBackColor = true;
            this.POAllCB.CheckedChanged += new System.EventHandler(this.POAllCB_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(142, 318);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(159, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "Purchase Order Number Range:";
            // 
            // PONumberTo
            // 
            this.PONumberTo.Location = new System.Drawing.Point(457, 315);
            this.PONumberTo.Name = "PONumberTo";
            this.PONumberTo.Size = new System.Drawing.Size(121, 20);
            this.PONumberTo.TabIndex = 32;
            this.PONumberTo.Enter += new System.EventHandler(this.PONumberTo_Enter);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(435, 317);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(16, 13);
            this.label15.TabIndex = 31;
            this.label15.Text = "to";
            // 
            // PONumberFrom
            // 
            this.PONumberFrom.Location = new System.Drawing.Point(308, 314);
            this.PONumberFrom.Name = "PONumberFrom";
            this.PONumberFrom.Size = new System.Drawing.Size(121, 20);
            this.PONumberFrom.TabIndex = 30;
            this.PONumberFrom.TextChanged += new System.EventHandler(this.PONumberFrom_TextChanged);
            // 
            // totalAmountAllCB
            // 
            this.totalAmountAllCB.AutoSize = true;
            this.totalAmountAllCB.Checked = true;
            this.totalAmountAllCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.totalAmountAllCB.Location = new System.Drawing.Point(584, 343);
            this.totalAmountAllCB.Name = "totalAmountAllCB";
            this.totalAmountAllCB.Size = new System.Drawing.Size(37, 17);
            this.totalAmountAllCB.TabIndex = 39;
            this.totalAmountAllCB.Text = "All";
            this.totalAmountAllCB.UseVisualStyleBackColor = true;
            this.totalAmountAllCB.CheckedChanged += new System.EventHandler(this.totalAmountAllCB_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(155, 344);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(146, 13);
            this.label16.TabIndex = 38;
            this.label16.Text = "Total Invoice Amount Range:";
            // 
            // totalAmountTo
            // 
            this.totalAmountTo.Location = new System.Drawing.Point(457, 341);
            this.totalAmountTo.Name = "totalAmountTo";
            this.totalAmountTo.Size = new System.Drawing.Size(121, 20);
            this.totalAmountTo.TabIndex = 37;
            this.totalAmountTo.Enter += new System.EventHandler(this.totalAmountTo_Enter);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(435, 344);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(16, 13);
            this.label17.TabIndex = 36;
            this.label17.Text = "to";
            // 
            // totalAmountFrom
            // 
            this.totalAmountFrom.Location = new System.Drawing.Point(308, 341);
            this.totalAmountFrom.Name = "totalAmountFrom";
            this.totalAmountFrom.Size = new System.Drawing.Size(121, 20);
            this.totalAmountFrom.TabIndex = 35;
            this.totalAmountFrom.TextChanged += new System.EventHandler(this.totalAmountFrom_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(584, 397);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 40;
            this.button2.Text = "Submit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alternateFunctionsToolStripMenuItem,
            this.invoicingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(836, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // alternateFunctionsToolStripMenuItem
            // 
            this.alternateFunctionsToolStripMenuItem.Name = "alternateFunctionsToolStripMenuItem";
            this.alternateFunctionsToolStripMenuItem.Size = new System.Drawing.Size(122, 20);
            this.alternateFunctionsToolStripMenuItem.Text = "Alternate Functions";
            this.alternateFunctionsToolStripMenuItem.Click += new System.EventHandler(this.alternateFunctionsToolStripMenuItem_Click);
            // 
            // invoicingToolStripMenuItem
            // 
            this.invoicingToolStripMenuItem.Name = "invoicingToolStripMenuItem";
            this.invoicingToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.invoicingToolStripMenuItem.Text = "Invoicing";
            this.invoicingToolStripMenuItem.Click += new System.EventHandler(this.invoicingToolStripMenuItem_Click);
            // 
            // dateColumn
            // 
            this.dateColumn.HeaderText = "Date";
            this.dateColumn.Name = "dateColumn";
            // 
            // invoiceNoColumn
            // 
            this.invoiceNoColumn.HeaderText = "Invoice Number";
            this.invoiceNoColumn.Name = "invoiceNoColumn";
            // 
            // customerColumn
            // 
            this.customerColumn.HeaderText = "Customer";
            this.customerColumn.Name = "customerColumn";
            // 
            // employeeColumn
            // 
            this.employeeColumn.HeaderText = "Employee";
            this.employeeColumn.Name = "employeeColumn";
            // 
            // totalColumn
            // 
            this.totalColumn.HeaderText = "Total";
            this.totalColumn.Name = "totalColumn";
            // 
            // paymentType
            // 
            this.paymentType.HeaderText = "Payment Type";
            this.paymentType.Name = "paymentType";
            // 
            // ReviewInvoices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 683);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ReviewInvoices";
            this.Text = "ReviewInvoices";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ReviewInvoices_FormClosed);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateFrom;
        private System.Windows.Forms.ComboBox formatPicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox customerNoAllCB;
        private System.Windows.Forms.CheckBox invoiceNoAllCB;
        private System.Windows.Forms.CheckBox dateAllCB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox productLineAllCB;
        private System.Windows.Forms.Button selectProductLineBtn;
        private System.Windows.Forms.TextBox productLineTB;
        private System.Windows.Forms.ListBox productLineLB;
        private System.Windows.Forms.TextBox customerNoTo;
        private System.Windows.Forms.TextBox customerNoFrom;
        private System.Windows.Forms.TextBox invNoTo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox invNoFrom;
        private System.Windows.Forms.DateTimePicker dateTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox totalAmountAllCB;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox totalAmountTo;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox totalAmountFrom;
        private System.Windows.Forms.CheckBox POAllCB;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox PONumberTo;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox PONumberFrom;
        private System.Windows.Forms.CheckBox employeeNumberAllCB;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox employeeNoTo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox employeeNoFrom;
        private System.Windows.Forms.CheckBox itemNumberAllCB;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox itemNumberPrefixTB;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox itemNumberTB;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem alternateFunctionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invoicingToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoiceNoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentType;
    }
}