namespace TIMS.Forms.WooCommerce
{
    partial class WordpressAuthorization
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
            this.authorizeBtn = new System.Windows.Forms.Button();
            this.OAuthSecret = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.OAuthToken = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.clientSecret = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.clientKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pageURL = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pageURL);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.authorizeBtn);
            this.groupBox1.Controls.Add(this.OAuthSecret);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.OAuthToken);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.clientSecret);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.clientKey);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(371, 216);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Integration";
            // 
            // authorizeBtn
            // 
            this.authorizeBtn.Location = new System.Drawing.Point(158, 97);
            this.authorizeBtn.Name = "authorizeBtn";
            this.authorizeBtn.Size = new System.Drawing.Size(75, 23);
            this.authorizeBtn.TabIndex = 9;
            this.authorizeBtn.Text = "Authorize";
            this.authorizeBtn.UseVisualStyleBackColor = true;
            this.authorizeBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // OAuthSecret
            // 
            this.OAuthSecret.Location = new System.Drawing.Point(79, 190);
            this.OAuthSecret.Name = "OAuthSecret";
            this.OAuthSecret.Size = new System.Drawing.Size(286, 20);
            this.OAuthSecret.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "OAuth Secret";
            // 
            // OAuthToken
            // 
            this.OAuthToken.Location = new System.Drawing.Point(79, 164);
            this.OAuthToken.Name = "OAuthToken";
            this.OAuthToken.Size = new System.Drawing.Size(286, 20);
            this.OAuthToken.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "OAuth Token";
            // 
            // clientSecret
            // 
            this.clientSecret.Location = new System.Drawing.Point(79, 71);
            this.clientSecret.Name = "clientSecret";
            this.clientSecret.Size = new System.Drawing.Size(286, 20);
            this.clientSecret.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Client Secret";
            // 
            // clientKey
            // 
            this.clientKey.Location = new System.Drawing.Point(79, 45);
            this.clientKey.Name = "clientKey";
            this.clientKey.Size = new System.Drawing.Size(286, 20);
            this.clientKey.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Client Key";
            // 
            // pageURL
            // 
            this.pageURL.Location = new System.Drawing.Point(96, 19);
            this.pageURL.Name = "pageURL";
            this.pageURL.Size = new System.Drawing.Size(269, 20);
            this.pageURL.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Homepage URL";
            // 
            // webView21
            // 
            this.webView21.AllowExternalDrop = true;
            this.webView21.CreationProperties = null;
            this.webView21.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView21.Location = new System.Drawing.Point(389, 12);
            this.webView21.Name = "webView21";
            this.webView21.Size = new System.Drawing.Size(737, 552);
            this.webView21.TabIndex = 2;
            this.webView21.ZoomFactor = 1D;
            // 
            // WordpressAuthorization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 576);
            this.Controls.Add(this.webView21);
            this.Controls.Add(this.groupBox1);
            this.Name = "WordpressAuthorization";
            this.Text = "WordpressAuthorization";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button authorizeBtn;
        private System.Windows.Forms.TextBox OAuthSecret;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox OAuthToken;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox clientSecret;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox clientKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pageURL;
        private System.Windows.Forms.Label label5;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
    }
}