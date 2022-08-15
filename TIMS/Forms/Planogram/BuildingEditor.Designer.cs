namespace TIMS.Forms.Planogram
{
    partial class BuildingEditor
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.widthNumeric = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.depthNumeric = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ceilingHeightNumeric = new System.Windows.Forms.NumericUpDown();
            this.acceptButton = new System.Windows.Forms.Button();
            this.floorSpaceProperties = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.depthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceilingHeightNumeric)).BeginInit();
            this.floorSpaceProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(600, 600);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(746, 620);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(665, 620);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // widthNumeric
            // 
            this.widthNumeric.Location = new System.Drawing.Point(84, 18);
            this.widthNumeric.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.widthNumeric.Name = "widthNumeric";
            this.widthNumeric.Size = new System.Drawing.Size(49, 20);
            this.widthNumeric.TabIndex = 7;
            this.widthNumeric.ValueChanged += new System.EventHandler(this.widthNumeric_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Width";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Feet";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(139, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Feet";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(43, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Depth";
            // 
            // depthNumeric
            // 
            this.depthNumeric.Location = new System.Drawing.Point(84, 44);
            this.depthNumeric.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.depthNumeric.Name = "depthNumeric";
            this.depthNumeric.Size = new System.Drawing.Size(49, 20);
            this.depthNumeric.TabIndex = 12;
            this.depthNumeric.ValueChanged += new System.EventHandler(this.depthNumeric_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(139, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Feet";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Ceiling Height";
            // 
            // ceilingHeightNumeric
            // 
            this.ceilingHeightNumeric.Location = new System.Drawing.Point(84, 70);
            this.ceilingHeightNumeric.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.ceilingHeightNumeric.Name = "ceilingHeightNumeric";
            this.ceilingHeightNumeric.Size = new System.Drawing.Size(49, 20);
            this.ceilingHeightNumeric.TabIndex = 16;
            this.ceilingHeightNumeric.ValueChanged += new System.EventHandler(this.ceilingHeightNumeric_ValueChanged);
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(74, 96);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 19;
            this.acceptButton.Text = "Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // floorSpaceProperties
            // 
            this.floorSpaceProperties.Controls.Add(this.label5);
            this.floorSpaceProperties.Controls.Add(this.acceptButton);
            this.floorSpaceProperties.Controls.Add(this.widthNumeric);
            this.floorSpaceProperties.Controls.Add(this.label4);
            this.floorSpaceProperties.Controls.Add(this.label3);
            this.floorSpaceProperties.Controls.Add(this.label2);
            this.floorSpaceProperties.Controls.Add(this.ceilingHeightNumeric);
            this.floorSpaceProperties.Controls.Add(this.depthNumeric);
            this.floorSpaceProperties.Controls.Add(this.label6);
            this.floorSpaceProperties.Controls.Add(this.label7);
            this.floorSpaceProperties.Location = new System.Drawing.Point(621, 12);
            this.floorSpaceProperties.Name = "floorSpaceProperties";
            this.floorSpaceProperties.Size = new System.Drawing.Size(200, 133);
            this.floorSpaceProperties.TabIndex = 20;
            this.floorSpaceProperties.TabStop = false;
            this.floorSpaceProperties.Text = "Floor Space Properties";
            // 
            // BuildingEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 655);
            this.Controls.Add(this.floorSpaceProperties);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.pictureBox1);
            this.Name = "BuildingEditor";
            this.Text = "Building Editor";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.depthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceilingHeightNumeric)).EndInit();
            this.floorSpaceProperties.ResumeLayout(false);
            this.floorSpaceProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.NumericUpDown widthNumeric;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown depthNumeric;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown ceilingHeightNumeric;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.GroupBox floorSpaceProperties;
    }
}