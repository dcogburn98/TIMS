using System;
using System.Net;
using System.Net.Sockets;

namespace ClientTest
{
    class Program
    {
        public static int port = 13000;
        static void Main()
        {
            Console.WriteLine("Hello World!");
            while (true)
                Connect(IPAddress.Loopback.ToString(), Console.ReadLine());
        }

        static void Connect(String server, String message)
        {
            try
            {
                TcpClient client = new TcpClient(server, port);

                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                data = new Byte[65536];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0} ", responseData);
                if (port == 13000)
                    if (!int.TryParse(responseData, out port))
                        port = 13000;

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}
