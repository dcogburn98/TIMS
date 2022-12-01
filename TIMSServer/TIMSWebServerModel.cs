using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

using TIMSServerModel;

namespace TIMSServer
{
    internal class TIMSWebServerModel : ITIMSWebServerModel
    {
        private string basePath = null;
        private MimetypeHelper baseMimetypeHelper = null;

        public TIMSWebServerModel()
        {
            basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");

            this.baseMimetypeHelper = MimetypeHelper.GetInstance();
        }

        public Stream GetResource(string path)
        {
            Stream resourceStream = null;

            if (string.IsNullOrEmpty(path))
            {
                path = "index.html";
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");
                resourceStream = new MemoryStream(File.ReadAllBytes(Path.Combine(basePath, "index.html")));
                return resourceStream;
            }

            try
            {

                string mimetype = this.baseMimetypeHelper.GetMimetype(Path.GetExtension(path));

                if (mimetype == null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                    WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");

                    resourceStream = this.GetErrorResponseStream(404, path);

                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = mimetype;
                    WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), mimetype);

                    resourceStream = new MemoryStream(File.ReadAllBytes(Path.Combine(basePath, path)));
                }
            }
            catch (FileNotFoundException)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");
                resourceStream = this.GetErrorResponseStream(404, "Not Found");
            }
            catch (DirectoryNotFoundException)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");
                resourceStream = this.GetErrorResponseStream(404, "Not Found");
            }
            catch (Exception ex)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");
                resourceStream = this.GetErrorResponseStream(500, "Internal Server Error");
            }

            return resourceStream;
        }

        public Stream GetCSS(string path)
        {
            Stream resourceStream = null;

            if (string.IsNullOrEmpty(path))
            {
                path = "index.html";
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");
                resourceStream = new MemoryStream(File.ReadAllBytes(Path.Combine(basePath, "index.html")));
                return resourceStream;
            }

            try
            {

                string mimetype = this.baseMimetypeHelper.GetMimetype(Path.GetExtension("css/" + path));

                if (mimetype == null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                    WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");

                    resourceStream = this.GetErrorResponseStream(404, "css/" + path);

                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = mimetype;
                    WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), mimetype);

                    resourceStream = new MemoryStream(File.ReadAllBytes(Path.Combine(basePath, "css/" + path)));
                }
            }
            catch (FileNotFoundException)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");
                resourceStream = this.GetErrorResponseStream(404, "css/" + path);
            }
            catch (DirectoryNotFoundException)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");
                resourceStream = this.GetErrorResponseStream(404, "css/" + path);
            }
            catch (Exception ex)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");
                resourceStream = this.GetErrorResponseStream(500, "Internal Server Error");
            }

            return resourceStream;
        }

        public Stream GetImage(string path)
        {
            Stream resourceStream = null;

            if (string.IsNullOrEmpty(path))
            {
                path = "index.html";
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");
                resourceStream = new MemoryStream(File.ReadAllBytes(Path.Combine(basePath, "index.html")));
                return resourceStream;
            }

            try
            {

                string mimetype = this.baseMimetypeHelper.GetMimetype(Path.GetExtension("images/" + path));

                if (mimetype == null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                    WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");

                    resourceStream = this.GetErrorResponseStream(404, "images/" + path);

                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = mimetype;
                    WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), mimetype);

                    resourceStream = new MemoryStream(File.ReadAllBytes(Path.Combine(basePath, "images/" + path)));
                }
            }
            catch (FileNotFoundException)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");
                resourceStream = this.GetErrorResponseStream(404, "images/" + path);
            }
            catch (DirectoryNotFoundException)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");
                resourceStream = this.GetErrorResponseStream(404, "images/" + path);
            }
            catch (Exception ex)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                WebOperationContext.Current.OutgoingRequest.Headers.Add(new HttpResponseHeader(), "text/html");
                resourceStream = this.GetErrorResponseStream(500, "Internal Server Error");
            }

            return resourceStream;
        }

        #region private

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

        #endregion
    }
}
