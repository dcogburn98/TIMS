using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Data.Sqlite;

using TIMSServer.WebServer;
using TIMSServer.WebServer.WooCommerce;
using TIMSServerModel;

namespace TIMSServer
{
    internal class TIMSWebServerModel : ITIMSWebServerModel
    {
        private static SqliteConnection sqlite_conn = Program.sqlite_conn;
        private static void OpenConnection()
        {
            Program.OpenConnection();
        }
        private static void CloseConnection()
        {
            Program.CloseConnection();
        }

        private static string basePath = null;
        private static MimetypeHelper baseMimetypeHelper = null;
        private static Dictionary<string, string> FriendlyURLs;
        private static Dictionary<string, string> MediaStrings;

        private static Dictionary<Guid, Session> sessions;
        
        public static void Init()
        {
            basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");

            baseMimetypeHelper = MimetypeHelper.GetInstance();

            FriendlyURLs = new Dictionary<string, string>();
            FriendlyURLs.Add("index", "index.html");
            FriendlyURLs.Add("product", "productPage.html");

            MediaStrings = new Dictionary<string, string>();
            MediaStrings.Add("CompanyLogo", "Company Logo");

            sessions = new Dictionary<Guid, Session>();
        }

        public TIMSWebServerModel()
        {
            
        }

        public Stream GetResource(string path)
        {
            string[] splitPath = path.ToLower().Split('/');
            Stream resourceStream;

            #region Session Management
            Session currentSession = new Session();
            string cookies = WebOperationContext.Current.IncomingRequest.Headers[HttpRequestHeader.Cookie]?.ToString();
            if (string.IsNullOrEmpty(cookies))
            {
                Guid id = Guid.NewGuid();
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Set-Cookie: SessionID=" + id.ToString() + ";");
                sessions.Add(id, new Session());
                sessions[id].guid = id;
                currentSession = sessions[id];
            }
            else
            {
                string[] split = cookies.Split(';');
                foreach (string cookie in split)
                {
                    if (cookie.Trim().Split('=')[0] == "SessionID")
                    {
                        if (Guid.TryParse(cookie.Trim().Split('=')[1], out Guid id))
                        {
                            try
                            {
                                currentSession = sessions[id];
                                break;
                            }
                            catch (KeyNotFoundException)
                            {
                                id = Guid.NewGuid();
                                WebOperationContext.Current.OutgoingResponse.Headers.Add("Set-Cookie: SessionID=" + id.ToString() + ";");
                                sessions.Add(id, new Session());
                                sessions[id].guid = id;
                                currentSession = sessions[id];
                                break;
                            }
                        }
                        else
                        {
                            id = Guid.NewGuid();
                            WebOperationContext.Current.OutgoingResponse.Headers.Add("Set-Cookie: SessionID=" + id.ToString() + ";");
                            sessions.Add(id, new Session());
                            sessions[id].guid = id;
                            currentSession = sessions[id];
                            break;
                        }
                    }
                }
            }
            #endregion

            if (string.IsNullOrEmpty(path)) //Default path (http://localhost/)
            {
                path = "index.html";
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                resourceStream = new MemoryStream(File.ReadAllBytes(Path.Combine(basePath, "index.html")));
                return resourceStream;
            }

            if (MediaStrings.ContainsKey(path))
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "image/png";
                resourceStream = new MemoryStream(RetrieveMedia(MediaStrings[path]));
                return resourceStream;
            }

            if (FriendlyURLs.ContainsKey(splitPath[0]))
            {
                FriendlyURLs.TryGetValue(splitPath[0], out splitPath[0]);
            }

            if (splitPath[0] == "productPage.html")
            {
                if (splitPath.Length != 2)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                    resourceStream = this.GetErrorResponseStream(500, "Internal Server Error");
                    return resourceStream;
                }
                else
                {
                    currentSession.item = new Item() { itemNumber = splitPath[1] };
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                    string html = File.ReadAllText(Path.Combine(basePath, splitPath[0]));
                    string parsed = ParseHTMLVariables(html, '#', currentSession);
                    resourceStream = new MemoryStream(Encoding.ASCII.GetBytes(parsed));
                    return resourceStream;
                }
            }

