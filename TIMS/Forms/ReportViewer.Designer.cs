﻿namespace TIMS.Forms
{
    partial class ReportViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportViewer));
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.pageSetupDialog2 = new System.Windows.Forms.PageSetupDialog();
            this.pagePreview1 = new PdfSharp.Forms.PagePreview();
            this.closeButton = new System.Windows.Forms.Button();
            this.printBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pagePreview1
            // 
            this.pagePreview1.DesktopColor = System.Drawing.SystemColors.ControlDark;
            this.pagePreview1.Location = new System.Drawing.Point(12, 12);
            this.pagePreview1.Name = "pagePreview1";
            this.pagePreview1.PageColor = System.Drawing.Color.GhostWhite;
            this.pagePreview1.PageGraphicsUnit = PdfSharp.Drawing.XGraphicsUnit.Point;
            this.pagePreview1.PageSize = ((PdfSharp.Drawing.XSize)(resources.GetObject("pagePreview1.PageSize")));
            this.pagePreview1.PageSizeF = new System.Drawing.Size(595, 842);
            this.pagePreview1.PageUnit = PdfSharp.Drawing.XGraphicsUnit.Point;
            this.pagePreview1.Size = new System.Drawing.Size(924, 600);
            this.pagePreview1.TabIndex = 0;
            this.pagePreview1.ZoomPercent = 51;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(861, 618);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // printBtn
            // 
            this.printBtn.Location = new System.Drawing.Point(780, 618);
            this.printBtn.Name = "printBtn";
            this.printBtn.Size = new System.Drawing.Size(75, 23);
            this.printBtn.TabIndex = 2;
            this.printBtn.Text = "Print";
            this.printBtn.UseVisualStyleBackColor = true;
            this.printBtn.Click += new System.EventHandler(this.printBtn_Click);
            // 
            // ReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 653);
            this.Controls.Add(this.printBtn);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.pagePreview1);
            this.Name = "ReportViewer";
            this.Text = "ReportViewer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog2;
        private PdfSharp.Forms.PagePreview pagePreview1;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button printBtn;
    }
}