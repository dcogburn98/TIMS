
namespace TIMS.Forms
{
    partial class PermissionsEditor
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Charge Accounts");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Cash Sales");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Create Held Invoices");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Create Invoices", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Retrieve Held Invoices");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Open Held Invoices");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("View Today\'s Invoices");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("View All Invoices");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("View Invoices", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Void Invoices");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Delete Held Invoices");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Release Invoices");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Edit Invoices", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11,
            treeNode12});
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Invoicing", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode9,
            treeNode13});
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("View Customers");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Add Customers");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Edit Customers");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Delete Customers");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Customers", new System.Windows.Forms.TreeNode[] {
            treeNode15,
            treeNode16,
            treeNode17,
            treeNode18});
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("View Permissions");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Edit Permissions");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Employees", new System.Windows.Forms.TreeNode[] {
            treeNode20,
            treeNode21});
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Permissions", new System.Windows.Forms.TreeNode[] {
            treeNode14,
            treeNode19,
            treeNode22});
            this.permissionsViewer = new System.Windows.Forms.TreeView();
            this.saveButton = new System.Windows.Forms.Button();
            this.discardButton = new System.Windows.Forms.Button();
            this.revertDefaultButton = new System.Windows.Forms.Button();
            this.employeeNumberTextBox = new System.Windows.Forms.TextBox();
            this.employeeNumberLabel = new System.Windows.Forms.Label();
            this.employeeNameTextBox = new System.Windows.Forms.TextBox();
            this.searchEmployeeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // permissionsViewer
            // 
            this.permissionsViewer.CheckBoxes = true;
            this.permissionsViewer.FullRowSelect = true;
            this.permissionsViewer.Indent = 30;
            this.permissionsViewer.Location = new System.Drawing.Point(12, 32);
            this.permissionsViewer.Name = "permissionsViewer";
            treeNode1.Name = "chargeAccounts";
            treeNode1.Text = "Charge Accounts";
            treeNode2.Name = "cashSales";
            treeNode2.Text = "Cash Sales";
            treeNode3.Name = "createHeldInvoices";
            treeNode3.Text = "Create Held Invoices";
            treeNode4.Name = "createInvoices";
            treeNode4.Text = "Create Invoices";
            treeNode5.Name = "retrieveHeldInvoices";
            treeNode5.Text = "Retrieve Held Invoices";
            treeNode6.Name = "openHeldInvoices";
            treeNode6.Text = "Open Held Invoices";
            treeNode7.Name = "viewTodaysInvoices";
            treeNode7.Text = "View Today\'s Invoices";
            treeNode8.Name = "viewAllInvoices";
            treeNode8.Text = "View All Invoices";
            treeNode9.Name = "viewInvoices";
            treeNode9.Text = "View Invoices";
            treeNode10.Name = "voidInvoices";
            treeNode10.Text = "Void Invoices";
            treeNode11.Name = "deleteHeldInvoices";
            treeNode11.Text = "Delete Held Invoices";
            treeNode12.Name = "releaseInvoices";
            treeNode12.Text = "Release Invoices";
            treeNode13.Name = "editInvoices";
            treeNode13.Text = "Edit Invoices";
            treeNode14.Name = "invoicing";
            treeNode14.Text = "Invoicing";
            treeNode15.Name = "viewCustomers";
            treeNode15.Text = "View Customers";
            treeNode16.Name = "addCustomers";
            treeNode16.Text = "Add Customers";
            treeNode17.Name = "editCustomers";
            treeNode17.Text = "Edit Customers";
            treeNode18.Name = "deleteCustomers";
            treeNode18.Text = "Delete Customers";
            treeNode19.Name = "customers";
            treeNode19.Text = "Customers";
            treeNode20.Name = "viewPermissions";
            treeNode20.Text = "View Permissions";
            treeNode21.Name = "editPermissions";
            treeNode21.Text = "Edit Permissions";
            treeNode22.Name = "employees";
            treeNode22.Text = "Employees";
            treeNode23.Name = "permissions";
            treeNode23.Text = "Permissions";
            this.permissionsViewer.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode23});
            this.permissionsViewer.Size = new System.Drawing.Size(825, 438);
            this.permissionsViewer.TabIndex = 0;
            this.permissionsViewer.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.permissionsViewer_BeforeCheck);
            this.permissionsViewer.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.permissionsViewer_AfterCheck);
            this.permissionsViewer.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.permissionsViewer_BeforeCollapse);
            this.permissionsViewer.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.permissionsViewer_BeforeExpand);
            this.permissionsViewer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.permissionsViewer_AfterSelect);
            this.permissionsViewer.DoubleClick += new System.EventHandler(this.permissionsViewer_DoubleClick);
            this.permissionsViewer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.permissionsViewer_MouseDown);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(762, 476);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // discardButton
            // 
            this.discardButton.Location = new System.Drawing.Point(659, 476);
            this.discardButton.Name = "discardButton";
            this.discardButton.Size = new System.Drawing.Size(97, 23);
            this.discardButton.TabIndex = 2;
            this.discardButton.Text = "Discard Changes";
            this.discardButton.UseVisualStyleBackColor = true;
            // 
            // revertDefaultButton
            // 
            this.revertDefaultButton.Location = new System.Drawing.Point(12, 476);
            this.revertDefaultButton.Name = "revertDefaultButton";
            this.revertDefaultButton.Size = new System.Drawing.Size(100, 23);
            this.revertDefaultButton.TabIndex = 3;
            this.revertDefaultButton.Text = "Revert to Default";
            this.revertDefaultButton.UseVisualStyleBackColor = true;
            // 
            // employeeNumberTextBox
            // 
            this.employeeNumberTextBox.Location = new System.Drawing.Point(111, 6);
            this.employeeNumberTextBox.Name = "employeeNumberTextBox";
            this.employeeNumberTextBox.Size = new System.Drawing.Size(79, 20);
            this.employeeNumberTextBox.TabIndex = 4;
            // 
            // employeeNumberLabel
            // 
            this.employeeNumberLabel.AutoSize = true;
            this.employeeNumberLabel.Location = new System.Drawing.Point(9, 9);
            this.employeeNumberLabel.Name = "employeeNumberLabel";
            this.employeeNumberLabel.Size = new System.Drawing.Size(96, 13);
            this.employeeNumberLabel.TabIndex = 5;
            this.employeeNumberLabel.Text = "Employee Number:";
            // 
            // employeeNameTextBox
            // 
            this.employeeNameTextBox.Location = new System.Drawing.Point(196, 6);
            this.employeeNameTextBox.Name = "employeeNameTextBox";
            this.employeeNameTextBox.Size = new System.Drawing.Size(254, 20);
            this.employeeNameTextBox.TabIndex = 6;
            // 
            // searchEmployeeButton
            // 
            this.searchEmployeeButton.Location = new System.Drawing.Point(456, 4);
            this.searchEmployeeButton.Name = "searchEmployeeButton";
            this.searchEmployeeButton.Size = new System.Drawing.Size(75, 23);
            this.searchEmployeeButton.TabIndex = 7;
            this.searchEmployeeButton.Text = "Search";
            this.searchEmployeeButton.UseVisualStyleBackColor = true;
            // 
            // PermissionsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 511);
            this.Controls.Add(this.searchEmployeeButton);
            this.Controls.Add(this.employeeNameTextBox);
            this.Controls.Add(this.employeeNumberLabel);
            this.Controls.Add(this.employeeNumberTextBox);
            this.Controls.Add(this.revertDefaultButton);
            this.Controls.Add(this.discardButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.permissionsViewer);
            this.Name = "PermissionsEditor";
            this.Text = "Permissions Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView permissionsViewer;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button discardButton;
        private System.Windows.Forms.Button revertDefaultButton;
        private System.Windows.Forms.TextBox employeeNumberTextBox;
        private System.Windows.Forms.Label employeeNumberLabel;
        private System.Windows.Forms.TextBox employeeNameTextBox;
        private System.Windows.Forms.Button searchEmployeeButton;
    }
}