            try
            {

                string mimetype = baseMimetypeHelper.GetMimetype(Path.GetExtension(path));

                if (mimetype == null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                    resourceStream = this.GetErrorResponseStream(404, path);

                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = mimetype;
                    resourceStream = new MemoryStream(File.ReadAllBytes(Path.Combine(basePath, path)));
                }
            }
            catch (FileNotFoundException)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                resourceStream = this.GetErrorResponseStream(404, "Not Found");
            }
            catch (DirectoryNotFoundException)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                resourceStream = this.GetErrorResponseStream(404, "Not Found");
            }
            catch (Exception ex)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                resourceStream = this.GetErrorResponseStream(500, "Internal Server Error");
            }

            return resourceStream;
        }

        public string FormPost(Stream testInput)
        {
            if (WebOperationContext.Current.IncomingRequest.Headers.AllKeys.FirstOrDefault(el => el.ToLower().Contains("x-wc-webhook-topic")) != default(string))
            {
                WebhookHandler.HandleWebhook(WebOperationContext.Current.IncomingRequest.Headers, testInput);
            }

            StreamReader someReader = new StreamReader(testInput);
            string theInput = someReader.ReadToEnd();

            // Unfortunately, various places on the internet seem to indicate that data that you now have in your string
            // theInput is multipart form data.  If you were willingto use ASP.NET Compatibility Mode in your Interface, 
            // then you could have used the trick here mentioned by Mike Atlas and Mark Gravel in [2] but let's assume
            // you cannot use compatibilty mode.  Then you might try a multipart parser like the one written by antscode
            // at [3] or you can just do what I did and guess your way through.  Since you have a simple form, you can 
            // just guess your way through the parsing by replacing "testInput" with nothing and UrlDecoding what is left.
            // That won't work on more complex forms but it works on this example.  You get some more backround on this 
            // and why UrlDecode is necessary at [4]
            theInput = theInput.Replace("testInput=", "");
            //theInput = HttpUtility.UrlDecode(theInput);
            //return "Post paramether value: " + theInput;
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Set-Cookie: foo_b=bar_b;");
            return WebOperationContext.Current.IncomingRequest.Headers[HttpRequestHeader.Cookie]?.ToString()??"FAIL";
        }

        private Stream GetErrorResponseStream(int errorCode, string message)
        {
            string pageFormat = 
          @"<!DOCTYPE html>
            <html xmlns =\""http://www.w3.org/1999/xhtml\"">
                <head>
                    <title>Error</title>
                </head>
                <body> 
                    <font style=""font-size: 35px;"">{0} {1}</font>
                </body>
            </html>";

            return new MemoryStream(Encoding.ASCII.GetBytes
                       (string.Format(pageFormat, errorCode, message)));
        }

        private string ParseHTMLVariables(string html, char delimiter, Session sesh)
        {
            string newHTML = "";
            string[] split = html.Split(delimiter);
            for (int i = 0; i != split.Length; i++)
            {
                if (split[i] != "")
                    continue;

                if (split.Length <= i + 2)
                    continue;

                if (split[i + 2] != "")
                    continue;

                if (!Program.IsAlphanumeric(split[i + 1]))
                    continue;

                newHTML += split[i] + GetVariableValue(split[i + 1], sesh);
            }

            return newHTML;
        }

        private string GetVariableValue(string var, Session sesh)
        {
            if (var == "ItemNumber")
                return sesh.item.itemNumber;
            else
                return "NULL";
        }

        public byte[] RetrieveMedia(string key)
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"SELECT VALUE FROM MEDIA WHERE KEY = $KEY AND MEDIATYPE = ""Image""";
            command.Parameters.Add(new SqliteParameter("$KEY", key));
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return new byte[0];
            }

            byte[] imgBytes = new byte[1048576];
            while (reader.Read())
            {
                reader.GetBytes(0, 0, imgBytes, 0, 1048576);
            }

            CloseConnection();
            return imgBytes;
        }
    }
}
