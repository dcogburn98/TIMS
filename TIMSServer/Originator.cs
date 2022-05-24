using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TIMSServer
{
    public class Originator
    {
        public string OID;
        public IPAddress IP;
        public int port;
        public string Hash;

        public Originator(string OID, IPAddress IP, string Hash)
        {
            this.OID = OID;
            this.IP = IP;
            this.Hash = Hash;
            Thread listenerThread = new Thread(new ParameterizedThreadStart(TerminalListener));
            listenerThread.Start();
            port = Server.port + Server.terminals.Count + 1;
        }

        public void TerminalListener(object o)
        {
            TcpListener server = null;
            try
            {
                server = new TcpListener(Server.localAddr, port);
                Console.WriteLine(port);
                server.Start();
                byte[] bytes = new byte[65536];
                string data = null;

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    IPAddress IP = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                    if (!IPAddress.Equals(this.IP, IP))
                        return;
                    Console.WriteLine("Input from terminal " + IP.ToString());
                    Program.ReturnToInput();

                    data = null;
                    NetworkStream stream = client.GetStream();
                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);

                        Database.QueueCommand(data);
                        while (Database.ResponseQueue.Count < 1) { }
                        byte[] msg = Database.ResponseQueue.Dequeue().responseData;

                        stream.Write(msg, 0, msg.Length);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Server failure... Please save the following exception information for troubleshooting purposes.");
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                Console.WriteLine("Server failure... Server is haulted and server thread has aborted.");
                Console.WriteLine("Please attempt to restart the server or contact your network administrator.");
                server.Stop();
                Program.ReturnToInput();
            }
        }
    }
}
