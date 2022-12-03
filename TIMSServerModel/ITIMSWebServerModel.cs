using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PdfSharp.Drawing;

namespace TIMSServerModel
{
    [ServiceContract]
    public interface ITIMSWebServerModel
    {
        [OperationContract]
        [WebGet(UriTemplate = "{*filename}")]
        Stream GetResource(string filename);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Xml, UriTemplate = "{*filename}")]
        string FormPost(Stream testInput, string filename);
    }
}
