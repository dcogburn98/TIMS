namespace TIMS.Forms.POS
{
    partial class ItemInformation
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
            this.nextPictureButton = new System.Windows.Forms.Button();
            this.prevPictureButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.itemNameTB = new System.Windows.Forms.TextBox();
            this.descriptionBrowser = new System.Windows.Forms.WebBrowser();
            this.label1 = new System.Windows.Forms.Label();
            this.onHandQtyLabel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.departmentLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.subdepartmentLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.brandLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.prevPictureButton);
            this.groupBox1.Controls.Add(this.nextPictureButton);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(415, 461);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Media";
            // 
            // nextPictureButton
            // 
            this.nextPictureButton.Location = new System.Drawing.Point(313, 426);
            this.nextPictureButton.Name = "nextPictureButton";
            this.nextPictureButton.Size = new System.Drawing.Size(75, 23);
            this.nextPictureButton.TabIndex = 1;
            this.nextPictureButton.Text = "Next";
            this.nextPictureButton.UseVisualStyleBackColor = true;
            this.nextPictureButton.Click += new System.EventHandler(this.nextPictureButton_Click);
            // 
            // prevPictureButton
            // 
            this.prevPictureButton.Location = new System.Drawing.Point(28, 425);
            this.prevPictureButton.Name = "prevPictureButton";
            this.prevPictureButton.Size = new System.Drawing.Size(75, 23);
            this.prevPictureButton.TabIndex = 2;
            this.prevPictureButton.Text = "Previous";
            this.prevPictureButton.UseVisualStyleBackColor = true;
            this.prevPictureButton.Click += new System.EventHandler(this.prevPictureButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.descriptionBrowser);
            this.groupBox2.Location = new System.Drawing.Point(433, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(547, 342);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Description";
            // 
            // itemNameTB
            // 
            this.itemNameTB.Enabled = false;
            this.itemNameTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemNameTB.ForeColor = System.Drawing.Color.Black;
            this.itemNameTB.Location = new System.Drawing.Point(433, 12);
            this.itemNameTB.Multiline = true;
            this.itemNameTB.Name = "itemNameTB";
            this.itemNameTB.Size = new System.Drawing.Size(547, 71);
            this.itemNameTB.TabIndex = 2;
            this.itemNameTB.Text = "Coastal Pet Herm Sprenger Fur Saver Link Dog Chain Training Collar 3.0mm Coastal " +
    "Pet Herm Sprenger Fur Saver Link Dog Chain Training Collar 3.0mm";
            this.itemNameTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // descriptionBrowser
            // 
            this.descriptionBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionBrowser.Location = new System.Drawing.Point(3, 16);
            this.descriptionBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.descriptionBrowser.Name = "descriptionBrowser";
            this.descriptionBrowser.Size = new System.Drawing.Size(541, 323);
            this.descriptionBrowser.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(433, 443);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Quantity Available:";
            // 
            // onHandQtyLabel
            // 
            this.onHandQtyLabel.AutoSize = true;
            this.onHandQtyLabel.Location = new System.Drawing.Point(534, 443);
            this.onHandQtyLabel.Name = "onHandQtyLabel";
            this.onHandQtyLabel.Size = new System.Drawing.Size(13, 13);
            this.onHandQtyLabel.TabIndex = 4;
            this.onHandQtyLabel.Text = "0";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.brandLabel);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.subdepartmentLabel);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.departmentLabel);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.categoryLabel);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(12, 479);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(254, 148);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Categorization";
            // 
            // categoryLabel
            // 
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Location = new System.Drawing.Point(94, 16);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(13, 13);
            this.categoryLabel.TabIndex = 6;
            this.categoryLabel.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Category:";
            // 
            // departmentLabel
            // 
            this.departmentLabel.AutoSize = true;
            this.departmentLabel.Location = new System.Drawing.Point(94, 29);
            this.departmentLabel.Name = "departmentLabel";
            this.departmentLabel.Size = new System.Drawing.Size(13, 13);
            this.departmentLabel.TabIndex = 8;
            this.departmentLabel.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Department:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // subdepartmentLabel
            // 
            this.subdepartmentLabel.AutoSize = true;
            this.subdepartmentLabel.Location = new System.Drawing.Point(94, 42);
            this.subdepartmentLabel.Name = "subdepartmentLabel";
            this.subdepartmentLabel.Size = new System.Drawing.Size(13, 13);
            this.subdepartmentLabel.TabIndex = 10;
            this.subdepartmentLabel.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Subdepartment:";
            // 
            // brandLabel
            // 
            this.brandLabel.AutoSize = true;
            this.brandLabel.Location = new System.Drawing.Point(50, 80);
            this.brandLabel.Name = "brandLabel";
            this.brandLabel.Size = new System.Drawing.Size(13, 13);
            this.brandLabel.TabIndex = 12;
            this.brandLabel.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 80);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Brand:";
            // 
            // ItemInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 639);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.onHandQtyLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.itemNameTB);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ItemInformation";
            this.Text = "ItemInformation";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button prevPictureButton;
        private System.Windows.Forms.Button nextPictureButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.WebBrowser descriptionBrowser;
        private System.Windows.Forms.TextBox itemNameTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label onHandQtyLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label departmentLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label categoryLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label brandLabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label subdepartmentLabel;
        private System.Windows.Forms.Label label8;
    }
}