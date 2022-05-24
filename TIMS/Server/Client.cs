using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TIMS.Server
{
    class Client
    {
        /// <summary>
        /// The port that the client is talking to the server over. Initially is 13000, but changes once a connection is established.
        /// </summary>
        public static int port = 13000;

        /// <summary>
        /// Sends a command to the server and returns the reply
        /// </summary>
        /// <param name="msg">The message to be sent</param>
        /// <returns></returns>
        public static string SendMessage(string msg)
        {
            return Connect(IPAddress.Loopback.ToString(), msg);
        }

        static string Connect(String server, String message)
        {
            try
            {
                TcpClient client = new TcpClient(server, port); //Create client for sending message to server
                byte[] data = Encoding.ASCII.GetBytes(message); //Convert to binary for transmission over network
                NetworkStream stream = client.GetStream();      //Get the stream to write binary data to
                stream.Write(data, 0, data.Length);             //Write it to the stream

                data = new byte[65535];                         //Initialize the data byte with an arbitrarily large number for the response

                int bytes = stream.Read(data, 0, data.Length);  //Read the binary response from the server
                string response = Encoding.ASCII.GetString(data, 0, bytes); //Convert it to a string

                if (port == 13000)                              //If the current port is 13000, we need to change it to the new communication port
                    if (!int.TryParse(response, out port))      //Try parsing the response from the server
                        port = 13000;

                stream.Close();
                client.Close();

                return response;
            }
            catch (ArgumentNullException e)
            {
                return e.Message;
            }
            catch (SocketException e)
            {
                return e.Message;
            }
        }
    }
}
