using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using WooCommerceNET.WooCommerce.v3;

namespace TIMSServer.WebServer.WooCommerce
{
    public class WebhookHandler
    {
        public static async void HandleWebhook(WebHeaderCollection headers, Stream jsonStream)
        {
            string jsonString = new StreamReader(jsonStream).ReadToEnd();
            JObject json = JObject.Parse(jsonString);
            foreach (string key in headers.AllKeys)
            {
                string header = headers.Get(key);
                //Console.WriteLine(key + ": \"" + header + "\"");
            }
            //Console.WriteLine(json);


            switch (headers.Get("X-WC-Webhook-Topic"))
            {
                case ("order.created"):
                    Console.WriteLine("An order was created with an id of " + json.Value<string>("id").ToString());
                    WooCommerceHandler.ProcessOrder(await WooCommerceHandler.WC.Order.Get(ulong.Parse(json.Value<string>("id").ToString())));
                    break;
            }
        }
    }
}
