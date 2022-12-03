using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TIMSServer.WebServer
{
    internal class MimetypeHelper
    {
        private Dictionary<string, string> baseMimetypes = null;
        private static MimetypeHelper INSTANCE = null;

        private MimetypeHelper()
        {
            baseMimetypes = new Dictionary<string, string>();
        }

        public static MimetypeHelper GetInstance()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new MimetypeHelper();
                INSTANCE.Load();
            }

            return INSTANCE;
        }

        public void Load()
        {
            XElement element = XElement.Load("MIMETypes.XML");

            IEnumerable<XElement> mimetypeElements = element.Element("MimeTypes").Elements();

            foreach (XElement mimetype in mimetypeElements)
            {
                string extension = mimetype.Attribute("fileExtension").Value;
                string type = mimetype.Attribute("type").Value;

                baseMimetypes.Add(extension, type);
            }
        }

        public string GetMimetype(string fileExtension)
        {
            string value = null;

            if (baseMimetypes.ContainsKey(fileExtension))
            {
                value = baseMimetypes[fileExtension];
            }

            return value;
        }
    }
}
