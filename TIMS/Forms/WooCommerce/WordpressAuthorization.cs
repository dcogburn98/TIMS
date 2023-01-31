using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIMS.Forms.WooCommerce
{
    public partial class WordpressAuthorization : Form
    {
        public WordpressAuthorization()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(clientKey.Text) || string.IsNullOrEmpty(clientSecret.Text) || string.IsNullOrEmpty(pageURL.Text))
            {
                MessageBox.Show("Invalid URL, Client Key, or Client Secret.");
                return;
            }

            webView21.Source = new Uri(
                $"{pageURL.Text}/oauth1/authorize?oauth_consumer_key=TUPFNj1ZTd8u&oauth_token={clientKey.Text}&oauth_token_secret={clientSecret.Text}");
        }

        private void GetAuthTokens(string bareURL)
        {
            Uri url = new Uri(new Uri(bareURL), "oauth1/request");

            WebRequest request = WebRequest.Create(url.ToString());
            request.Method = "GET";
            //request.Headers.Add()

            WebResponse webResponse = request.GetResponse();
            Stream webStream = webResponse.GetResponseStream();

            StreamReader reader = new StreamReader(webStream);
            string data = reader.ReadToEnd();

            Console.WriteLine(data);
        }
    }
}
