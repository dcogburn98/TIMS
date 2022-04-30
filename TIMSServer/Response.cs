using System;
using System.Collections.Generic;
using System.Text;

namespace TIMSServer
{
    public class Response
    {
        public string responseText = String.Empty; //Text version of SQL response
        public byte[] responseData; //Byte version of SQL response for sending over network to terminals

        public Response(string terminalID)
        {
            responseText = terminalID;

            responseData = Encoding.ASCII.GetBytes(responseText);
        }

        public Response(string terminalID, List<List<string>> response)
        {
            responseText = terminalID + "\0";
            int i = 0;
            foreach (List<string> row in response)
            {
                if (i != 0)
                    responseText += "\0";
                i++;
                int j = 0;
                foreach (string col in row)
                {
                    if (j != 0)
                        responseText += "\t";
                    j++;
                    responseText += col;
                }
            }

            responseData = Encoding.ASCII.GetBytes(responseText);
        }
    }
}
