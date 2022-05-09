namespace TIMS.Forms
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
            this.pagePreview1.Size = new System.Drawing.Size(776, 426);
            this.pagePreview1.TabIndex = 0;
            this.pagePreview1.ZoomPercent = 35;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(713, 444);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // ReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 477);
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
    }
}